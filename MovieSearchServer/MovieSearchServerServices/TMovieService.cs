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

using MovieSearchModels;

namespace MovieSearchServerServices.MovieService {

  // <summary>
  /// Server Movie service. Provides access to groups, movies and pictures from NAS
  /// </summary>
  public class TMovieService : ALoggable, IMovieService {

    #region --- Constants --------------------------------------------
    private const string ROOT_NAME = "/";
    public const int TIMEOUT_IN_MS = 5000;
    #endregion --- Constants --------------------------------------------

    /// <summary>
    /// Root path to look for data
    /// </summary>
    public string Storage { get; init; }

    /// <summary>
    /// The name of the source
    /// </summary>
    public string StorageName { get; init; } = "Andromeda";

    /// <summary>
    /// The extensions of the files of interest
    /// </summary>
    public List<string> MoviesExtensions { get; } = new() { ".mkv", ".avi", ".mp4" };

    private IMovieCache _MoviesCache;

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TMovieService() { }
    public TMovieService(IMovieCache movieCache) {
      _MoviesCache = movieCache;
      _MoviesCache.SetLogger(Logger);
      Storage = movieCache.Storage;
    }


    private bool _IsInitialized = false;
    private bool _IsInitializing = false;
    public async Task Initialize() {
      if (_IsInitialized) {
        return;
      }

      if (_IsInitializing) {
        return;
      }

      if (_MoviesCache is null) {
        if (Directory.Exists(Storage)) {
          _MoviesCache = new TMovieCache() { Storage = this.Storage };
          _MoviesCache.SetLogger(Logger);
        } else {
          _MoviesCache = new XMovieCache();
          _MoviesCache.SetLogger(Logger);
        }
      }

      if (_MoviesCache.Any()) {
        return;
      }

      _IsInitializing = true;

      using (CancellationTokenSource Timeout = new CancellationTokenSource((int)TimeSpan.FromMinutes(5).TotalMilliseconds)) {
        IEnumerable<IFileInfo> Source = _MoviesCache.FetchFiles(Timeout.Token);
        await _MoviesCache.Parse(Source, Timeout.Token).ConfigureAwait(false);
      }

      _IsInitialized = true;
      _IsInitializing = false;
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public void Reset() {
      _MoviesCache.Clear();
    }

    #region --- Movies --------------------------------------------
    public async ValueTask<int> MoviesCount(string filter = "") {
      await Initialize().ConfigureAwait(false);

      if (string.IsNullOrWhiteSpace(filter)) {
        return _MoviesCache.Count();
      } else {
        return _MoviesCache.GetAllMovies().Count(m => m.Filename.Contains(filter, StringComparison.CurrentCultureIgnoreCase));
      }
    }

    public async ValueTask<int> PagesCount(int pageSize = IMovieService.DEFAULT_PAGE_SIZE) {
      int AllMoviesCount = await MoviesCount().ConfigureAwait(false);
      return (AllMoviesCount / pageSize) + (AllMoviesCount % pageSize > 0 ? 1 : 0);
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

      foreach (TMovie MovieItem in _MoviesCache.GetAllMovies().OrderBy(m => m.Filename).ThenBy(m => m.OutputYear)) {
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
                                               .Where(m => filter is null ? true : m.Filename.Contains(filter, StringComparison.CurrentCultureIgnoreCase))
                                               .Skip((startPage - 1) * pageSize)
                                               .Take(pageSize)) {
        yield return MovieItem;
      }
    }
    #endregion --- Movies --------------------------------------------


    public async Task<byte[]> GetPicture(string picturePath,
                                         string pictureName = IMovieService.DEFAULT_PICTURE_NAME,
                                         int width = IMovieService.DEFAULT_PICTURE_WIDTH,
                                         int height = IMovieService.DEFAULT_PICTURE_HEIGHT) {

      string FullPicturePath = Path.Combine(Storage, picturePath, pictureName);

      LogDebug($"GetPicture {FullPicturePath} : size({width}, {height})");

      try {
        if (!File.Exists(FullPicturePath)) {
          LogError($"Unable to fetch picture {FullPicturePath} : File is missing or access is denied");
          return null;
        }
        using (CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT_IN_MS)) {
          using (FileStream SourceStream = new FileStream(FullPicturePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true)) {
            using (MemoryStream PictureStream = new()) {
              await SourceStream.CopyToAsync(PictureStream, Timeout.Token);
              Image Picture = Image.FromStream(PictureStream);
              Bitmap ResizedPicture = new Bitmap(Picture, width, height);
              using (MemoryStream OutputStream = new()) {
                ResizedPicture.Save(OutputStream, ImageFormat.Jpeg);
                return OutputStream.ToArray();
              }
            }
          }
        }
      } catch (Exception ex) {
        LogError($"Unable to fetch picture {FullPicturePath} : {ex.Message}");
        return null;
      }
    }



    //public string CurrentGroup { get; private set; }
    //public async Task<IMovieGroups> GetGroups(string group, string filter) {

    //  #region === Validate parameters ===
    //  if (group == null) {
    //    group = "";
    //  }
    //  if (filter == null || filter == "undefined") {
    //    filter = "";
    //  }
    //  #endregion === Validate parameters ===

    //  int GroupLevel;

    //  if (group == ROOT_NAME || group == "") {
    //    CurrentGroup = ROOT_NAME;
    //    GroupLevel = 1;
    //  } else {
    //    CurrentGroup = $"/{group.Trim('/')}/";
    //    GroupLevel = CurrentGroup.Count(x => x == '/');
    //  }

    //  Log($"Getting groups for {CurrentGroup}, level={GroupLevel}");

    //  IMovieGroups RetVal = new TMovieGroups() { Name = CurrentGroup };

    //  if (_MoviesCache.IsEmpty()) {
    //    return RetVal;
    //  }

    //  foreach (TMovieGroup MovieGroupItem in await _MoviesCache.GetGroupsByGroupAndFilter(CurrentGroup, filter)) {
    //    RetVal.Groups.Add(MovieGroupItem);
    //  }

    //  return RetVal;
    //}

    //public async IAsyncEnumerable<TMovie> GetMovies(string group, string filter = "", int startPage = 0, int pageSize = 20) {
    //  await Initialize().ConfigureAwait(false);

    //  #region === Validate parameters ===
    //  if (group == ROOT_NAME) {
    //    CurrentGroup = ROOT_NAME;
    //  } else {
    //    CurrentGroup = $"/{group.Trim('/')}/";
    //  }
    //  if (filter == null) {
    //    filter = "";
    //  }
    //  #endregion === Validate parameters ===

    //  foreach (TMovie MovieItem in _MoviesCache.GetAllMovies()) {
    //    yield return MovieItem;
    //  }

    //}



  }
}
