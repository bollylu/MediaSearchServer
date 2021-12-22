using SkiaSharp;

namespace MediaSearch.Server.Services;

// <summary>
/// Server Movie service. Provides access to groups, movies and pictures from NAS
/// </summary>
public class TMovieService : ALoggable, IMovieService, IName {
  #region --- Constants --------------------------------------------
  public const int TIMEOUT_IN_MS = 500000;
  #endregion --- Constants --------------------------------------------

  /// <summary>
  /// Root path to look for data
  /// </summary>
  public string RootStoragePath { get; init; }

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
  public TMovieService(string storage) {
    _MoviesCache.SetLogger(Logger);
    RootStoragePath = storage;
    _MoviesCache = new TMovieCache() { Storage = storage };
    _DataSource = _MoviesCache.FetchFiles();
  }

  public TMovieService(IMovieCache movieCache) {
    _MoviesCache = movieCache;
    _MoviesCache.SetLogger(Logger);
    RootStoragePath = movieCache.Storage;
    _DataSource = _MoviesCache.FetchFiles();
  }
  public TMovieService(IEnumerable<IFileInfo> files, string storage, string storageName = "(anonymous)") {
    _MoviesCache.SetLogger(Logger);
    RootStoragePath = storage;
    Name = storageName;
    _DataSource = files;
  }

  private bool _IsInitialized = false;
  private bool _IsInitializing = false;
  public async Task Initialize() {
    if (_IsInitialized) {
      return;
    }

    if (_IsInitializing) {
      return;
    }

    if (_MoviesCache.Any()) {
      return;
    }

    _IsInitializing = true;

    Log($"Parsing data source : {RootStoragePath}");
    using (CancellationTokenSource Timeout = new CancellationTokenSource((int)TimeSpan.FromMinutes(5).TotalMilliseconds)) {
      await _MoviesCache.Parse(_DataSource, Timeout.Token).ConfigureAwait(false);
    }

    _IsInitialized = true;
    _IsInitializing = false;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public void Reset() {
    _MoviesCache.Clear();
    _IsInitialized = false;
  }

  public async Task RefreshData() {
    Reset();
    await Initialize();
  }

  public int GetRefreshStatus() {
    if (_IsInitialized) {
      return -1;
    }
    return _MoviesCache.Count();
  }

  #region --- ILoggable --------------------------------------------
  public override void SetLogger(ILogger logger) {
    base.SetLogger(logger);
    _MoviesCache.SetLogger(logger);
  }
  #endregion --- ILoggable --------------------------------------------

  #region --- Movies --------------------------------------------
  public async ValueTask<int> MoviesCount(RFilter filter) {
    await Initialize().ConfigureAwait(false);

    return _MoviesCache.GetAllMovies().FilterByName(filter.Name).FilterByDays(filter.DaysBack).Count();
  }

  public async ValueTask<int> PagesCount(int pageSize = IMovieService.DEFAULT_PAGE_SIZE) {
    return await PagesCount(RFilter.Empty, pageSize).ConfigureAwait(false);
  }

  public async ValueTask<int> PagesCount(RFilter filter, int pageSize = IMovieService.DEFAULT_PAGE_SIZE) {
    int FilteredMoviesCount = await MoviesCount(filter).ConfigureAwait(false);
    return (FilteredMoviesCount / pageSize) + (FilteredMoviesCount % pageSize > 0 ? 1 : 0);
  }

  public async IAsyncEnumerable<TMovie> GetAllMovies() {
    await Initialize().ConfigureAwait(false);

    foreach (TMovie MovieItem in _MoviesCache.GetAllMovies()) {
      yield return MovieItem;
    }
  }

  public async Task<IMoviesPage> GetMoviesPage(int startPage = 1, int pageSize = IMovieService.DEFAULT_PAGE_SIZE) {
    await Initialize().ConfigureAwait(false);
    return await GetMoviesPage(RFilter.Empty, startPage, pageSize);
  }

  public async Task<IMoviesPage> GetMoviesPage(RFilter filter, int startPage = 1, int pageSize = 20) {
    await Initialize().ConfigureAwait(false);
    if (startPage > 1) {
      startPage = startPage.WithinLimits(1, await PagesCount(filter, pageSize));
    }
    return _MoviesCache.GetMoviesPage(filter, startPage, pageSize);
  }

  public async Task<IMoviesPage> GetMoviesLastPage(RFilter filter, int pageSize = 20) {
    await Initialize().ConfigureAwait(false);

    return _MoviesCache.GetMoviesPage(filter, await PagesCount(filter, pageSize), pageSize);
  }

  public Task<IMovie> GetMovie(string id) {
    if (string.IsNullOrWhiteSpace(id)) {
      LogWarning("Unable to retrieve movie : id is null or invalid");
      return null;
    }
    IMovie Movie = _MoviesCache.GetMovie(id);
    return Task.FromResult(Movie);

  }
  #endregion --- Movies --------------------------------------------

  public async Task<byte[]> GetPicture(string id,
                                       string pictureName,
                                       int width,
                                       int height) {

    #region === Validate parameters ===
    string ParamPictureName = pictureName ?? IMovieService.DEFAULT_PICTURE_NAME;
    int ParamWidth = width.WithinLimits(IMovieService.MIN_PICTURE_WIDTH, IMovieService.MAX_PICTURE_WIDTH);
    int ParamHeight = height.WithinLimits(IMovieService.MIN_PICTURE_HEIGHT, IMovieService.MAX_PICTURE_HEIGHT);
    if (string.IsNullOrWhiteSpace(id)) {
      LogError("Unable to fetch picture : id is null or invalid");
      return null;
    }
    string ParamId = id;
    #endregion === Validate parameters ===

    IMovie Movie = _MoviesCache.GetMovie(ParamId);
    if (Movie is null) {
      LogError($"Unable to fetch picture id \"{ParamId}\"");
      return null;
    }

    string FullPicturePath = Path.Join(RootStoragePath.NormalizePath(), Movie.StoragePath.NormalizePath(), ParamPictureName);

    LogDebug($"GetPicture {FullPicturePath} : size({ParamWidth}, {ParamHeight})");
    if (!File.Exists(FullPicturePath)) {
      LogError($"Unable to fetch picture {FullPicturePath} : File is missing or access is denied");
      return null;
    }

    try {
      using CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT_IN_MS);
      using FileStream SourceStream = new FileStream(FullPicturePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
      using MemoryStream PictureStream = new MemoryStream();
      await SourceStream.CopyToAsync(PictureStream, Timeout.Token);
      PictureStream.Seek(0, SeekOrigin.Begin);
      SKImage Image = SKImage.FromEncodedData(PictureStream);
      SKBitmap Picture = SKBitmap.FromImage(Image);
      SKBitmap ResizedPicture = Picture.Resize(new SKImageInfo(width, height), SKFilterQuality.High);
      SKData Result = ResizedPicture.Encode(SKEncodedImageFormat.Jpeg, 100);
      using (MemoryStream OutputStream = new()) {
        Result.SaveTo(OutputStream);
        return OutputStream.ToArray();
      }
    } catch (Exception ex) {
      LogError($"Unable to fetch picture {FullPicturePath} : {ex.Message}");
      if (ex.InnerException is not null) {
        LogError($"  {ex.InnerException.Message}");
      }
      return null;
    }
  }

}

