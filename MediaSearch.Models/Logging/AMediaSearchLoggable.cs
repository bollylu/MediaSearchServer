using BLTools.Diagnostic.Logging;
using BLTools.Text;

namespace MediaSearch.Models.Logging;
public abstract class AMediaSearchLoggable : IMediaSearchLoggable {

#pragma warning disable IDE0022 // Use block body for methods

  #region --- Log --------------------------------------------
  public void Log(string text) => Logger.Log(text, GetType().Name);
  public void Log(object something) => Logger.Log(something?.ToString() ?? "Invalid content", GetType().Name);
  public void LogBox(string? title, string message) => Logger.Log(message.BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
  public void LogBox(string? title, object message) => Logger.Log(message.ToString().BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
  public void LogBox(string? title, IEnumerable<object> message) {
    if (message is null) {
      Logger.Log($"IEnumerable message is null".BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
      return;
    }

    if (message.IsEmpty()) {
      Logger.Log($"{message.GetType().Name} is empty".BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
      return;
    }

    StringBuilder sb = new();
    foreach (object ObjectItem in message) {
      sb.AppendLine(ObjectItem.ToString());
    }
    Logger.Log(sb.ToString().BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
  }
  #endregion --- Log --------------------------------------------

  #region --- LogWarning --------------------------------------------
  public void LogWarning(string text) => Logger.LogWarning(text, GetType().Name);
  public void LogWarning(object something) => Logger.LogWarning(something?.ToString() ?? "Invalid content", GetType().Name);
  public void LogWarningBox(string? title, string message) => Logger.LogWarning(message.BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
  public void LogWarningBox(string? title, object message) => Logger.LogWarning(message.ToString().BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
  public void LogWarningBox(string? title, IEnumerable<object> message) {
    if (Logger.SeverityLimit < ESeverity.Warning) {
      return;
    }

    if (message is null) {
      Logger.LogWarning($"IEnumerable message is null".BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
      return;
    }

    if (message.IsEmpty()) {
      Logger.LogWarning($"{message.GetType().Name} is empty".BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
      return;
    }

    StringBuilder sb = new();
    foreach (object ObjectItem in message) {
      sb.AppendLine(ObjectItem.ToString());
    }
    Logger.LogWarning(sb.ToString().BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
  }
  #endregion --- LogWarning --------------------------------------------

  #region --- LogError --------------------------------------------
  public void LogError(string text) => Logger.LogError(text, GetType().Name);
  public void LogError(object something) => Logger.LogError(something?.ToString() ?? "Invalid content", GetType().Name);
  public void LogErrorBox(string? title, string message) => Logger.LogError(message.BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
  public void LogErrorBox(string? title, object message) => Logger.LogError(message.ToString().BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
  public void LogErrorBox(string? title, IEnumerable<object> message) {
    if (Logger.SeverityLimit < ESeverity.Error) {
      return;
    }

    if (message is null) {
      Logger.LogError($"IEnumerable message is null".BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
      return;
    }

    if (message.IsEmpty()) {
      Logger.LogError($"{message.GetType().Name} is empty".BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
      return;
    }

    StringBuilder sb = new();
    foreach (object ObjectItem in message) {
      sb.AppendLine(ObjectItem.ToString());
    }
    Logger.LogError(sb.ToString().BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
  }
  public void LogErrorBox(string? title, Exception ex, bool withStack = false) {
    if (Logger.SeverityLimit < ESeverity.Error) {
      return;
    }

    if (ex is null) {
      Logger.LogError($"Exception is null".BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
      return;
    }

    StringBuilder sb = new();
    sb.AppendLine($"Exception source : {ex.Source}");
    sb.AppendLine($"Message: {ex.Message}");
    sb.AppendLine($"TargetSite : {ex.TargetSite?.ToString()}");
    if (withStack) {
      sb.AppendLine(ex.StackTrace);
    }
    Logger.LogError(sb.ToString().BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
  }
  #endregion --- LogError --------------------------------------------

  #region --- LogDebug --------------------------------------------
  public void LogDebug(string text) => Logger.LogDebug(text, GetType().Name);
  public void LogDebug(object something) => Logger.LogDebug(something?.ToString() ?? "Invalid content", GetType().Name);
  public void LogDebugBox(string? title, string message) => Logger.LogDebug(message.BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
  public void LogDebugBox(string? title, object message) => Logger.LogDebug(message.ToString().BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
  public void LogDebugBox(string? title, IEnumerable<object> message) {
    if (Logger.SeverityLimit < ESeverity.Debug) {
      return;
    }

    if (message is null) {
      Logger.LogDebug($"IEnumerable message is null".BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
      return;
    }

    if (message.IsEmpty()) {
      Logger.LogDebug($"{message.GetType().Name} is empty".BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
      return;
    }

    StringBuilder sb = new();
    foreach (object ObjectItem in message) {
      sb.AppendLine(ObjectItem.ToString());
    }
    Logger.LogDebug(sb.ToString().BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
  }
  #endregion --- LogDebug --------------------------------------------

  #region --- LogDebugEx --------------------------------------------
  public void LogDebugEx(string text) => Logger.LogDebugEx(text, GetType().Name);
  public void LogDebugEx(object something) => Logger.LogDebugEx(something?.ToString() ?? "Invalid content", GetType().Name);
  public void LogDebugExBox(string? title, string message) => Logger.LogDebugEx(message.BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
  public void LogDebugExBox(string? title, object message) => Logger.LogDebugEx(message.ToString().BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
  public void LogDebugExBox(string? title, IEnumerable<object> message) {
    if (Logger.SeverityLimit < ESeverity.DebugEx) {
      return;
    }

    if (message is null) {
      Logger.LogDebugEx($"IEnumerable message is null".BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
      return;
    }

    if (message.IsEmpty()) {
      Logger.LogDebugEx($"{message.GetType().Name} is empty".BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
      return;
    }

    StringBuilder sb = new();
    foreach (object ObjectItem in message) {
      sb.AppendLine(ObjectItem.ToString());
    }
    Logger.LogDebugEx(sb.ToString().BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
  }
  #endregion --- LogDebugEx --------------------------------------------

  #region --- LogFatal --------------------------------------------
  public void LogFatal(string text) => Logger.LogFatal(text, GetType().Name);
  public void LogFatal(object something) => Logger.LogFatal(something?.ToString() ?? "Invalid content", GetType().Name);
  public void LogFatalBox(string? title, string message) => Logger.LogFatal(message.BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
  public void LogFatalBox(string? title, object message) => Logger.LogFatal(message.ToString().BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
  public void LogFatalBox(string? title, IEnumerable<object> message) {
    if (Logger.SeverityLimit < ESeverity.Debug) {
      return;
    }

    if (message is null) {
      Logger.LogFatal($"IEnumerable message is null".BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
      return;
    }

    if (message.IsEmpty()) {
      Logger.LogFatal($"{message.GetType().Name} is empty".BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
      return;
    }

    StringBuilder sb = new();
    foreach (object ObjectItem in message) {
      sb.AppendLine(ObjectItem.ToString());
    }
    Logger.LogFatal(sb.ToString().BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH), GetType().Name);
  }
  #endregion --- LogFatal --------------------------------------------

#pragma warning restore IDE0022 // Use block body for methods

  public virtual IMediaSearchLogger Logger { get; set; } = GlobalSettings.GlobalLogger;
}
