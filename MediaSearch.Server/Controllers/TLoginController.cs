using Microsoft.AspNetCore.Mvc;

namespace MediaSearch.Server.Controllers;

[ApiController]
[Route("api")]
public class TLoginController : AController {

  private readonly ILoginService _LoginService;

  public TLoginController(ILoginService loginService, ILogger logger) : base(logger) {
    Logger.LogDebugEx("Building Login controller");
    _LoginService = loginService;
    _LoginService.SetLogger(logger);
  }

  [HttpPost("login")]
  public async Task<ActionResult<IUserAccount>> Login(TUserAccountSecret user) {

    LogDebug(HttpContext.Request.ListHeaders());
    LogDebug(HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Remote ip not found");

    await Task.Yield();

    if (user is null) {
      LogWarning("Invalid request");
      return BadRequest();
    }
    
    if (!string.IsNullOrWhiteSpace(user.Password)) {
      IUserAccount? TestLogin = _LoginService.Login(user);
      if (TestLogin is null) {
        return new UnauthorizedResult();
      } else {
        return new ActionResult<IUserAccount>(TestLogin);
      }
    }

    return new UnauthorizedResult();
  }
}
