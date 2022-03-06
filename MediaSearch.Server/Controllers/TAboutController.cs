using Microsoft.AspNetCore.Mvc;

namespace MediaSearch.Server.Controllers;

[ApiController]
[Route("api")]
public class TAboutController : ControllerBase, IMediaSearchLoggable<TAboutController> {

  public IMediaSearchLogger<TAboutController> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TAboutController>();

  [HttpGet()]
  public ActionResult Index() {
    return Ok();
  }

  [HttpGet("about")]
  public async Task<ActionResult<TAbout>> GetAbout(string name = "MediaSearch.Server") {

    Logger.LogDebugEx(HttpContext.Request.ListHeaders());
    Logger.LogDebugBox("Remote IP", HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Remote ip not found");

    switch (name.ToLower()) {
      case "server": {
          TAbout About = new TAbout(AppDomain.CurrentDomain.GetAssemblies().Single(a => (a.GetName()?.Name ?? "").Equals("MediaSearch.Server", StringComparison.InvariantCultureIgnoreCase)));
          await About.Initialize();
          return new ActionResult<TAbout>(About);
        }
      case "serverservices": {
          TAbout About = new TAbout(AppDomain.CurrentDomain.GetAssemblies().Single(a => (a.GetName()?.Name ?? "").Equals("MediaSearch.Server.Services", StringComparison.InvariantCultureIgnoreCase)));
          await About.Initialize();
          return new ActionResult<TAbout>(About);
        }
      default: {
          return new BadRequestResult();
        }
    };
  }

  
}
