using Microsoft.AspNetCore.Mvc;

namespace MediaSearch.Server.Controllers;

[ApiController]
[Route("api/about")]
public class AboutController : AController {

  public AboutController(ILogger logger) : base(logger) {
  }

  [HttpGet()]
  public async Task<ActionResult<TAbout>> GetAbout(string name) {

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
