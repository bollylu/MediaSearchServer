using System.Net;

using Microsoft.AspNetCore.Mvc;

namespace MediaSearch.Server.Controllers;

[ApiController]
[Route("api")]
public class TLoginController : ControllerBase, ILoggable {

  private readonly ILoginService _LoginService;
  private readonly IAuditService _AuditService = GlobalSettings.AuditService ?? throw new ApplicationException("Missing audit service");

  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TLoginController>();

  public TLoginController(ILoginService loginService) {
    Logger.LogDebugEx("Building Login controller");
    _LoginService = loginService;
  }

  [HttpPost("login")]
  public async Task<ActionResult<TUserAccountInfo>> Login(TUserAccountSecret user) {

    Logger.IfDebugMessage("Headers", HttpContext.Request.GetHeaders());
    IPAddress RemoteIP = IPAddress.Parse(HttpContext.Request.Headers["X-Real-IP"].SingleOrDefault() ?? "127.0.0.1") ?? IPAddress.Loopback;
    Logger.IfDebugMessage("Remote ip", RemoteIP);

    _AuditService.Audit(user.Name, $"Login request from {RemoteIP}");

    await Task.Yield();

    if (user is null) {
      Logger.LogWarningBox($"Invalid request : {HttpContext.Request.ContentType ?? "(no ContentType)"}", HttpContext.Request.GetHeaders());
      return BadRequest();
    }

    if (!string.IsNullOrWhiteSpace(user.PasswordHash)) {
      IUserAccountInfo? TestLogin = await _LoginService.Login(user);
      if (TestLogin is not null) {
        TUserAccountInfo UserAccountInfo = new TUserAccountInfo(TestLogin);
        UserAccountInfo.RemoteIp = RemoteIP;
        return new ActionResult<TUserAccountInfo>(UserAccountInfo);
      }
    }

    return new UnauthorizedResult();
  }


}
