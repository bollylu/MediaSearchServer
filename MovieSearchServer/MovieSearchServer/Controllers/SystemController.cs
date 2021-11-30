
using Microsoft.AspNetCore.Mvc;

namespace MovieSearchServer.Controllers;

[ApiController]
[Route("api/system")]
public class TSystemController : AController {

  private readonly IMovieService _MovieService;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMovieController(IMovieService movieService, ILogger logger) : base(logger) {
    _MovieService = movieService;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  /// <summary>
  /// Refresh the selection from disk to cache
  /// </summary>
  /// <returns>A message</returns>
  [HttpPost()]
  public async Task<ActionResult<string>> RefreshData() {
    Logger?.Log("Request to refresh the data");
    Logger?.Log($"Origin : {HttpContext.Connection.RemoteIpAddress}:{HttpContext.Connection.RemotePort}");

    _MovieService.Refresh();

    return new ActionResult<string>("Refresh initiated.");
  }
}
