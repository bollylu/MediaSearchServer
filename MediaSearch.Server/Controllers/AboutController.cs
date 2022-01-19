using Microsoft.AspNetCore.Mvc;

namespace MediaSearch.Server.Controllers;

[ApiController]
[Route("api/about")]
public class AboutController : AController {

  public AboutController(ILogger logger) : base(logger) {
  }

  [HttpGet()]
  public ActionResult<IAbout> Index() {
    return new ActionResult<IAbout>(Program.EntryAbout);
  }
}
