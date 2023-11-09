namespace MediaSearch.Server.Services;

// <summary>
/// Server Movie service. Provides access to groups, movies and pictures from NAS
/// </summary>
public class XMediaService : AMediaService {

  #region --- Constants --------------------------------------------
  public const int TIMEOUT_IN_MS = 500000;
  #endregion --- Constants --------------------------------------------

  private readonly IMediaCache _MoviesCache;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public XMediaService(IMediaCache movieCache) : base() {
    _MoviesCache = movieCache;
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

    Logger.Log($"Parsing data source : {RootStoragePath}");
    using (CancellationTokenSource Timeout = new CancellationTokenSource((int)TimeSpan.FromMinutes(5).TotalMilliseconds)) {
      IMediaMovieParser MovieParser = new TMediaMovieParserWindows(RootStoragePath);
      MovieParser.Init();
      Task DogetResults = Task.Run(() => {
        while (MovieParser.Results.Any() || !MovieParser.ParsingComplete) {
          MovieParser.Results.TryDequeue(out IMediaMovie? MovieItem);
          if (MovieItem is not null) {
            _MoviesCache.AddMedia(MovieItem);
          }
        }
      }, Timeout.Token);

      await Task.WhenAll(MovieParser.ParseFolderAsync(RootStoragePath), DogetResults);
    }
    _IsInitialized = true;
    _IsInitializing = false;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override Task Reset() {
    return Task.CompletedTask;
  }

  public override Task RefreshData() {
    return Task.CompletedTask;
  }

  public override ValueTask<int> GetRefreshStatus() {
    return ValueTask.FromResult(-1);
  }

  #region --- Movies --------------------------------------------
  public override async ValueTask<int> MediasCount(IFilter filter) {
    await Initialize().ConfigureAwait(false);
    return _MoviesCache.GetAll().WithFilter(filter).Count();
  }

  public override async ValueTask<int> PagesCount(IFilter filter) {
    await Initialize().ConfigureAwait(false);
    int FilteredMoviesCount = await MediasCount(filter).ConfigureAwait(false);
    return (FilteredMoviesCount / filter.PageSize) + (FilteredMoviesCount % filter.PageSize > 0 ? 1 : 0);
  }

  public override async IAsyncEnumerable<IMedia> GetAll() {
    await Initialize().ConfigureAwait(false);

    foreach (IMedia MovieItem in _MoviesCache.GetAll()) {
      yield return MovieItem;
    }
  }

  public override async Task<IMediasPage?> GetPage(IFilter filter) {
    await Initialize().ConfigureAwait(false);
    return _MoviesCache.GetPage(filter);
  }

  public override async Task<IMediasPage?> GetLastPage(IFilter filter) {
    await Initialize().ConfigureAwait(false);
    TFilter NewFilter = new TFilter(filter);
    NewFilter.Page = await PagesCount(filter);
    return _MoviesCache.GetPage(NewFilter);
  }

  public override async Task<IMedia?> Get(IRecord movie) {
    await Initialize().ConfigureAwait(false);
    if (string.IsNullOrWhiteSpace(movie.Id)) {
      Logger.LogWarning("Unable to retrieve movie : id is null or invalid");
      return null;
    }
    IMedia? Movie = _MoviesCache.Get(movie.Id);
    return Movie;
  }
  #endregion --- Movies --------------------------------------------

  public override async IAsyncEnumerable<string> GetGroups() {
    await Initialize().ConfigureAwait(false);
    await foreach (string GroupItem in _MoviesCache.GetGroups()) {
      yield return GroupItem;
    }
  }
  //public override async IAsyncEnumerable<string> GetSubGroups(string group) {
  //  await Task.Yield();

  //  foreach (string GroupItem in _MoviesCache.GetSubGroups(group)) {
  //    yield return GroupItem;
  //  }
  //}


  public override Task<byte[]> GetPicture(string id,
                                          string pictureName,
                                          int width,
                                          int height) {
    return Task.FromResult(Array.Empty<byte>());
  }

}

