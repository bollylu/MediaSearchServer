using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BLTools;
using BLTools.Diagnostic.Logging;

using MovieSearchModels;

namespace MovieSearchServerServices.MovieService {
  public abstract class AMovieCache : ALoggable, IMovieCache {

    public const int DEFAULT_START_PAGE = 1;
    public const int DEFAULT_PAGE_SIZE = 20;

    public const char FOLDER_SEPARATOR = '/';

    #region --- Internal data storage --------------------------------------------
    /// <summary>
    /// Store the movies
    /// </summary>
    protected readonly SortedList<string, IMovie> _Items = new();

    protected readonly ReaderWriterLockSlim _LockCache = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
    #endregion --- Internal data storage --------------------------------------------

    public string Storage { get; init; }

    public string StorageName { get; set; }

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
          _Items.Add($"{NewMovie.Filename}{NewMovie.OutputYear}", NewMovie);
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
      string ProcessedFileItem = item.FullName.Replace('\\', FOLDER_SEPARATOR);

      RetVal.StorageRoot = Storage.Replace('\\', FOLDER_SEPARATOR);
      RetVal.StoragePath = ProcessedFileItem.BeforeLast(FOLDER_SEPARATOR).After(RetVal.StorageRoot, System.StringComparison.InvariantCultureIgnoreCase);

      RetVal.Filename = item.Name;
      RetVal.Name = ProcessedFileItem.AfterLast(FOLDER_SEPARATOR).BeforeLast(" (");
      RetVal.FileExtension = RetVal.Filename.AfterLast('.').ToLowerInvariant();

      string[] Tags = RetVal.StoragePath.BeforeLast(FOLDER_SEPARATOR).Split(FOLDER_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
      RetVal.Group = Tags.Any() ? Tags[0] : "(Unknown)";

      foreach (string TagItem in Tags) {
        RetVal.Tags.Add(TagItem);
      }

      try {
        RetVal.OutputYear = int.Parse(RetVal.Filename.AfterLast('(').BeforeLast(')'));
      } catch {
        LogWarning($"Unable to find output year : Invalid or missing number : {item.FullName}");
        RetVal.OutputYear = 0;
      }

      RetVal.Size = item.Length;

      return RetVal;
    }

    #endregion --- Cache I/O --------------------------------------------

    #region --- Cache management --------------------------------------------
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
    public IEnumerable<IMovie> GetMoviesWithGroup() {
      lock (_LockCache) {
        return _Items.Values.Where(m => !string.IsNullOrWhiteSpace(m.Group));
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
        Log($"==> GetMovies({filter.WithQuotes()}, {startPage}, {pageSize})");
        lock (_LockCache) {
          if (string.IsNullOrWhiteSpace(filter)) {
            return _Items.Values.Skip(pageSize * (startPage - 1))
                         .Take(pageSize);
          } else {
            return _Items.Values.Where(m => m.Filename.Contains(filter, StringComparison.CurrentCultureIgnoreCase))
                         .Skip(pageSize * (startPage - 1))
                         .Take(pageSize);
          }
        }
      } finally {
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
                                               .Where(m => m.Filename.Contains(filter, StringComparison.CurrentCultureIgnoreCase))
                                               .ToList();
          return Task.FromResult(RetVal);
        }
      } finally {
        Log("<== GetMoviesForGroupAndFilter");
      }
    }

    public Task<IReadOnlyList<IMovieGroup>> GetGroupsByGroupAndFilter(string groupName, string filter) {
      try {
        Log("==> GetGroupsByGroupAndFilter");
        int Level = groupName.Count(x => x == '/');

        IReadOnlyList<IMovieGroup> RetVal = GetMoviesNotInGroup(groupName)
                                              .Where(x => x.Filename.Contains(filter, StringComparison.CurrentCultureIgnoreCase))
                                              .GroupBy(x => _getGroupFilter(x.Group, Level))
                                              .Select(x => new TMovieGroup() { Name = x.Key, Count = x.Count() })
                                              .ToList();

        return Task.FromResult(RetVal);
      } finally {
        Log("<== GetGroupsByGroupAndFilter");
      }
    }
    #endregion --- Movies access --------------------------------------------



    #region --- Groups access --------------------------------------------
    /// <summary>
    /// Get the distinct list of group names 
    /// </summary>
    /// <returns>A list of string</returns>
    public IEnumerable<string> GetGroupNames() {
      return _Items.Values.OrderBy(x => x.Group).Select(x => x.Group).Distinct();
    }

    public async Task<IMovies> GetMoviesForGroupAndFilterInPages(string groupName, string filter, int startPage = DEFAULT_START_PAGE, int pageSize = DEFAULT_PAGE_SIZE) {

      try {
        Log("==> GetMoviesForGroupAndFilterInPages");
        Log($"Filter = {filter}");
        Log($"StartPage = {startPage}");

        if (IsEmpty()) {
          return new TMovies() { Name = "", Page = 1, AvailablePages = 1 };
        }

        IReadOnlyList<IMovie> FilteredItems = await GetMoviesForGroupAndFilter(groupName, filter);
        int ItemCount = FilteredItems.Count();

        TMovies RetVal = new TMovies() {
          Name = groupName,
          Source = StorageName,
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
}
