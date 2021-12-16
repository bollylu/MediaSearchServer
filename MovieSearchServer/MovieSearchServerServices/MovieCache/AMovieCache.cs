using BLTools.Encryption;

using MovieSearch.Services;

namespace MovieSearchServerServices.MovieService;

public abstract class AMovieCache : ALoggable, IMovieCache {

  public const int DEFAULT_START_PAGE = 1;
  public const int DEFAULT_PAGE_SIZE = 20;

  public static char FOLDER_SEPARATOR = Path.DirectorySeparatorChar;

  #region --- Internal data storage --------------------------------------------
  /// <summary>
  /// Store the movies
  /// </summary>
  protected readonly SortedList<string, IMovie> _Items = new();

  protected readonly ReaderWriterLockSlim _LockCache = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
  #endregion --- Internal data storage --------------------------------------------

  public string Storage { get; init; }

  #region --- IName --------------------------------------------
  public string Name { get; set; }
  public string Description { get; set; }
  #endregion --- IName --------------------------------------------

  #region --- Cache I/O --------------------------------------------
  public virtual IEnumerable<IFileInfo> FetchFiles() {
    return FetchFiles(CancellationToken.None);
  }

  public abstract IEnumerable<IFileInfo> FetchFiles(CancellationToken token);

  public virtual Task Parse(IEnumerable<IFileInfo> fileSource, CancellationToken token) {
    Log("Initializing movies cache");
    Clear();

    foreach (IFileInfo FileItem in fileSource) {
      if (token.IsCancellationRequested) {
        return Task.FromCanceled(token);
      }
      try {
        IMovie NewMovie = _ParseEntry(FileItem);

        _Items.Add($"{NewMovie.Id}", NewMovie);
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

    RetVal.StorageRoot = Storage.NormalizePath();
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
      _Items.Add(item.Id, item);
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
      return _Items.FirstOrDefault(x => x.Key == id).Value;
    } finally {
      _LockCache.ExitReadLock();
    }
  }

  public IEnumerable<IMovie> GetMoviesWithGroup() {
    try {
      _LockCache.EnterReadLock();
      return _Items.Values.Where(m => !string.IsNullOrWhiteSpace(m.Group));
    } finally {
      _LockCache.ExitReadLock();
    }
  }

  public IEnumerable<IMovie> GetAllMovies() {
    try {
      Log($"==> GetAllMovies() from cache");
      _LockCache.EnterReadLock();
      foreach (KeyValuePair<string, IMovie> MovieItem in _Items) {
        yield return MovieItem.Value;
      }
    } finally {
      Log($"<== GetAllMovies() from cache");
      _LockCache.ExitReadLock();
    }
  }

  public IEnumerable<IMovie> GetMovies(int startPage = DEFAULT_START_PAGE, int pageSize = DEFAULT_PAGE_SIZE) {
    return GetMovies("", startPage, pageSize);
  }

  public IEnumerable<IMovie> GetMovies(string filter, int startPage = DEFAULT_START_PAGE, int pageSize = DEFAULT_PAGE_SIZE) {
    try {
      _LockCache.EnterReadLock();
      Log($"==> GetMovies({filter.WithQuotes()}, {startPage}, {pageSize})");
      if (string.IsNullOrWhiteSpace(filter)) {
        return _Items.Values.Skip(pageSize * (startPage - 1))
                     .Take(pageSize);
      } else {
        return _Items.Values.Where(m => m.FileName.Contains(filter, StringComparison.CurrentCultureIgnoreCase))
                     .Skip(pageSize * (startPage - 1))
                     .Take(pageSize);
      }
    } finally {
      _LockCache.ExitReadLock();
      Log($"<== GetMovies({filter.WithQuotes()}, {startPage}, {pageSize})");
    }
  }


  public IReadOnlyList<IMovie> GetMoviesInGroup(string groupName) {
    try {
      Log("==> GetMoviesInGroup");
      lock (_LockCache) {
        return _Items.Values.Where(m => m.Group.StartsWith(groupName, StringComparison.CurrentCultureIgnoreCase))
                     .ToList();
      }
    } finally {
      Log("<== GetMoviesInGroup");
    }
  }

  public IReadOnlyList<IMovie> GetMoviesNotInGroup(string groupName) {
    try {
      Log("==> GetMoviesNotInGroup");
      lock (_LockCache) {
        return _Items.Values.Where(m => m.Group.StartsWith(groupName))
                     .Where(m => !m.Group.Equals(groupName, StringComparison.CurrentCultureIgnoreCase))
                     .ToList();
      }
    } finally {
      Log("<== GetMoviesNotInGroup");
    }
  }

  public Task<IReadOnlyList<IMovie>> GetMoviesForGroupAndFilter(string groupName, string filter) {
    try {
      Log("==> GetMoviesForGroupAndFilter");
      lock (_LockCache) {
        IReadOnlyList<IMovie> RetVal = _Items.Values.Where(m => m.Group.StartsWith(groupName, StringComparison.CurrentCultureIgnoreCase))
                                             .Where(m => m.FileName.Contains(filter, StringComparison.CurrentCultureIgnoreCase))
                                             .ToList();
        return Task.FromResult(RetVal);
      }
    } finally {
      Log("<== GetMoviesForGroupAndFilter");
    }
  }

  //public Task<IReadOnlyList<IMovieGroup>> GetGroupsByGroupAndFilter(string groupName, string filter) {
  //  try {
  //    Log("==> GetGroupsByGroupAndFilter");
  //    int Level = groupName.Count(x => x == '/');

  //    IReadOnlyList<IMovieGroup> RetVal = GetMoviesNotInGroup(groupName)
  //                                          .Where(x => x.FileName.Contains(filter, StringComparison.CurrentCultureIgnoreCase))
  //                                          .GroupBy(x => _getGroupFilter(x.Group, Level))
  //                                          .Select(x => new TMovieGroup() { Name = x.Key, Count = x.Count() })
  //                                          .ToList();

  //    return Task.FromResult(RetVal);
  //  } finally {
  //    Log("<== GetGroupsByGroupAndFilter");
  //  }
  //}
  #endregion --- Movies access --------------------------------------------

  #region --- Groups access --------------------------------------------
  /// <summary>
  /// Get the distinct list of group names 
  /// </summary>
  /// <returns>A list of string</returns>
  public IEnumerable<string> GetGroupNames() {
    return _Items.Values.OrderBy(x => x.Group).Select(x => x.Group).Distinct();
  }

  public async Task<IMoviesPage> GetMoviesForGroupAndFilterInPages(string groupName, string filter, int startPage = DEFAULT_START_PAGE, int pageSize = DEFAULT_PAGE_SIZE) {

    try {
      Log("==> GetMoviesForGroupAndFilterInPages");
      Log($"Filter = {filter}");
      Log($"StartPage = {startPage}");

      if (IsEmpty()) {
        return new TMoviesPage() { Name = "", Page = 1, AvailablePages = 1 };
      }

      IReadOnlyList<IMovie> FilteredItems = await GetMoviesForGroupAndFilter(groupName, filter);
      int ItemCount = FilteredItems.Count();

      TMoviesPage RetVal = new TMoviesPage() {
        Name = groupName,
        Source = Name,
        Page = startPage,
        AvailablePages = ItemCount % pageSize == 0 ?
                         ItemCount / pageSize :
                         (int)(ItemCount / pageSize) + 1
      };

      foreach (TMovie MovieItem in FilteredItems.Skip((startPage.WithinLimits(0, int.MaxValue) - 1) * pageSize).Take(pageSize)) {
        RetVal.Movies.Add(MovieItem);
      }

      return RetVal;
    } finally {
      Log("<== GetMoviesForGroupAndFilterInPages");
    }

  }

  private string _getGroupFilter(string group, int level = 1) {
    if (string.IsNullOrWhiteSpace(group)) {
      return "";
    }
    string RetVal = string.Join("/", group.Split('/', StringSplitOptions.RemoveEmptyEntries).Take(level));
    return RetVal;
  }
  #endregion --- Groups access --------------------------------------------
}
