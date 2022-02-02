using SkiaSharp;

namespace MediaSearch.Server.Services;

// <summary>
/// Server Movie service. Provides access to groups, movies and pictures from NAS
/// </summary>
public class TMovieService : AMovieService {
  #region --- Constants --------------------------------------------
  public const int TIMEOUT_IN_MS = 500000;
  #endregion --- Constants --------------------------------------------

  private readonly IMovieCache _MoviesCache = new TMovieCache();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMovieService() {
    SetLogger(new TConsoleLogger());
    _MoviesCache.SetLogger(new TConsoleLogger());
  }
  
  public TMovieService(string storage) {
    _MoviesCache.SetLogger(Logger);
    RootStoragePath = storage;
    _MoviesCache = new TMovieCache() { RootStoragePath = storage };
  }

  public TMovieService(IMovieCache movieCache) {
    _MoviesCache = movieCache;
    _MoviesCache.SetLogger(Logger);
    RootStoragePath = movieCache.RootStoragePath;
  }

  private bool _IsInitialized = false;
  private bool _IsInitializing = false;
  public override async Task Initialize() {
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
      await _MoviesCache.Parse(Timeout.Token).ConfigureAwait(false);
    }

    _IsInitialized = true;
    _IsInitializing = false;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override void Reset() {
    _MoviesCache.Clear();
    _IsInitialized = false;
  }

  public override Task RefreshData() {
    Reset();
    return Task.Run(async () => await Initialize());
  }

  public override int GetRefreshStatus() {
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
  public override async ValueTask<int> MoviesCount(IFilter filter) {
    await Initialize().ConfigureAwait(false);
    return _MoviesCache.GetAllMovies().WithFilter(filter).Count();
  }

  public override async ValueTask<int> PagesCount(IFilter filter) {
    int FilteredMoviesCount = await MoviesCount(filter).ConfigureAwait(false);
    return (FilteredMoviesCount / filter.PageSize) + (FilteredMoviesCount % filter.PageSize > 0 ? 1 : 0);
  }

  public override async IAsyncEnumerable<TMovie> GetAllMovies() {
    await Initialize().ConfigureAwait(false);

    foreach (TMovie MovieItem in _MoviesCache.GetAllMovies()) {
      yield return MovieItem;
    }
  }

  public override async Task<TMoviesPage?> GetMoviesPage(IFilter filter) {
    await Initialize().ConfigureAwait(false);
    return _MoviesCache.GetMoviesPage(filter);
  }

  public override async Task<TMoviesPage?> GetMoviesLastPage(IFilter filter) {
    await Initialize().ConfigureAwait(false);
    TFilter NewFilter = new TFilter(filter);
    NewFilter.Page = await PagesCount(filter);
    return _MoviesCache.GetMoviesPage(NewFilter);
  }

  public override Task<IMovie?> GetMovie(string id) {
    if (string.IsNullOrWhiteSpace(id)) {
      LogWarning("Unable to retrieve movie : id is null or invalid");
      return Task.FromResult<IMovie?>(null);
    }
    IMovie? Movie = _MoviesCache.GetMovie(id);
    return Task.FromResult<IMovie?>(Movie);
  }
  #endregion --- Movies --------------------------------------------

  public override async IAsyncEnumerable<string> GetGroups() {
    await Initialize().ConfigureAwait(false);

    foreach (string GroupItem in _MoviesCache.GetGroups()) {
      yield return GroupItem;
    }
  }
  public override async IAsyncEnumerable<string> GetSubGroups(string group) {
    await Initialize().ConfigureAwait(false);

    foreach (string GroupItem in _MoviesCache.GetSubGroups(group)) {
      yield return GroupItem;
    }
  }


  public override async Task<byte[]> GetPicture(string id,
                                       string pictureName,
                                       int width,
                                       int height) {

    #region === Validate parameters ===
    string ParamPictureName = pictureName ?? IMovieService.DEFAULT_PICTURE_NAME;
    int ParamWidth = width.WithinLimits(IMovieService.MIN_PICTURE_WIDTH, IMovieService.MAX_PICTURE_WIDTH);
    int ParamHeight = height.WithinLimits(IMovieService.MIN_PICTURE_HEIGHT, IMovieService.MAX_PICTURE_HEIGHT);
    if (string.IsNullOrWhiteSpace(id)) {
      LogError("Unable to fetch picture : id is null or invalid");
      return Array.Empty<byte>();
    }
    string ParamId = id;
    #endregion === Validate parameters ===

    IMovie? Movie = _MoviesCache.GetMovie(ParamId);
    if (Movie is null) {
      LogError($"Unable to fetch picture id \"{ParamId}\"");
      return Array.Empty<byte>();
    }

    string FullPicturePath = Path.Join(RootStoragePath.NormalizePath(), Movie.StoragePath.NormalizePath(), ParamPictureName);

    LogDebug($"GetPicture {FullPicturePath} : size({ParamWidth}, {ParamHeight})");
    if (!File.Exists(FullPicturePath)) {
      LogError($"Unable to fetch picture {FullPicturePath} : File is missing or access is denied");
      return Array.Empty<byte>();
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
      return Array.Empty<byte>();
    }
  }

}

