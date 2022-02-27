using Microsoft.AspNetCore.Mvc;

namespace MediaSearch.Server.Controllers;

[ApiController]
[Route("api")]
public class TLoginController : ControllerBase, IMediaSearchLoggable<TLoginController> {

  private readonly ILoginService _LoginService;

  public IMediaSearchLogger<TLoginController> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TLoginController>();

  public TLoginController(ILoginService loginService) {
    Logger.LogDebugEx("Building Login controller");
    _LoginService = loginService;
  }

  [HttpPost("login")]
  public async Task<ActionResult<IUserAccount>> Login(TUserAccountSecret user) {

    Logger.LogDebug(HttpContext.Request.ListHeaders());
    Logger.LogDebug(HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Remote ip not found");

    await Task.Yield();

    if (user is null) {
      Logger.LogWarning("Invalid request");
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
