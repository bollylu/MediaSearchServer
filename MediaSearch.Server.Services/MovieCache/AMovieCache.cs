using System.Globalization;

namespace MediaSearch.Server.Services;

public abstract class AMovieCache : ALoggable, IMovieCache {

  public const int DEFAULT_START_PAGE = 1;
  public const int DEFAULT_PAGE_SIZE = 20;

  public static char FOLDER_SEPARATOR = Path.DirectorySeparatorChar;

  #region --- Internal data storage --------------------------------------------
  /// <summary>
  /// Store the movies
  /// </summary>
  protected readonly List<IMovie> _Items = new();

  protected readonly ReaderWriterLockSlim _LockCache = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
  #endregion --- Internal data storage --------------------------------------------

  public string RootStoragePath { get; init; }

  #region --- IName --------------------------------------------
  public string Name { get; set; }
  public string Description { get; set; }
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

  public virtual Task Parse(IEnumerable<IFileInfo> fileSource, CancellationToken token) {
    Log("Initializing movies cache");
    Clear();

    foreach (IFileInfo FileItem in fileSource) {
      if (token.IsCancellationRequested) {
        return Task.FromCanceled(token);
      }
      try {
        IMovie NewMovie = _ParseEntry(FileItem);

        _Items.Add(NewMovie);
      } catch (Exception ex) {
        LogWarning($"Unable to parse movie {FileItem} : {ex.Message}");
        if (ex.InnerException is not null) {
          LogWarning($"  {ex.InnerException.Message}");
        }
      }
    }

    Log("Cache initialized successfully");

    return Task.CompletedTask;
  }

  protected virtual IMovie _ParseEntry(IFileInfo item) {

    IMovie RetVal = new TMovie();

    // Standardize directory separator
    string ProcessedFileItem = item.FullName.NormalizePath();

    RetVal.StorageRoot = RootStoragePath.NormalizePath();
    RetVal.StoragePath = ProcessedFileItem.BeforeLast(FOLDER_SEPARATOR).After(RetVal.StorageRoot, System.StringComparison.InvariantCultureIgnoreCase);

    RetVal.FileName = item.Name;
    RetVal.Name = ProcessedFileItem.AfterLast(FOLDER_SEPARATOR).BeforeLast(" (");
    RetVal.FileExtension = RetVal.FileName.AfterLast('.').ToLowerInvariant();

    string[] Tags = RetVal.StoragePath.BeforeLast(FOLDER_SEPARATOR).Split(FOLDER_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
    RetVal.Group = Tags.Any() ? Tags[0] : "(Unknown)";

    foreach (string TagItem in Tags) {
      RetVal.Tags.Add(TagItem);
    }

    try {
      RetVal.OutputYear = int.Parse(RetVal.FileName.AfterLast('(').BeforeLast(')'));
    } catch (FormatException ex) {
      LogWarning($"Unable to find output year : {ex.Message} : {item.FullName}");
      RetVal.OutputYear = 0;
    }

    RetVal.Size = item.Length;

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
  public IMovie GetMovie(string id) {
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
      return _Items.OrderedByName();
    } finally {
      //LogDebugEx($"<== GetAllMovies() from cache");
      _LockCache.ExitReadLock();
    }
  }

  public IMoviesPage GetMoviesPage(int startPage = DEFAULT_START_PAGE, int pageSize = DEFAULT_PAGE_SIZE) {
    return GetMoviesPage(TFilter.Empty, startPage, pageSize);
  }

  public IMoviesPage GetMoviesPage(TFilter filter, int startPage = DEFAULT_START_PAGE, int pageSize = DEFAULT_PAGE_SIZE) {
    LogDebugEx($"==> GetMoviesPage({startPage}, {pageSize})");

    IMoviesPage RetVal = new TMoviesPage() {
      Source = RootStoragePath,
      Page = startPage
    };

    try {
      _LockCache.EnterReadLock();
      IEnumerable<IMovie> FilteredMovies = _Items.FilterBy(filter).OrderedByName();
      RetVal.AvailableMovies = FilteredMovies.Count();
      RetVal.AvailablePages = (RetVal.AvailableMovies / pageSize) + (RetVal.AvailableMovies % pageSize > 0 ? 1 : 0);
      RetVal.Movies.AddRange(FilteredMovies.Skip(pageSize * (startPage - 1)).Take(pageSize));
      return RetVal;
    } catch (Exception ex) {
      LogError($"Unable to build a movie page : {ex.Message}");
      return null;
    } finally {
      _LockCache.ExitReadLock();
    }
  }

  #endregion --- Movies access --------------------------------------------

}

