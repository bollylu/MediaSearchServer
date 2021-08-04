using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BLTools;
using BLTools.Diagnostic.Logging;

using MovieSearch.Models;

namespace MovieSearchServerServices.MovieService {
  public abstract class AMovieCache : ALoggable, IMovieCache {

    public const int DEFAULT_START_PAGE = 1;
    public const int DEFAULT_PAGE_SIZE = 20;

    /// <summary>
    /// Store the movies
    /// </summary>
    protected readonly List<IMovie> _Items = new();

    protected readonly object _LockCache = new();

    public string SourceName { get; set; }

    public abstract Task Load();

    public void Clear() {
      lock (_LockCache) {
        _Items.Clear();
      }
    }

    public bool IsEmpty() {
      lock (_LockCache) {
        return _Items.IsEmpty();
      }
    }

    public int Count() {
      return _Items.Count;
    }

    /// <summary>
    /// Get the list of movies with a valid group name
    /// </summary>
    public IEnumerable<IMovie> HaveGroup {
      get {
        lock (_LockCache) {
          return _Items.Where(m => !string.IsNullOrWhiteSpace(m.Group));
        }
      }
    }

    public IEnumerable<IMovie> GetAllMovies() {
      try {
        Log($"==> GetAllMovies()");
        lock (_LockCache) {
          return _Items.AsEnumerable();
        }
      } finally {
        Log($"<== GetAllMovies()");
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
            return _Items.Skip(pageSize * (startPage - 1))
                         .Take(pageSize);
          } else {
            return _Items.Where(m => m.LocalName.Contains(filter, StringComparison.CurrentCultureIgnoreCase))
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
          return _Items.Where(m => m.Group.StartsWith(groupName, StringComparison.CurrentCultureIgnoreCase))
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
          return _Items.Where(m => m.Group.StartsWith(groupName))
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
          IReadOnlyList<IMovie> RetVal = _Items.Where(m => m.Group.StartsWith(groupName, StringComparison.CurrentCultureIgnoreCase))
                                               .Where(m => m.LocalName.Contains(filter, StringComparison.CurrentCultureIgnoreCase))
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
                                              .Where(x => x.LocalName.Contains(filter, StringComparison.CurrentCultureIgnoreCase))
                                              .GroupBy(x => _getGroupFilter(x.Group, Level))
                                              .Select(x => new TMovieGroup() { Name = x.Key, Count = x.Count() })
                                              .ToList();

        return Task.FromResult(RetVal);
      } finally {
        Log("<== GetGroupsByGroupAndFilter");
      }
    }

    /// <summary>
    /// Get the distinct list of group names 
    /// </summary>
    /// <returns>A list of string</returns>
    public IEnumerable<string> GetGroupNames() {
      return _Items.OrderBy(x => x.Group).Select(x => x.Group).Distinct();
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
          Source = SourceName,
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

  }
}
