using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BLTools;
using BLTools.Diagnostic.Logging;

using MovieSearch.Models;

namespace MovieSearchServerServices.MovieService {

  // <summary>
  /// Server Movie service. Provides access to groups, movies and pictures from NAS
  /// </summary>
  public class TMovieService : ALoggable, IMovieService {

    #region --- Constants --------------------------------------------
    private const string ROOT_NAME = "/";

    #endregion --- Constants --------------------------------------------

    /// <summary>
    /// Root path to look for data
    /// </summary>
    public string Storage { get; init; }

    /// <summary>
    /// The name of the source
    /// </summary>
    public string StorageName { get; set; } = "Andromeda";

    /// <summary>
    /// The extensions of the files of interest
    /// </summary>
    public List<string> MoviesExtensions { get; } = new() { ".mkv", ".avi", ".mp4" };

    private IMovieCache _MoviesCache;

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TMovieService() { }
    public TMovieService(IMovieCache movieCache) {
      _MoviesCache = movieCache;
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    private bool _IsInitialized = false;
    private bool _IsInitializing = false;
    public async Task Initialize() {
      if (_IsInitialized) {
        return;
      }

      if (_IsInitializing) {
        return;
      }

      if (_MoviesCache is not null && _MoviesCache.Any()) {
        return;
      }

      _IsInitializing = true;

      if (string.IsNullOrEmpty(Storage)) {
        throw new ApplicationException("Root path is missing. Cannot process movies.");
      }

      if (Directory.Exists(Storage)) {
        _MoviesCache = new TMovieCache() { Logger = ALogger.Create(Logger), Storage = this.Storage };
      } else {
        _MoviesCache = new XMovieCache() { Logger = ALogger.Create(Logger) };
      }
      await _MoviesCache.Load();

      _IsInitialized = true;
      _IsInitializing = false;
    }

    public void Reset() {
      _MoviesCache.Clear();
    }

    public async ValueTask<int> MoviesCount(string filter = "") {
      await Initialize().ConfigureAwait(false);

      if (string.IsNullOrWhiteSpace(filter)) {
        return _MoviesCache.Count();
      } else {
        return _MoviesCache.GetAllMovies().Count(m => m.LocalName.Contains(filter, StringComparison.CurrentCultureIgnoreCase));
      }
    }

    public async ValueTask<int> PagesCount(int pageSize = IMovieService.DEFAULT_PAGE_SIZE) {
      int AllMoviesCount = await MoviesCount().ConfigureAwait(false);
      return ( AllMoviesCount / pageSize) + (AllMoviesCount % pageSize > 0 ? 1 : 0);
    }

    public async ValueTask<int> PagesCount(string filter, int pageSize = IMovieService.DEFAULT_PAGE_SIZE) {
      if (string.IsNullOrWhiteSpace(filter)) {
        return await PagesCount(pageSize).ConfigureAwait(false);
      }

      int FilteredMoviesCount = await MoviesCount(filter).ConfigureAwait(false);
      return (FilteredMoviesCount / pageSize) + (FilteredMoviesCount % pageSize > 0 ? 1 : 0);
    }

    public async IAsyncEnumerable<TMovie> GetAllMovies() {
      await Initialize().ConfigureAwait(false);

      foreach (TMovie MovieItem in _MoviesCache.GetAllMovies()) {
        yield return MovieItem;
      }
    }

    public async IAsyncEnumerable<TMovie> GetMovies(int startPage = 1, int pageSize = 20) {
      await Initialize().ConfigureAwait(false);

      foreach (TMovie MovieItem in _MoviesCache.GetAllMovies().Skip((startPage - 1) * pageSize).Take(pageSize)) {
        yield return MovieItem;
      }
    }

    public async IAsyncEnumerable<TMovie> GetMovies(string filter = "", int startPage = 1, int pageSize = 20) {
      await Initialize().ConfigureAwait(false);

      foreach (TMovie MovieItem in _MoviesCache.GetAllMovies()
                                               .Where(m => filter is null ? true : m.LocalName.Contains(filter, StringComparison.CurrentCultureIgnoreCase))
                                               .Skip((startPage - 1) * pageSize)
                                               .Take(pageSize)) {
        yield return MovieItem;
      }
    }






    public string CurrentGroup { get; private set; }
    public async Task<IMovieGroups> GetGroups(string group, string filter) {

      #region === Validate parameters ===
      if (group == null) {
        group = "";
      }
      if (filter == null || filter == "undefined") {
        filter = "";
      }
      #endregion === Validate parameters ===

      int GroupLevel;

      if (group == ROOT_NAME || group == "") {
        CurrentGroup = ROOT_NAME;
        GroupLevel = 1;
      } else {
        CurrentGroup = $"/{group.Trim('/')}/";
        GroupLevel = CurrentGroup.Count(x => x == '/');
      }

      Log($"Getting groups for {CurrentGroup}, level={GroupLevel}");

      IMovieGroups RetVal = new TMovieGroups() { Name = CurrentGroup };

      if (_MoviesCache.IsEmpty()) {
        return RetVal;
      }

      foreach (TMovieGroup MovieGroupItem in await _MoviesCache.GetGroupsByGroupAndFilter(CurrentGroup, filter)) {
        RetVal.Groups.Add(MovieGroupItem);
      }

      return RetVal;
    }


    public async IAsyncEnumerable<TMovie> GetMovies(string group, string filter = "", int startPage = 0, int pageSize = 20) {
      await Initialize().ConfigureAwait(false);

      #region === Validate parameters ===
      if (group == ROOT_NAME) {
        CurrentGroup = ROOT_NAME;
      } else {
        CurrentGroup = $"/{group.Trim('/')}/";
      }
      if (filter == null) {
        filter = "";
      }
      #endregion === Validate parameters ===

      foreach (TMovie MovieItem in _MoviesCache.GetAllMovies()) {
        yield return MovieItem;
      }

    }



  }
}
