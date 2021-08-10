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
    public string RootPath { get; set; }

    /// <summary>
    /// The name of the source
    /// </summary>
    public string Source => "Andromeda";

    /// <summary>
    /// The extensions of the files of interest
    /// </summary>
    public List<string> MoviesExtensions { get; } = new() { ".mkv", ".avi", ".mp4" };


    private IMovieCache _MoviesCache;

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TMovieService() { }

    public TMovieService(string rootPath) {
      RootPath = rootPath;
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

      _IsInitializing = true;

      if (string.IsNullOrEmpty(RootPath)) {
        if (OperatingSystem.IsWindows()) {
          RootPath = @"\\andromeda.sharenet.priv\films\";
        } else {
          RootPath = @"/volume1/Films/";
        }
      }

      if (Directory.Exists(RootPath)) {
        _MoviesCache = new TMovieCache() { Logger = ALogger.Create(Logger), RootPath = this.RootPath };
      } else {
        _MoviesCache = new XMovieCache() { Logger = ALogger.Create(Logger) };
      }
      await _MoviesCache.Load();

      _IsInitialized = true;
      _IsInitializing = false;
    }

    public string CurrentGroup { get; private set; }
    public int MoviesCount(string filter = "") {
      if (string.IsNullOrWhiteSpace(filter)) {
        return _MoviesCache.Count();
      } else {
        return _MoviesCache.GetAllMovies().Count(m => m.LocalName.Contains(filter, StringComparison.CurrentCultureIgnoreCase));
      }
    }

    public int PagesCount(int pageSize = IMovieService.DEFAULT_PAGE_SIZE) {
      return (MoviesCount() / pageSize) + (MoviesCount() % pageSize > 0 ? 1 : 0);
    }

    public int PagesCount(string filter, int pageSize = IMovieService.DEFAULT_PAGE_SIZE) {
      if (string.IsNullOrWhiteSpace(filter)) {
        return PagesCount(pageSize);
      }

      int FilteredMoviesCount = MoviesCount(filter);
      return (FilteredMoviesCount / pageSize) + (FilteredMoviesCount % pageSize > 0 ? 1 : 0);
    }

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








    public async IAsyncEnumerable<IMovie> GetAllMovies() {
      await Initialize().ConfigureAwait(false);

      foreach (IMovie MovieItem in _MoviesCache.GetAllMovies()) {
        yield return MovieItem;
      }
    }

    public async IAsyncEnumerable<IMovie> GetMovies(int startPage = 1, int pageSize = 20) {
      await Initialize().ConfigureAwait(false);

      foreach (IMovie MovieItem in _MoviesCache.GetAllMovies().Skip((startPage - 1) * pageSize).Take(pageSize)) {
        yield return MovieItem;
      }
    }

    public async IAsyncEnumerable<IMovie> GetMovies(string filter, int startPage = 1, int pageSize = 20) {
      await Initialize().ConfigureAwait(false);

      foreach (IMovie MovieItem in _MoviesCache.GetAllMovies()
                                               .Where(m => m.LocalName.Contains(filter, StringComparison.CurrentCultureIgnoreCase))
                                               .Skip((startPage - 1) * pageSize)
                                               .Take(pageSize)) {
        yield return MovieItem;
      }
    }

    public async IAsyncEnumerable<IMovie> GetMovies(string group, string filter = "", int startPage = 0, int pageSize = 20) {
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

      foreach (IMovie MovieItem in _MoviesCache.GetAllMovies()) {
        yield return MovieItem;
      }

    }

    #region --- Pictures --------------------------------------------
    private static byte[] _MissingPicture = File.ReadAllBytes($"Pictures{Path.DirectorySeparatorChar}missing.jpg");

    public async Task<byte[]> GetPicture(string pathname, int timeout = 5000) {
      if (string.IsNullOrWhiteSpace(pathname)) {
        return _MissingPicture;
      }

      string FullFileName = Path.Combine(RootPath, pathname, "folder.jpg");

      if (!File.Exists(FullFileName)) {
        return _MissingPicture;
      }

      using (CancellationTokenSource TimeOut = new CancellationTokenSource(timeout)) {
        try {
          using (Image FolderJpg = Image.FromStream((await File.ReadAllBytesAsync(FullFileName, TimeOut.Token)).ToStream())) {
            using (Bitmap ResizedPicture = new Bitmap(FolderJpg, 128, 160)) {
              using (MemoryStream OutputStream = new()) {
                ResizedPicture.Save(OutputStream, ImageFormat.Jpeg);
                return OutputStream.ToArray();
              }
            }
          }
        } catch (Exception ex) {
          LogError($"Unable to get picture {pathname} : {ex.Message}");
          return _MissingPicture;
        }
      }
    }

    public async Task<string> GetPicture64(string pathname, int timeout = 5000) {

      byte[] PictureBytes = await GetPicture(pathname, timeout);
      return $"data:image/jpg;base64, {Convert.ToBase64String(PictureBytes)}";
    }
    #endregion --- Pictures --------------------------------------------

  }
}
