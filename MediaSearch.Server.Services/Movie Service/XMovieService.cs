namespace MediaSearch.Server.Services;

// <summary>
/// Server Movie service. Provides access to groups, movies and pictures from NAS
/// </summary>
public class XMovieService : AMovieService {

  #region --- Constants --------------------------------------------
  public const int TIMEOUT_IN_MS = 500000;
  #endregion --- Constants --------------------------------------------

  private readonly IMovieCache _MoviesCache;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public XMovieService(IMovieCache movieCache) : base() {
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
      await _MoviesCache.Parse(Timeout.Token).ConfigureAwait(false);
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
  public override async ValueTask<int> MoviesCount(IFilter filter) {
    await Initialize().ConfigureAwait(false);
    return _MoviesCache.GetAllMovies().WithFilter(filter).Count();
  }

  public override async ValueTask<int> PagesCount(IFilter filter) {
    await Initialize().ConfigureAwait(false);
    int FilteredMoviesCount = await MoviesCount(filter).ConfigureAwait(false);
    return (FilteredMoviesCount / filter.PageSize) + (FilteredMoviesCount % filter.PageSize > 0 ? 1 : 0);
  }

  public override async IAsyncEnumerable<IMovie> GetAllMovies() {
    await Initialize().ConfigureAwait(false);

    foreach (TMovie MovieItem in _MoviesCache.GetAllMovies()) {
      yield return MovieItem;
    }
  }

  public override async Task<IMoviesPage?> GetMoviesPage(IFilter filter) {
    await Initialize().ConfigureAwait(false);
    return _MoviesCache.GetMoviesPage(filter);
  }

  public override async Task<IMoviesPage?> GetMoviesLastPage(IFilter filter) {
    await Initialize().ConfigureAwait(false);
    TFilter NewFilter = new TFilter(filter);
    NewFilter.Page = await PagesCount(filter);
    return _MoviesCache.GetMoviesPage(NewFilter);
  }

  public override async Task<IMovie?> GetMovie(IRecord movie) {
    await Initialize().ConfigureAwait(false);
    if (string.IsNullOrWhiteSpace(movie.Id)) {
      Logger.LogWarning("Unable to retrieve movie : id is null or invalid");
      return null;
    }
    IMovie? Movie = _MoviesCache.GetMovie(movie.Id);
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

