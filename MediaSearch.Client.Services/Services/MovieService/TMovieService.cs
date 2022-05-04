using BLTools.Text;

using System.Diagnostics;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace MediaSearch.Client.Services;

/// <summary>
/// Client Movie service. Provides access to groups, movies and pictures from a REST server
/// </summary>

public class TMovieService : IMovieService, IMediaSearchLoggable<TMovieService> {

  public IApiServer ApiServer { get; set; } = new TApiServer();
  public IMediaSearchLogger<TMovieService> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMovieService>();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  private readonly TImageCache _ImagesCache = new TImageCache();

  public TMovieService() { }

  public TMovieService(IApiServer apiServer, TImageCache imagesCache) : this() {
    Logger.Log($"Api server = {apiServer}");
    _ImagesCache = imagesCache;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public async Task<bool> ProbeApi() {
    try {
      using (CancellationTokenSource Timeout = new CancellationTokenSource(GlobalSettings.HTTP_TIMEOUT_IN_MS)) {
        return await ApiServer.ProbeServerAsync(Timeout.Token);
      }
    } catch {
      return false;
    }
  }

  #region --- Refresh data --------------------------------------------
  public async Task StartRefresh() {
    try {
      using (CancellationTokenSource Timeout = new CancellationTokenSource(GlobalSettings.HTTP_TIMEOUT_IN_MS)) {
        string? Response = await ApiServer.GetStringAsync("system/startrefreshdata", Timeout.Token);
      }
    } catch { }
  }

  public async Task<int> GetRefreshStatus() {
    try {
      using (CancellationTokenSource Timeout = new CancellationTokenSource(GlobalSettings.HTTP_TIMEOUT_IN_MS)) {
        string? Response = await ApiServer.GetStringAsync("system/getrefreshstatus", Timeout.Token);
        if (Response is not null) {
          int RetVal = int.Parse(Response);
          return RetVal;
        }
        return 0;
      }
    } catch {
      return 0;
    }
  }
  #endregion --- Refresh data --------------------------------------------

  #region --- Movie actions --------------------------------------------
  public async Task<IMoviesPage?> GetMoviesPage(IFilter filter) {
    try {

      string RequestUrl = $"movie";
      Logger.LogDebugEx(RequestUrl.BoxFixedWidth($"get movie page request", GlobalSettings.DEBUG_BOX_WIDTH));
      Logger.LogDebug(filter.ToString().BoxFixedWidth($"Movies page filter", GlobalSettings.DEBUG_BOX_WIDTH));

      using (CancellationTokenSource Timeout = new CancellationTokenSource(GlobalSettings.HTTP_TIMEOUT_IN_MS)) {

        TMoviesPage? Result = await ApiServer.GetJsonAsync<IFilter, TMoviesPage>(RequestUrl, new TFilter(filter), CancellationToken.None).ConfigureAwait(false);
        if (Result is null) {
          return null;
        }
        Logger.LogDebugExBox("TMoviePage", Result);
        return Result;

      }
    } catch (Exception ex) {
      Logger.LogError($"Unable to get movies data : {ex.Message}");
      if (ex.InnerException is not null) {
        Logger.LogError($"  Inner exception : {ex.InnerException.Message}");
        Logger.LogError($"  Inner call stack : {ex.InnerException.StackTrace}");
      }
      return null;
    }
  }
  #endregion --- Movie actions --------------------------------------------

  #region --- Picture actions --------------------------------------------
  public async Task<byte[]> GetPicture(string id, CancellationToken cancelToken, int w = 128, int h = 160) {

    string RequestUrl = $"movie/getPicture?movieId={id.ToUrl64()}&width={w}&height={h}";
    IfDebugMessageEx("Requesting picture", RequestUrl);

    try {
      using (CancellationTokenSource Timeout = new CancellationTokenSource(GlobalSettings.HTTP_TIMEOUT_IN_MS)) {
        byte[]? RetVal = await ApiServer.GetByteArrayAsync(RequestUrl, Timeout.Token).ConfigureAwait(false);
        if (RetVal is null) {
          return Array.Empty<byte>();
        }
        return RetVal;
      }
    } catch (TaskCanceledException) {
      Logger.LogError("Task is cancelled.");
      return Array.Empty<byte>();
    } catch (OperationCanceledException) {
      Logger.LogError("Operation is cancelled.");
      return Array.Empty<byte>();
    } catch (Exception ex) {
      Logger.LogError($"Unable to get picture : {ex.Message}");
      return Array.Empty<byte>();
    } finally {
      //LogDebug("Completed getpicture");
    }
  }

  public async Task<string> GetPicture64(IMovie movie, CancellationToken cancelToken) {

    byte[] PictureBytes = _ImagesCache.GetImage(movie.ID);

    if (PictureBytes is null || PictureBytes.IsEmpty()) {
      PictureBytes = await GetPicture(movie.ID, cancelToken).ConfigureAwait(false);
      if (PictureBytes is null) {
        PictureBytes = TMovie.PictureMissing;
      } else {
        _ImagesCache.AddImage(movie.ID, PictureBytes);
      }
    }

    return $"data:image/jpg;base64, {Convert.ToBase64String(PictureBytes)}";
  }
  #endregion --- Picture actions --------------------------------------------

  #region --- Groups --------------------------------------------
  public async Task<IList<string>> GetGroups(CancellationToken cancelToken) {
    try {
      string RequestUrl = $"movie/getGroups";
      using (CancellationTokenSource Timeout = new CancellationTokenSource(GlobalSettings.HTTP_TIMEOUT_IN_MS)) {

        string? GroupsJson = await ApiServer.GetStringAsync(RequestUrl, Timeout.Token).ConfigureAwait(false);
        if (GroupsJson is null) {
          return new List<string>();
        }
        IList<string>? RetVal = JsonSerializer.Deserialize<IList<string>>(GroupsJson);
        if (RetVal is null) {
          return new List<string>();
        }

        return RetVal;
      }

    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to get groups data", ex);
      if (ex.InnerException is not null) {
        Logger.LogErrorBox($"  Inner exception", ex.InnerException);
      }
      return new List<string>();
    }
  }
  public async Task<IList<string>> GetSubGroups(string group, CancellationToken cancelToken) {
    try {
      string RequestUrl = $"movie/getSubGroups?group={group.ToUrl()}";
      using (CancellationTokenSource Timeout = new CancellationTokenSource(GlobalSettings.HTTP_TIMEOUT_IN_MS)) {
        string? GroupsJson = await ApiServer.GetStringAsync(RequestUrl, Timeout.Token).ConfigureAwait(false);
        if (GroupsJson is null) {
          return new List<string>();
        }
        IList<string>? RetVal = JsonSerializer.Deserialize<IList<string>>(GroupsJson);
        if (RetVal is null) {
          return new List<string>();
        }
        return RetVal;
      }

    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to get subgroups data", ex);
      if (ex.InnerException is not null) {
        Logger.LogErrorBox($"  Inner exception", ex.InnerException);
      }
      return new List<string>();
    }
  } 
  #endregion --- Groups --------------------------------------------

  [Conditional("DEBUG")]
  private void IfDebugMessage(string title, object? message, [CallerMemberName] string CallerName = "") {
    Logger.LogDebugBox(title, message?.ToString() ?? "", CallerName);
  }

  [Conditional("DEBUG")]
  private void IfDebugMessageEx(string title, object? message, [CallerMemberName] string CallerName = "") {
    Logger.LogDebugExBox(title, message?.ToString() ?? "", CallerName);
  }

}
