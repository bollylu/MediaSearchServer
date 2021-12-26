using Microsoft.AspNetCore.Mvc;

namespace MediaSearch.Server.Controllers;

[ApiController]
[Route("api/movie")]
[Produces("application/json")]
public class TMovieController : AController {

  private readonly IMovieService _MovieService;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMovieController(IMovieService movieService, ILogger logger) : base(logger) {
    Logger?.LogDebugEx("Building TMovie controller");
    _MovieService = movieService;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  /// <summary>
  /// Obtain a page of movies with the possibility of filtering
  /// </summary>
  /// <param name="filter">A possible filter for the movie names</param>
  /// <param name="page">The first page (x items count)</param>
  /// <param name="items">The items count for the request</param>
  /// <returns>A IMoviesPage object containing the data</returns>
  [HttpGet()]
  public async Task<ActionResult<IMoviesPage>> Get(string filterName = "", string keywordsSelection = "Any", int days = 0, int page = 1, int items = 20) {
    Logger?.LogDebug($"Request : {HttpContext.Connection.RemoteIpAddress}:{HttpContext.Connection.Id} > {HttpContext.Request.QueryString}");

    days = days.WithinLimits(0, int.MaxValue);
    page = page.WithinLimits(1, int.MaxValue);
    items = items.WithinLimits(1, int.MaxValue);

    RFilter Filter = new RFilter() {
      Name = string.IsNullOrWhiteSpace(filterName) ? "" : filterName.FromUrl(),
      DaysBack = days,
      KeywordsSelection = Enum.Parse<EFilterKeywords>(keywordsSelection),
    };

    IMoviesPage RetVal = await _MovieService.GetMoviesPage(Filter, page, items).ConfigureAwait(false);

    Logger?.LogDebug($"< {RetVal}");
    Logger?.LogDebugEx(_PrintMovies(RetVal.Movies));

    return new ActionResult<IMoviesPage>(RetVal);
  }

  /// <summary>
  /// Obtain a page of movies with the possibility of filtering
  /// </summary>
  /// <param name="filter">A possible filter for the movie names</param>
  /// <param name="page">The first page (x items count)</param>
  /// <param name="items">The items count for the request</param>
  /// <returns>A IMoviesPage object containing the data</returns>
  [HttpGet("getFiltered")]
  public async Task<ActionResult<IMoviesPage>> GetFiltered(string filter = "", int page = 1, int items = 20) {
    Logger?.LogDebug($"Request : {HttpContext.Connection.RemoteIpAddress}:{HttpContext.Connection.Id} > {HttpContext.Request.QueryString}");
    RFilter Filter = string.IsNullOrWhiteSpace(filter) ? new RFilter() : RFilter.FromJson(filter);
    Logger?.LogDebug($"  Filter : {Filter}");

    page = page.WithinLimits(1, int.MaxValue);
    items = items.WithinLimits(1, int.MaxValue);

    IMoviesPage RetVal = await _MovieService.GetMoviesPage(Filter, page, items).ConfigureAwait(false);

    Logger?.LogDebug($"< {RetVal}");
    Logger?.LogDebugEx(_PrintMovies(RetVal.Movies));

    return new ActionResult<IMoviesPage>(RetVal);
  }

  /// <summary>
  /// Obtain a page of movies filtered by added in the last n days
  /// </summary>
  /// <param name="days">The number of days from today</param>
  /// <param name="page">The first page (x items count)</param>
  /// <param name="items">The items count for the request</param>
  /// <returns>A IMoviesPage object containing the data</returns>
  [HttpGet("getNews")]
  public async Task<ActionResult<IMoviesPage>> GetNews(int days, int page = 1, int items = 20) {
    Logger?.LogDebug($"Request : {HttpContext.Connection.RemoteIpAddress}:{HttpContext.Connection.Id} > {HttpContext.Request.QueryString}");

    days = days.WithinLimits(0, int.MaxValue);
    page = page.WithinLimits(1, int.MaxValue);
    items = items.WithinLimits(1, int.MaxValue);

    RFilter Filter = new RFilter() {
      DaysBack = days
    };
    IMoviesPage RetVal = await _MovieService.GetMoviesPage(Filter, page, items).ConfigureAwait(false);

    //if (RetVal.AvailablePages < page) {
    //  return BadRequest();
    //}

    Logger?.LogDebug($"< {RetVal}");
    Logger?.LogDebugEx(_PrintMovies(RetVal.Movies));

    return new ActionResult<IMoviesPage>(RetVal);
  }



  [HttpGet("getPicture")]
  public async Task<ActionResult> GetPicture(string id, int width, int height) {
    Logger?.LogDebug($"Request for picture {id}, width={width}, height={height}");
    byte[] Result = await _MovieService.GetPicture(id.FromUrl64(),
                                                   "folder.jpg",
                                                   width: width,
                                                   height: height).ConfigureAwait(false);
    if (Result is null) {
      Logger?.LogWarning($"Picture {id} not found");
      return new NotFoundResult();
    } else {
      return File(Result, "image/jpeg");
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

