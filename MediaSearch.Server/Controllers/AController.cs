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
  protected void LogBox(string title, string message) => Logger.Log(message.BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH), this.GetType().Name);
  [NonAction]
  protected void LogBox(string? title, object message) => Logger.Log(message.ToString().BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH), this.GetType().Name);
  [NonAction]
  protected void LogDebug(string message) => Logger.LogDebug(message, this.GetType().Name);
  [NonAction]
  protected void LogDebugBox(string title, string message) => Logger.LogDebug(message.BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH), this.GetType().Name);
  [NonAction]
  protected void LogDebugBox(string? title, object message) => Logger.LogDebug(message.ToString().BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH), this.GetType().Name);
  [NonAction]
  protected void LogDebugBox(string? title, IEnumerable<object> message) {
    if (Logger.SeverityLimit >= ESeverity.Debug) {
      if (message is null) {
        Logger.LogDebugEx($"message is null".BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH));
        return;
      }
      if (message.IsEmpty()) {
        Logger.LogDebug($"{message.GetType().Name} is empty".BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH));
        return;
      }
      StringBuilder sb = new();
      foreach (object ObjectItem in message) {
        sb.AppendLine(ObjectItem.ToString());
      }
      Logger.LogDebug(sb.ToString().BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH));
    }
  }

  [NonAction]
  protected void LogDebugEx(string message) => Logger.LogDebugEx(message, this.GetType().Name);
  [NonAction]
  protected void LogDebugExBox(string title, string message) => Logger.LogDebugEx(message.BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH), this.GetType().Name);
  [NonAction]
  protected void LogDebugExBox(string? title, object message) => Logger.LogDebugEx(message.ToString().BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH), this.GetType().Name);
  [NonAction]
  protected void LogDebugExBox(string? title, IEnumerable<object> message) {
    if (Logger.SeverityLimit >= ESeverity.Debug) {
      if (message is null) {
        Logger.LogDebugEx($"message is null".BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH));
        return;
      }
      if (message.IsEmpty()) {
        Logger.LogDebugEx($"{message.GetType().Name} is empty".BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH));
        return;
      }
      StringBuilder sb = new();
      foreach (object ObjectItem in message) {
        sb.AppendLine(ObjectItem.ToString());
      }
      Logger.LogDebugEx(sb.ToString().BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH));
    }
  }

  [NonAction]
  protected void LogWarning(string message) => Logger.LogWarning(message, this.GetType().Name);
  [NonAction]
  protected void LogWarningBox(string title, string message) => Logger.LogWarning(message.BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH), this.GetType().Name);
  [NonAction]
  protected void LogWarningBox(string? title, object message) => Logger.LogWarning(message.ToString().BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH), this.GetType().Name);
  [NonAction]
  protected void LogError(string message) => Logger.LogError(message, this.GetType().Name);
  [NonAction]
  protected void LogErrorBox(string title, string message) => Logger.LogError(message.BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH), this.GetType().Name);
  [NonAction]
  protected void LogErrorBox(string? title, object message) => Logger.LogError(message.ToString().BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH), this.GetType().Name);
  [NonAction]
  protected void LogFatal(string message) => Logger.LogFatal(message, this.GetType().Name);
  [NonAction]
  protected void LogFatalBox(string title, string message) => Logger.LogFatal(message.BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH), this.GetType().Name);
  [NonAction]
  protected void LogFatalBox(string? title, object message) => Logger.LogFatal(message.ToString().BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH), this.GetType().Name);

  #endregion --- ILoggable --------------------------------------------

  protected AController(ILogger logger) {
    Logger = ALogger.Create(logger);
  }

}
