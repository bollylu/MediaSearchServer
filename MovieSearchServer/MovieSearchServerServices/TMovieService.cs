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
    private static string DEFAULT_ROOT_PATH;
    #endregion --- Constants --------------------------------------------

    /// <summary>
    /// Root path to look for data
    /// </summary>
    public string RootPath { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public string Source => "Andromeda";

    /// <summary>
    /// The files that match one extension in this list will be ignored
    /// </summary>
    public List<string> ExcludedExtensions { get; } = new List<string>() { ".nfo", ".jpg", ".vsmeta" };

    private IMovieCache _MoviesCache;

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TMovieService() {
      _Initialize();
    }

    public TMovieService(string rootPath) {
      RootPath = rootPath;
      _Initialize();
    }

    private bool _IsInitialized = false;
    private void _Initialize() {
      if (_IsInitialized) {
        return;
      }

      if (OperatingSystem.IsWindows()) {
        DEFAULT_ROOT_PATH = @"\\andromeda.sharenet.priv\films\";
      } else {
        DEFAULT_ROOT_PATH = @"/volume1/Films/";
      }

      if (string.IsNullOrEmpty(RootPath)) {
        RootPath = DEFAULT_ROOT_PATH;
      }

      if (Directory.Exists(RootPath)) {
        _MoviesCache = new TMovieCache() { Logger = ALogger.Create(Logger), RootPath = this.RootPath };
      } else {
        _MoviesCache = new XMovieCache() { Logger = ALogger.Create(Logger) };
      }
      Task.Run(() => _MoviesCache.Load());

      _IsInitialized = true;

    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

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

    public async Task<IMovies> GetMovies(string group, string filter = "", int startPage = 0, int pageSize = 20) {

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

      #region --- All movies --------------------------------------------

      return await _MoviesCache.GetMoviesForGroupAndFilterInPages(CurrentGroup, filter, startPage, pageSize);

      #endregion --- All movies --------------------------------------------
    }


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


  }
}
