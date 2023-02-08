
using Microsoft.AspNetCore.Mvc;

namespace MediaSearch.Server.Controllers;

[ApiController]
[Route("api/system")]
public class TSystemController : ControllerBase, ILoggable {

  private readonly IMovieService _MovieService;

  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TSystemController>();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TSystemController(IMovieService movieService) {
    _MovieService = movieService;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  /// <summary>
  /// Refresh the selection from disk to cache
  /// </summary>
  /// <returns>A message</returns>
  [HttpGet("startRefreshData")]
  public IActionResult StartRefreshData() {
    Logger?.Log("Request to refresh the data");
    Logger?.Log($"Origin : {HttpContext.Connection.RemoteIpAddress}:{HttpContext.Connection.RemotePort}");
    Task.Run(async () => await _MovieService.RefreshData()).ConfigureAwait(false);
    return Ok();
  }

  /// <summary>
  /// Refresh the selection from disk to cache
  /// </summary>
  /// <returns>The number of procesed records or -1 if completed</returns>
  [HttpGet("getRefreshStatus")]
  public ActionResult<int> GetRefreshStatus() {
    int RetVal = _MovieService.GetRefreshStatus();
    return new ActionResult<int>(RetVal);
  }


}
