using Microsoft.AspNetCore.Mvc;

namespace MediaSearch.Server.Controllers;

[ApiController]
[Route("api")]
public class TAboutController : AController {

  public TAboutController(ILogger logger) : base(logger) {
    Logger.LogDebug("Building About controller");
  }

  [HttpGet()]
  public ActionResult Index() {
    return Ok();
  }

  [HttpGet("about")]
  public async Task<ActionResult<TAbout>> GetAbout(string name = "MediaSearch.Server") {

    LogDebug(HttpContext.Request.ListHeaders());
    LogDebug(HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Remote ip not found");

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
