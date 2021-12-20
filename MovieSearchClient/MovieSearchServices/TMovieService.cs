using MovieSearchModels;

namespace MovieSearchClientServices;

/// <summary>
/// Client Movie service. Provides access to groups, movies and pictures from a REST server
/// </summary>

public class TMovieService : ALoggable, IMovieService {

  #region --- Constants --------------------------------------------
  private int HTTP_TIMEOUT_IN_MS = 10000;
  #endregion --- Constants --------------------------------------------

  public string ApiBase { get; set; }

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  private readonly THttpClientEx _Client;
  private readonly TImageCache _ImagesCache;

  public TMovieService(string apiServer, TImageCache imagesCache) {
    SetLogger(new TTraceLogger());
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
  #region --- Movie actions --------------------------------------------
  public async Task<IMoviesPage> GetMovies(string filterName, int days = 0, int startPage = 1, int pageSize = 20) {
    try {

      string RequestUrl = $"movie?filtername={filterName.ToUrl()}&days={days}&page={startPage}&size={pageSize}";
      Logger?.LogDebug($"Requesting movies : {RequestUrl}");

      using (CancellationTokenSource Timeout = new CancellationTokenSource(HTTP_TIMEOUT_IN_MS)) {

        string JsonMovies = await _Client.GetStringAsync(RequestUrl, Timeout.Token);
        IMoviesPage Result = TMoviesPage.FromJson(JsonMovies);

        Logger?.LogDebugEx(Result.ToString());
        return Result;

      }
    } catch (Exception ex) {
      Logger?.LogError($"Unable to get movies data : {ex.Message}");
      if (ex.InnerException is not null) {
        Logger?.LogError($"  Inner exception : {ex.InnerException.Message}");
        Logger?.LogError($"  Inner call stack : {ex.InnerException.StackTrace}");
      }
      return null;
    }
  }
  #endregion --- Movie actions --------------------------------------------

  #region --- Picture actions --------------------------------------------
  public async Task<byte[]> GetPicture(string id, int w = 128, int h = 160) {
    try {
      string RequestUrl = $"movie/getPicture?id={id.ToUrl64()}&width={w}&height={h}";
      Log($"Requesting picture : {RequestUrl}");
      using (CancellationTokenSource Timeout = new CancellationTokenSource(HTTP_TIMEOUT_IN_MS)) {
        byte[] Result = await _Client.GetByteArrayAsync(RequestUrl, Timeout.Token);
        if (Result is null) {
          return AMovie.PictureMissing;
        } else {
          return Result;
        }
      }
    } catch (Exception ex) {
      LogError($"Unable to get movies data : {ex.Message}");
      return TMovie.PictureMissing;
    }
  }

  public async Task<string> GetPicture64(IMovie movie) {

    byte[] CachedPicture = _ImagesCache.GetImage(movie.Id);
    byte[] PictureBytes;
    if (CachedPicture is null) {
      PictureBytes = await GetPicture(movie.Id);
      _ImagesCache.AddImage(movie.Id, PictureBytes);
    } else {
      PictureBytes = new byte[CachedPicture.Length];
      CachedPicture.CopyTo(PictureBytes, 0);
    }

    if (PictureBytes is not null) {
      return $"data:image/jpg;base64, {Convert.ToBase64String(PictureBytes)}";
    } else {
      return "";
    }
  }
  #endregion --- Picture actions --------------------------------------------
}
