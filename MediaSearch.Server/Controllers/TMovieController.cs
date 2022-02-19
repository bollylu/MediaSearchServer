using MediaSearch.Models;

using Microsoft.AspNetCore.Mvc;

namespace MediaSearch.Server.Controllers;

[ApiController]
[Route("api/movie")]
[Produces("application/json")]
public class TMovieController : AController {

  private readonly IMovieService _MovieService;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMovieController(IMovieService movieService, ILogger logger) : base(logger) {
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

    LogDebugBox("Headers", HttpContext.Request.Headers);

    if (filter is null) {
      filter = TFilter.Empty;
    }

    LogDebugBox("Filter received in controller", filter);

    TMoviesPage? RetVal = await _MovieService.GetMoviesPage(filter).ConfigureAwait(false);

    if (RetVal is null) {
      return new EmptyResult();
    }

    LogDebugBox("Returned value", RetVal);

    LogDebugExBox("Movies", RetVal.Movies);

    return new ActionResult<TMoviesPage>(RetVal);
  }

  /// <summary>
  /// Obtain a page of movies with the possibility of filtering
  /// </summary>
  /// <param name="filter">A possible filter for the movie names</param>
  /// <returns>A list of groups</returns>
  [HttpGet("getGroups")]
  public async Task<ActionResult<IList<string>>> GetGroups() {
    LogDebug(HttpContext.Request.ListHeaders());
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
    LogDebug(HttpContext.Request.ListHeaders());
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
    LogDebug(HttpContext.Request.ListHeaders());

    IMoviesPage? RetVal = await _MovieService.GetMoviesPage(filter).ConfigureAwait(false);

    if (RetVal is null) {
      return new EmptyResult();
    }

    LogDebug($"< {RetVal}");
    LogDebugEx(_PrintMovies(RetVal.Movies));

    return new ActionResult<IMoviesPage>(RetVal);
  }



  [HttpGet("getPicture")]
  public async Task<ActionResult> GetPicture(string id, int width, int height) {
    LogDebug($"Request for picture {id}, width={width}, height={height}");
    LogDebug(HttpContext.Request.ListHeaders());

    try {
      string PictureId = id.FromUrl64();
      byte[] Result = await _MovieService.GetPicture(id.FromUrl64(),
                                                   "folder.jpg",
                                                   width: width,
                                                   height: height).ConfigureAwait(false);
      if (Result is null || Result.IsEmpty()) {
        LogWarning($"Picture {id} not found");
        byte[] MissingPicture = MediaSearch.Models.Support.GetPicture("missing", ".jpg");
        return new NotFoundObjectResult(MissingPicture);
      } else {
        return File(Result, "image/jpeg");
      }
    } catch (Exception ex) {
      LogWarning($"Invalid picture id : {id} : {ex.Message}");
      return new NotFoundObjectResult(MediaSearch.Models.Support.GetPicture("missing"));
    }

  }

  #region --- Support --------------------------------------------
  private string _PrintMovies(IEnumerable<IMovie> movies) {
    StringBuilder RetVal = new();
    foreach (IMovie MovieItem in movies) {
      RetVal.AppendLine($"  {MovieItem.FileName}");
    }
    return RetVal.ToString();
  }
  #endregion --- Support --------------------------------------------
}

