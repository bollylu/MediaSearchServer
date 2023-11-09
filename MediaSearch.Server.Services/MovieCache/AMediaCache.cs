using BLTools.Text;

namespace MediaSearch.Server.Services;

public abstract class AMediaCache : ALoggable, IMediaCache {

  public const int DEFAULT_START_PAGE = 1;
  public const int DEFAULT_PAGE_SIZE = 20;

  public static char FOLDER_SEPARATOR = Path.DirectorySeparatorChar;

  #region --- Internal data storage --------------------------------------------
  /// <summary>
  /// Store the movies
  /// </summary>
  protected readonly List<IMedia> _Items = new();

  protected readonly ReaderWriterLockSlim _LockCache = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
  #endregion --- Internal data storage --------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  protected AMediaCache() {
    Logger = GlobalSettings.LoggerPool.GetLogger<AMediaCache>();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public string RootStoragePath { get; init; } = "";

  #region --- IName --------------------------------------------
  public string Name { get; set; } = "";
  public string Description { get; set; } = "";
  #endregion --- IName --------------------------------------------

  #region --- Cache management --------------------------------------------
  public void AddMedia(IMedia item) {
    try {
      _LockCache.EnterWriteLock();
      _Items.Add(item);
    } finally {
      _LockCache.ExitWriteLock();
    }
  }
  public void Clear() {
    try {
      _LockCache.EnterWriteLock();
      _Items.Clear();
    } finally {
      _LockCache.ExitWriteLock();
    }
  }

  public bool Any() {
    try {
      _LockCache.EnterReadLock();
      return _Items.Any();
    } finally {
      _LockCache.ExitReadLock();
    }
  }

  public bool IsEmpty() {
    try {
      _LockCache.EnterReadLock();
      return _Items.IsEmpty();
    } finally {
      _LockCache.ExitReadLock();
    }
  }
  public int Count() {
    try {
      _LockCache.EnterReadLock();
      return _Items.Count;
    } finally {
      _LockCache.ExitReadLock();
    }
  }
  #endregion --- Cache management --------------------------------------------

  #region --- Movies access --------------------------------------------
  public IMedia? Get(string id) {
    try {
      _LockCache.EnterReadLock();
      return _Items.FirstOrDefault(x => x.Id == id);
    } finally {
      _LockCache.ExitReadLock();
    }
  }

  public IEnumerable<IMedia> GetAll() {
    try {
      //LogDebugEx("==> GetAll() from cache");
      _LockCache.EnterReadLock();
      return _Items.OrderedBy(TFilter.Empty);
    } finally {
      //LogDebugEx("<== GetAll() from cache");
      _LockCache.ExitReadLock();
    }
  }

  public IMediasPage? GetPage(IFilter filter) {
    Logger.LogDebug(filter.ToString() ?? "".BoxFixedWidth("Filter", GlobalSettings.DEBUG_BOX_WIDTH));

    TMediasPage RetVal = new TMediasPage() {
      Source = RootStoragePath,
      Page = filter.Page
    };

    try {
      _LockCache.EnterReadLock();
      IList<IMedia> FilteredMovies = _Items.WithFilter(filter).OrderedBy(filter).ToList();
      RetVal.AvailableMedias = FilteredMovies.Count;
      RetVal.AvailablePages = (RetVal.AvailableMedias / filter.PageSize) + (RetVal.AvailableMedias % filter.PageSize > 0 ? 1 : 0);
      RetVal.Medias.AddRange(FilteredMovies.Skip(filter.PageSize * (filter.Page - 1)).Take(filter.PageSize));
      return RetVal;
    } catch (Exception ex) {
      Logger.LogError($"Unable to build a movie page : {ex.Message}");
      return null;
    } finally {
      _LockCache.ExitReadLock();
    }
  }
  #endregion --- Movies access --------------------------------------------

  #region --- Groups --------------------------------------------
  public async IAsyncEnumerable<string> GetGroups() {
    try {
      //LogDebugEx($"==> GetGroups() from cache");
      _LockCache.EnterReadLock();
      await foreach (string GroupItem in _Items.GetGroups()) {
        yield return GroupItem;
      }
    } finally {
      //LogDebugEx($"<== GetGroups() from cache");
      _LockCache.ExitReadLock();
    }
  }

  //public IEnumerable<string> GetSubGroups(string group) {
  //  try {
  //    //LogDebugEx($"==> GetSubGroups() from cache");
  //    _LockCache.EnterReadLock();
  //    return _Items.GetSubGroups(group);
  //  } finally {
  //    //LogDebugEx($"<== GetSubGroups() from cache");
  //    _LockCache.ExitReadLock();
  //  }
  //}
  #endregion --- Groups --------------------------------------------
}

