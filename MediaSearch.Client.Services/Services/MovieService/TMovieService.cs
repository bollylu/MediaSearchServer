using BLTools.Text;

using System.Net.Http.Json;

namespace MediaSearch.Client.Services;

/// <summary>
/// Client Movie service. Provides access to groups, movies and pictures from a REST server
/// </summary>

public class TMovieService : ALoggable, IMovieService {

  public IApiServer ApiServer { get; set; } = new TApiServer();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  private readonly TImageCache _ImagesCache = new TImageCache();

  public TMovieService() {
    SetLogger(GlobalSettings.GlobalLogger);
  }

  public TMovieService(IApiServer apiServer, TImageCache imagesCache, ILogger logger) : this() {
    SetLogger(logger);
    Log($"Api server = {apiServer}");
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
      LogDebugEx(RequestUrl.BoxFixedWidth($"get movie page request", GlobalSettings.DEBUG_BOX_WIDTH));
      LogDebug(filter.ToString().BoxFixedWidth($"Movies page filter", GlobalSettings.DEBUG_BOX_WIDTH));

      using (CancellationTokenSource Timeout = new CancellationTokenSource(GlobalSettings.HTTP_TIMEOUT_IN_MS)) {

        TMoviesPage? Result = await ApiServer.GetJsonAsync<TMoviesPage>(RequestUrl, filter, CancellationToken.None).ConfigureAwait(false);

        LogDebugEx(Result?.ToString().BoxFixedWidth($"IMoviesPage", GlobalSettings.DEBUG_BOX_WIDTH));
        return Result;

      }
    } catch (Exception ex) {
      LogError($"Unable to get movies data : {ex.Message}");
      if (ex.InnerException is not null) {
        LogError($"  Inner exception : {ex.InnerException.Message}");
        LogError($"  Inner call stack : {ex.InnerException.StackTrace}");
      }
      return null;
    }
  }
  #endregion --- Movie actions --------------------------------------------

  #region --- Picture actions --------------------------------------------
  public async Task<byte[]> GetPicture(string id, CancellationToken cancelToken, int w = 128, int h = 160) {

    string RequestUrl = $"movie/getPicture?id={id.ToUrl64()}&width={w}&height={h}";
    LogDebugEx($"Requesting picture : {RequestUrl}");

    try {
      LogDebugEx($"starting getpicture {id}");
      using (CancellationTokenSource Timeout = new CancellationTokenSource(GlobalSettings.HTTP_TIMEOUT_IN_MS)) {
        byte[]? RetVal = await ApiServer.GetByteArrayAsync(RequestUrl, Timeout.Token).ConfigureAwait(false);
        if (RetVal is null) {
          return Array.Empty<byte>();
        }
        return RetVal;
      }
    } catch (TaskCanceledException) {
      //LogError("Task is cancelled.");
      return Array.Empty<byte>();
    } catch (OperationCanceledException) {
      //LogError("Operation is cancelled.");
      return Array.Empty<byte>();
    } catch (Exception ex) {
      LogError($"Unable to get picture : {ex.Message}");
      return Array.Empty<byte>();
    } finally {
      //LogDebug("Completed getpicture");
    }
  }

  public async Task<string> GetPicture64(IMovie movie, CancellationToken cancelToken) {

    byte[] PictureBytes = _ImagesCache.GetImage(movie.Id);

    if (PictureBytes is null) {
      PictureBytes = await GetPicture(movie.Id, cancelToken).ConfigureAwait(false);
      if (PictureBytes is null) {
        PictureBytes = TMovie.PictureMissing;
      } else {
        _ImagesCache.AddImage(movie.Id, PictureBytes);
      }
    }

    return $"data:image/jpg;base64, {Convert.ToBase64String(PictureBytes)}";
  }
  #endregion --- Picture actions --------------------------------------------

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
      LogError($"Unable to get movies data : {ex.Message}");
      if (ex.InnerException is not null) {
        LogError($"  Inner exception : {ex.InnerException.Message}");
        LogError($"  Inner call stack : {ex.InnerException.StackTrace}");
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
      LogError($"Unable to get movies data : {ex.Message}");
      if (ex.InnerException is not null) {
        LogError($"  Inner exception : {ex.InnerException.Message}");
        LogError($"  Inner call stack : {ex.InnerException.StackTrace}");
      }
      return new List<string>();
    }
  }
}
