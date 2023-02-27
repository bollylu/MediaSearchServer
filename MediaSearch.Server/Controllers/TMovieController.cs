using Microsoft.AspNetCore.Mvc;

namespace MediaSearch.Server.Controllers;

[ApiController]
[Route("api/movie")]
[Produces("application/json")]
public class TMovieController : ControllerBase, ILoggable {

  private readonly IMovieService _MovieService;

  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TMovieController>();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMovieController(IMovieService movieService) {
    Logger.LogDebugEx("Building TMovie controller");
    _MovieService = movieService;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  /// <summary>
  /// Obtain a page of movies with the possibility of filtering
  /// </summary>
  /// <param name="filter">A possible filter for the movie names</param>
  /// <returns>A IMoviesPage object containing the data</returns>
  [HttpPost()]
  public async Task<ActionResult<TMoviesPage>> GetWithFilter(TFilter filter) {

    Logger.LogDebugBox("Headers", HttpContext.Request.Headers);

    if (filter is null) {
      filter = TFilter.Empty;
    }

    Logger.LogDebugBox("Filter received in controller", filter);

    TMoviesPage? RetVal = await _MovieService.GetMoviesPage(filter).ConfigureAwait(false) as TMoviesPage;

    if (RetVal is null) {
      return new EmptyResult();
    }

    Logger.LogDebugBox("Returned value", RetVal);

    Logger.LogDebugExBox("Movies", RetVal.Movies);

    return new ActionResult<TMoviesPage>(RetVal);
  }

  /// <summary>
  /// Obtain a page of movies with the possibility of filtering
  /// </summary>
  /// <param name="filter">A possible filter for the movie names</param>
  /// <returns>A list of groups</returns>
  [HttpGet("getGroups")]
  public async Task<ActionResult<IList<string>>> GetGroups() {
    Logger.LogDebug(HttpContext.Request.ListHeaders());
    IList<string> RetVal = new List<string>();
    await foreach (string GroupItem in _MovieService.GetGroups().ConfigureAwait(false)) {
      RetVal.Add(GroupItem);
    }

    return new ActionResult<IList<string>>(RetVal);
  }

  /// <summary>
  /// Obtain a page of movies with the possibility of filtering
  /// </summary>
  /// <param name="filter">A possible filter for the movie names</param>
  /// <returns>A list of groups</returns>
  [HttpGet("getSubGroups")]
  public async Task<ActionResult<IList<string>>> GetSubGroups(string group) {
    Logger.LogDebug(HttpContext.Request.ListHeaders());
    IList<string> RetVal = await _MovieService.GetSubGroups(group ?? "").ToListAsync().ConfigureAwait(false);

    return new ActionResult<IList<string>>(RetVal);
  }

  /// <summary>
  /// Obtain a page of movies filtered by added in the last n days
  /// </summary>
  /// <param name="days">The number of days from today</param>
  /// <param name="page">The first page (x items count)</param>
  /// <param name="items">The items count for the request</param>
  /// <returns>A IMoviesPage object containing the data</returns>
  [HttpPost("getNews")]
  public async Task<ActionResult<IMoviesPage>> GetNews(TFilter filter) {
    Logger.LogDebug(HttpContext.Request.ListHeaders());

    IMoviesPage? RetVal = await _MovieService.GetMoviesPage(filter).ConfigureAwait(false);

    if (RetVal is null) {
      return new EmptyResult();
    }

    Logger.LogDebug($"< {RetVal}");
    Logger.LogDebugEx(_PrintMovies(RetVal.Movies));

    return new ActionResult<IMoviesPage>(RetVal);
  }



  [HttpGet("getPicture")]
  public async Task<ActionResult> GetPicture(string movieId, int width, int height) {
    Logger.LogDebug($"Request for picture {movieId.FromUrl64()}, width={width}, height={height}");
    Logger.LogDebug(HttpContext.Request.ListHeaders());

    try {
      string MovieId = movieId.FromUrl64();

      byte[]? Result = await _MovieService.GetPicture(MovieId, "folder.jpg", width, height).ConfigureAwait(false);
      if (Result is null || Result.IsEmpty()) {
        Logger.LogWarning($"Picture {movieId} not found");
        byte[] MissingPicture = MediaSearch.Models.Support.GetPicture("missing", ".jpg");
        return File(MissingPicture, "image/jpeg");
      }

      return File(Result, "image/jpeg");

    } catch (Exception ex) {
      Logger.LogWarning($"Invalid picture id : {movieId} : {ex.Message}");
      return new NotFoundObjectResult(MediaSearch.Models.Support.GetPicture("missing"));
    }

  }

  #region --- Support --------------------------------------------
  private string _PrintMovies(IEnumerable<IMovie> movies) {
    StringBuilder RetVal = new();
    foreach (IMovie MovieItem in movies) {
      RetVal.AppendLine($"  {MovieItem.Name}");
    }
    return RetVal.ToString();
  }
  #endregion --- Support --------------------------------------------
}

