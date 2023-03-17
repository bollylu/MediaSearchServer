namespace MediaSearch.Server.Services;

public abstract class AMovieService : ALoggable, IMovieService, IName {

  /// <summary>
  /// Root path to look for data
  /// </summary>
  public string RootStoragePath { get; init; } = "";

  //public IDataProvider DataProvider { get; protected set; } = new TDataProvider();

  protected AMovieService() {
    Logger = GlobalSettings.LoggerPool.GetLogger<AMovieService>();
  }

  #region --- IName --------------------------------------------
  /// <summary>
  /// The name of the source
  /// </summary>
  public string Name { get; set; } = "";

  /// <summary>
  /// The description of the source
  /// </summary>
  public string Description { get; set; } = "";
  #endregion --- IName --------------------------------------------

  /// <summary>
  /// The extensions of the files of interest
  /// </summary>
  public List<string> MoviesExtensions { get; } = new() { ".mkv", ".avi", ".mp4", ".iso" };

  public abstract IAsyncEnumerable<IMovie> GetAllMovies();
  public abstract IAsyncEnumerable<string> GetGroups();
  public abstract Task<IMovie?> GetMovie(IRecord id);
  public abstract Task<IMoviesPage?> GetMoviesLastPage(IFilter filter);
  public abstract Task<IMoviesPage?> GetMoviesPage(IFilter filter);
  public abstract Task<byte[]> GetPicture(string id, string pictureName, int width, int height);
  public abstract ValueTask<int> GetRefreshStatus();
  //public abstract IAsyncEnumerable<string> GetSubGroups(string group);
  public abstract Task Initialize();
  public abstract ValueTask<int> MoviesCount(IFilter filter);
  public abstract ValueTask<int> PagesCount(IFilter filter);
  public abstract Task RefreshData();
  public abstract Task Reset();
}
