using Microsoft.AspNetCore.Mvc;

namespace MediaSearch.Server.Controllers;

public abstract class AController : ControllerBase, ILoggable {

  #region --- ILoggable --------------------------------------------
  public ILogger Logger { get; set; }

  [NonAction]
  public void SetLogger(ILogger logger) {
    if (logger is null) {
      Logger = new TConsoleLogger();
    } else {
      Logger = ALogger.Create(logger);
    }
  }

  [NonAction]
  protected void Log(string message) => Logger.Log(message, this.GetType().Name);
  [NonAction]
  protected void LogDebug(string message) => Logger.LogDebug(message, this.GetType().Name);
  [NonAction]
  protected void LogDebugEx(string message) => Logger.LogDebugEx(message, this.GetType().Name);
  [NonAction]
  protected void LogWarning(string message) => Logger.LogWarning(message, this.GetType().Name);
  [NonAction]
  protected void LogError(string message) => Logger.LogError(message, this.GetType().Name);
  [NonAction]
  protected void LogFatal(string message) => Logger.LogFatal(message, this.GetType().Name);

  #endregion --- ILoggable --------------------------------------------

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
  protected AController(ILogger logger) {
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    SetLogger(logger);
  }

}
