using System.Drawing;
using System.Drawing.Imaging;

namespace MovieSearchServerServices.MovieService;

// <summary>
/// Server Movie service. Provides access to groups, movies and pictures from NAS
/// </summary>
public class TMovieService : ALoggable, IMovieService, IName
{
    #region --- Constants --------------------------------------------
    public const int TIMEOUT_IN_MS = 5000;
    #endregion --- Constants --------------------------------------------

    /// <summary>
    /// Root path to look for data
    /// </summary>
    public string Storage { get; init; }

    /// <summary>
    /// The name of the source
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The description of the source
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The extensions of the files of interest
    /// </summary>
    public List<string> MoviesExtensions { get; } = new() { ".mkv", ".avi", ".mp4", ".iso" };

    private readonly IEnumerable<IFileInfo> _DataSource;
    private readonly IMovieCache _MoviesCache = new TMovieCache();

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TMovieService(string storage)
    {
        _MoviesCache.SetLogger(Logger);
        Storage = storage;
        _MoviesCache = new TMovieCache() { Storage = storage };
        _DataSource = _MoviesCache.FetchFiles();
    }

    public TMovieService(IMovieCache movieCache)
    {
        _MoviesCache = movieCache;
        _MoviesCache.SetLogger(Logger);
        Storage = movieCache.Storage;
        _DataSource = _MoviesCache.FetchFiles();
    }
    public TMovieService(IEnumerable<IFileInfo> files, string storage, string storageName = "(anonymous)")
    {
        _MoviesCache.SetLogger(Logger);
        Storage = storage;
        Name = storageName;
        _DataSource = files;
    }

    private bool _IsInitialized = false;
    private bool _IsInitializing = false;
    public async Task Initialize()
    {
        if (_IsInitialized)
        {
            return;
        }

        if (_IsInitializing)
        {
            return;
        }

        if (_MoviesCache.Any())
        {
            return;
        }

        _IsInitializing = true;

        Log($"Parsing data source : {Storage}");
        using (CancellationTokenSource Timeout = new CancellationTokenSource((int)TimeSpan.FromMinutes(5).TotalMilliseconds))
        {
            await _MoviesCache.Parse(_DataSource, Timeout.Token).ConfigureAwait(false);
        }

        _IsInitialized = true;
        _IsInitializing = false;
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public void Reset()
    {
        _MoviesCache.Clear();
        _IsInitialized = false;
    }

    public async Task Refresh()
    {
        Reset();
        await Initialize();
    }

    #region --- ILoggable --------------------------------------------
    public override void SetLogger(ILogger logger)
    {
        base.SetLogger(logger);
        _MoviesCache.SetLogger(logger);
    } 
    #endregion --- ILoggable --------------------------------------------

    #region --- Movies --------------------------------------------
    public async ValueTask<int> MoviesCount(string filter = "")
    {
        await Initialize().ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(filter))
        {
            return _MoviesCache.Count();
        }
        else
        {
            return _MoviesCache.GetAllMovies().Count(m => m.FileName.Contains(filter, StringComparison.CurrentCultureIgnoreCase));
        }
    }

    public async ValueTask<int> PagesCount(int pageSize = IMovieService.DEFAULT_PAGE_SIZE)
    {
        int AllMoviesCount = await MoviesCount().ConfigureAwait(false);
        return (AllMoviesCount / pageSize) + (AllMoviesCount % pageSize > 0 ? 1 : 0);
    }

    public async ValueTask<int> PagesCount(string filter, int pageSize = IMovieService.DEFAULT_PAGE_SIZE)
    {
        if (string.IsNullOrWhiteSpace(filter))
        {
            return await PagesCount(pageSize).ConfigureAwait(false);
        }

        int FilteredMoviesCount = await MoviesCount(filter).ConfigureAwait(false);
        return (FilteredMoviesCount / pageSize) + (FilteredMoviesCount % pageSize > 0 ? 1 : 0);
    }

    public async IAsyncEnumerable<TMovie> GetAllMovies()
    {
        await Initialize().ConfigureAwait(false);

        foreach (TMovie MovieItem in _MoviesCache.GetAllMovies().OrderBy(m => m.FileName).ThenBy(m => m.OutputYear))
        {
            yield return MovieItem;
        }
    }

    public async IAsyncEnumerable<TMovie> GetMovies(int startPage = 1, int pageSize = 20)
    {
        await Initialize().ConfigureAwait(false);

        foreach (TMovie MovieItem in _MoviesCache.GetAllMovies().Skip((startPage - 1) * pageSize).Take(pageSize))
        {
            yield return MovieItem;
        }
    }

    public async IAsyncEnumerable<TMovie> GetMovies(string filter = "", int startPage = 1, int pageSize = 20)
    {
        await Initialize().ConfigureAwait(false);

        foreach (TMovie MovieItem in _MoviesCache.GetAllMovies()
                                                 .Where(m => filter is null ? true : m.FileName.Contains(filter, StringComparison.CurrentCultureIgnoreCase))
                                                 .Skip((startPage - 1) * pageSize)
                                                 .Take(pageSize))
        {
            yield return MovieItem;
        }
    }
    #endregion --- Movies --------------------------------------------


    public async Task<byte[]> GetPicture(string picturePath,
                                         string pictureName = IMovieService.DEFAULT_PICTURE_NAME,
                                         int width = IMovieService.DEFAULT_PICTURE_WIDTH,
                                         int height = IMovieService.DEFAULT_PICTURE_HEIGHT)
    {

        string FullPicturePath = Path.Combine(Storage, picturePath, pictureName);

        LogDebug($"GetPicture {FullPicturePath} : size({width}, {height})");

        try
        {
            if (!File.Exists(FullPicturePath))
            {
                LogError($"Unable to fetch picture {FullPicturePath} : File is missing or access is denied");
                return null;
            }
            using (CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT_IN_MS))
            {
                using (FileStream SourceStream = new FileStream(FullPicturePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
                {
                    using (MemoryStream PictureStream = new())
                    {
                        await SourceStream.CopyToAsync(PictureStream, Timeout.Token);
                        Image Picture = Image.FromStream(PictureStream);
                        Bitmap ResizedPicture = new Bitmap(Picture, width, height);
                        using (MemoryStream OutputStream = new())
                        {
                            ResizedPicture.Save(OutputStream, ImageFormat.Jpeg);
                            return OutputStream.ToArray();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogError($"Unable to fetch picture {FullPicturePath} : {ex.Message}");
            return null;
        }
    }

}

