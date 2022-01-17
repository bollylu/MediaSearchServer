using BLTools.Text;

using System.Net.Http.Json;

namespace MediaSearch.Client.Services;

/// <summary>
/// Client Movie service. Provides access to groups, movies and pictures from a REST server
/// </summary>

public class TMovieService : ALoggable, IMovieService {

  #region --- Constants --------------------------------------------
  private int HTTP_TIMEOUT_IN_MS = 10000;
  private const int LOG_BOX_WIDTH = 160;
  #endregion --- Constants --------------------------------------------

  public string ApiBase { get; set; }

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  private readonly THttpClientEx _Client;
  private readonly TImageCache _ImagesCache;

  public TMovieService(string apiServer, TImageCache imagesCache, ILogger logger) {
    SetLogger(logger);
    Logger.SeverityLimit = ESeverity.Debug;
    Log($"Api server = {apiServer}");
    ApiBase = apiServer;
    _Client = new THttpClientEx() { BaseAddress = new Uri(ApiBase) };
    _ImagesCache = imagesCache;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public async Task<bool> ProbeApi() {
    try {
      using (CancellationTokenSource Timeout = new CancellationTokenSource(HTTP_TIMEOUT_IN_MS)) {
        HttpResponseMessage Response = await _Client.GetAsync("movie", Timeout.Token);
        return Response.IsSuccessStatusCode;
      }
    } catch {
      return false;
    }
  }

  #region --- Refresh data --------------------------------------------
  public async Task StartRefresh() {
    try {
      using (CancellationTokenSource Timeout = new CancellationTokenSource(HTTP_TIMEOUT_IN_MS)) {
        HttpResponseMessage Response = await _Client.GetAsync("system/startrefreshdata", Timeout.Token);
      }
    } catch { }
  }

  public async Task<int> GetRefreshStatus() {
    try {
      using (CancellationTokenSource Timeout = new CancellationTokenSource(HTTP_TIMEOUT_IN_MS)) {
        HttpResponseMessage Response = await _Client.GetAsync("system/getrefreshstatus", Timeout.Token);
        if (Response.IsSuccessStatusCode) {
          int RetVal = int.Parse(await Response.Content.ReadAsStringAsync());
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
  //public async Task<IMoviesPage> GetMoviesPage(TFilter filter, int startPage = 1, int pageSize = 20) {
  //  try {

  //    string RequestUrl = $"movie/getFiltered?filter={filter.ToJson()}&page={startPage}&size={pageSize}";
  //    LogDebug(RequestUrl.BoxFixedWidth($"get movie page request", LOG_BOX_WIDTH));

  //    using (CancellationTokenSource Timeout = new CancellationTokenSource(HTTP_TIMEOUT_IN_MS)) {

  //      string JsonMovies = await _Client.GetStringAsync(RequestUrl, Timeout.Token);
  //      LogDebugEx(JsonMovies.BoxFixedWidth($"get movie page raw result", LOG_BOX_WIDTH));
  //      IMoviesPage Result = TMoviesPage.FromJson(JsonMovies);

  //      LogDebugEx(Result.ToString().BoxFixedWidth($"IMoviesPage", LOG_BOX_WIDTH));
  //      return Result;

  //    }
  //  } catch (Exception ex) {
  //    LogError($"Unable to get movies data : {ex.Message}");
  //    if (ex.InnerException is not null) {
  //      LogError($"  Inner exception : {ex.InnerException.Message}");
  //      LogError($"  Inner call stack : {ex.InnerException.StackTrace}");
  //    }
  //    return null;
  //  }
  //}

  public async Task<IMoviesPage> GetMoviesPage(TFilter filter, int startPage = 1, int pageSize = 20) {
    try {

      string RequestUrl = $"movie/getWithFilter?page={startPage}&size={pageSize}";
      var Content = JsonContent.Create(filter);
      LogDebug(RequestUrl.BoxFixedWidth($"get movie page request", LOG_BOX_WIDTH));

      using (CancellationTokenSource Timeout = new CancellationTokenSource(HTTP_TIMEOUT_IN_MS)) {

        HttpResponseMessage Response = await _Client.PostAsync(RequestUrl, Content, Timeout.Token);
        string JsonMovies = await Response.Content.ReadAsStringAsync();
        LogDebugEx(JsonMovies.BoxFixedWidth($"get movie page raw result", LOG_BOX_WIDTH));
        IMoviesPage Result = TMoviesPage.FromJson(JsonMovies);

        LogDebugEx(Result.ToString().BoxFixedWidth($"IMoviesPage", LOG_BOX_WIDTH));
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
      using (CancellationTokenSource Timeout = new CancellationTokenSource(HTTP_TIMEOUT_IN_MS)) {
        return await _Client.GetByteArrayAsync(RequestUrl, Timeout.Token).WithCancellation(cancelToken).ConfigureAwait(false);
      }
    } catch (TaskCanceledException) {
      //LogError("Task is cancelled.");
      return null;
    } catch (OperationCanceledException) {
      //LogError("Operation is cancelled.");
      return null;
    } catch (Exception ex) {
      LogError($"Unable to get picture : {ex.Message}");
      return null;
    } finally {
      //LogDebug("Completed getpicture");
    }
  }

  public async Task<string> GetPicture64(IMovie movie, CancellationToken cancelToken) {

    byte[] PictureBytes = _ImagesCache.GetImage(movie.Id);

    if (PictureBytes is null) {
      PictureBytes = await GetPicture(movie.Id, cancelToken).ConfigureAwait(false);
      if (PictureBytes is null) {
        PictureBytes = AMovie.PictureMissing;
      } else {
        _ImagesCache.AddImage(movie.Id, PictureBytes);
      }
    }

    return $"data:image/jpg;base64, {Convert.ToBase64String(PictureBytes)}";
  }
  #endregion --- Picture actions --------------------------------------------
}
