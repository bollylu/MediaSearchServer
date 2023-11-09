namespace MediaSearch.Server.Services;

public abstract class AMediaService : ALoggable, IMediaService, IName {

  /// <summary>
  /// Root path to look for data
  /// </summary>
  public string RootStoragePath { get; init; } = "";

  //public IDataProvider DataProvider { get; protected set; } = new TDataProvider();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  protected AMediaService() {
    Logger = GlobalSettings.LoggerPool.GetLogger<AMediaService>();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

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

  public abstract IAsyncEnumerable<IMedia> GetAll();
  public abstract IAsyncEnumerable<string> GetGroups();
  public abstract Task<IMedia?> Get(IRecord id);
  public abstract Task<IMediasPage?> GetLastPage(IFilter filter);
  public abstract Task<IMediasPage?> GetPage(IFilter filter);
  public abstract Task<byte[]> GetPicture(string id, string pictureName, int width, int height);
  public abstract ValueTask<int> GetRefreshStatus();
  //public abstract IAsyncEnumerable<string> GetSubGroups(string group);
  public abstract Task Initialize();
  public abstract ValueTask<int> MediasCount(IFilter filter);
  public abstract ValueTask<int> PagesCount(IFilter filter);
  public abstract Task RefreshData();
  public abstract Task Reset();
}
