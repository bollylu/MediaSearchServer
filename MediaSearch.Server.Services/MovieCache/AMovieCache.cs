using BLTools.Text;

namespace MediaSearch.Server.Services;

public abstract class AMovieCache : IMovieCache, IMediaSearchLoggable<TMovieCache> {

  public const int DEFAULT_START_PAGE = 1;
  public const int DEFAULT_PAGE_SIZE = 20;

  public static char FOLDER_SEPARATOR = Path.DirectorySeparatorChar;

  public IMediaSearchLogger<TMovieCache> Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TMovieCache>();

  #region --- Internal data storage --------------------------------------------
  /// <summary>
  /// Store the movies
  /// </summary>
  protected readonly List<IMovie> _Items = new();

  protected readonly ReaderWriterLockSlim _LockCache = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
  #endregion --- Internal data storage --------------------------------------------

  public string RootStoragePath { get; init; } = "";

  #region --- IName --------------------------------------------
  public string Name { get; set; } = "";
  public string Description { get; set; } = "";
  #endregion --- IName --------------------------------------------

  #region --- Cache I/O --------------------------------------------
  protected virtual IEnumerable<IFileInfo> _FetchFiles() {
    return _FetchFiles(CancellationToken.None);
  }

  protected virtual IEnumerable<IFileInfo> _FetchFiles(CancellationToken token) {
    return new List<IFileInfo>();
  }

  public virtual Task Parse(CancellationToken token) {
    return Parse(_FetchFiles(token), token);
  }

  public virtual async Task Parse(IEnumerable<IFileInfo> fileSource, CancellationToken token) {
    Logger.Log("Initializing movies cache");
    Clear();

    foreach (IFileInfo FileItem in fileSource) {
      if (token.IsCancellationRequested) {
        return;
      }
      try {
        IMovie NewMovie = await _ParseEntry(FileItem);

        AddMovie(NewMovie);
      } catch (Exception ex) {
        Logger.LogWarning($"Unable to parse movie {FileItem} : {ex.Message}");
        if (ex.InnerException is not null) {
          Logger.LogWarning($"  {ex.InnerException.Message}");
        }
      }
    }

    Logger.Log("Cache initialized successfully");

    return;
  }

  protected virtual async Task<IMovie> _ParseEntry(IFileInfo item) {

    await Task.Yield();
    // Standardize directory separator
    string ProcessedFileItem = item.FullName.NormalizePath();

    TMovie RetVal = new TMovie() { Name = ProcessedFileItem.AfterLast(FOLDER_SEPARATOR).BeforeLast(" (") };

    RetVal.StorageRoot = RootStoragePath.NormalizePath();
    RetVal.StoragePath = ProcessedFileItem.BeforeLast(FOLDER_SEPARATOR).After(RetVal.StorageRoot, System.StringComparison.InvariantCultureIgnoreCase);

    RetVal.FileName = item.Name;
    RetVal.FileExtension = RetVal.FileName.AfterLast('.').ToLowerInvariant();

    IEnumerable<string> Tags = RetVal.StoragePath
                                      .BeforeLast(FOLDER_SEPARATOR)
                                      .Split(FOLDER_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);

    foreach (string TagItem in Tags.Where(t => !t.EndsWith(" #"))) {
      RetVal.Tags.Add(TagItem.TrimStart('[').TrimEnd(']'));
    }

    IEnumerable<string> GroupTags = Tags.Where(t => t.EndsWith(" #")).Select(t => t.TrimEnd(' ', '#'));
    RetVal.Group = string.Join("/", GroupTags);

    try {
      RetVal.CreationYear = int.Parse(RetVal.FileName.AfterLast('(').BeforeLast(')'));
    } catch (FormatException ex) {
      Logger.LogWarning($"Unable to find output year : {ex.Message} : {item.FullName}");
      RetVal.CreationYear = 0;
    }

    RetVal.Size = item.Length;

    //IMediaInfoFile DataFile = new TMovieInfoFileMeta(Path.Join(RetVal.StorageRoot, RetVal.StoragePath));
    //if (await DataFile.Exists()) {
    //  Logger.LogDebugExBox("Found datafile", DataFile);
    //  await DataFile.Read();
    //  //RetVal.MovieInfoContentMeta.Duplicate(DataFile.Content);
    //}

    return RetVal;
  }
  #endregion --- Cache I/O --------------------------------------------

  #region --- Cache management --------------------------------------------
  public void AddMovie(IMovie item) {
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
  public IMovie? GetMovie(string id) {
    try {
      _LockCache.EnterReadLock();
      return _Items.FirstOrDefault(x => x.Id == id);
    } finally {
      _LockCache.ExitReadLock();
    }
  }

  public IEnumerable<IMovie> GetAllMovies() {
    try {
      //LogDebugEx($"==> GetAllMovies() from cache");
      _LockCache.EnterReadLock();
      return _Items.OrderedBy(TFilter.Empty).Cast<IMovie>();
    } finally {
      //LogDebugEx($"<== GetAllMovies() from cache");
      _LockCache.ExitReadLock();
    }
  }

  public TMoviesPage? GetMoviesPage(IFilter filter) {
    Logger.LogDebug(filter.ToString().BoxFixedWidth("Filter", GlobalSettings.DEBUG_BOX_WIDTH));

    TMoviesPage RetVal = new TMoviesPage() {
      Source = RootStoragePath,
      Page = filter.Page
    };

    try {
      _LockCache.EnterReadLock();
      IList<IMovie> FilteredMovies = _Items.WithFilter(filter).OrderedBy(filter).Cast<IMovie>().ToList();
      RetVal.AvailableMovies = FilteredMovies.Count;
      RetVal.AvailablePages = (RetVal.AvailableMovies / filter.PageSize) + (RetVal.AvailableMovies % filter.PageSize > 0 ? 1 : 0);
      RetVal.Movies.AddRange(FilteredMovies.Skip(filter.PageSize * (filter.Page - 1)).Take(filter.PageSize));
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

