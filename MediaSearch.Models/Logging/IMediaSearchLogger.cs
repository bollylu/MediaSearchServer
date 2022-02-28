using System.Runtime.CompilerServices;

using BLTools.Diagnostic.Logging;
using BLTools.Text;

namespace MediaSearch.Models.Logging;

/// <summary>
/// Extension of ILogger. Allows more logging methods
/// </summary>
public interface IMediaSearchLogger : ILogger, IName {

  new string Name { get; set; }

  #region --- Log --------------------------------------------
  public void Log(string message, Type? callerType = default, [CallerMemberName] string callerName = "") {
    Log(message, _BuildCaller(callerType, callerName), ESeverity.Info);
  }
  public void LogBox(string? title, string message, Type? callerType = default, [CallerMemberName] string callerName = "") {
    Log(_BuildBox(title, message), _BuildCaller(callerType, callerName), ESeverity.Info);
  }

  public void LogBox(string? title, object message, Type? callerType = default, [CallerMemberName] string callerName = "") {
    Log(_BuildBox(title, message), _BuildCaller(callerType, callerName), ESeverity.Info);
  }

  public void LogBox(string? title, IEnumerable<object> objects, Type? callerType = default, [CallerMemberName] string callerName = "") {
    Log(_BuildBoxMessageFromEnumerable(title, objects), _BuildCaller(callerType, callerName), ESeverity.Info);
  }
  #endregion --- Log --------------------------------------------

  #region --- LogWarning --------------------------------------------
  public void LogWarning(string message, Type? callerType = default, [CallerMemberName] string callerName = "", ESeverity severity = ESeverity.Warning) {
    if (SeverityLimit > ESeverity.Warning) {
      return;
    }
    Log(message, _BuildCaller(callerType, callerName), ESeverity.Warning);
  }

  public void LogWarningBox(string? title, string message, Type? callerType = default, [CallerMemberName] string callerName = "") {
    if (SeverityLimit > ESeverity.Warning) {
      return;
    }
    Log(_BuildBox(title, message), _BuildCaller(callerType, callerName), ESeverity.Warning);
  }

  public void LogWarningBox(string? title, object message, Type? callerType = default, [CallerMemberName] string callerName = "") {
    if (SeverityLimit > ESeverity.Warning) {
      return;
    }
    Log(_BuildBox(title, message), _BuildCaller(callerType, callerName), ESeverity.Warning);
  }

  public void LogWarningBox(string? title, IEnumerable<object> objects, Type? callerType = default, [CallerMemberName] string callerName = "") {
    if (SeverityLimit > ESeverity.Warning) {
      return;
    }
    Log(_BuildBoxMessageFromEnumerable(title, objects), _BuildCaller(callerType, callerName), ESeverity.Warning);
  }
  #endregion --- LogWarning --------------------------------------------

  #region --- LogError --------------------------------------------
  public void LogError(string message, Type? callerType = default, [CallerMemberName] string callerName = "", ESeverity severity = ESeverity.Error) {
    if (SeverityLimit > ESeverity.Error) {
      return;
    }
    Log(message, _BuildCaller(callerType, callerName), ESeverity.Error);
  }

  public void LogErrorBox(string? title, string message, Type? callerType = default, [CallerMemberName] string callerName = "") {
    if (SeverityLimit > ESeverity.Error) {
      return;
    }
    Log(_BuildBox(title, message), _BuildCaller(callerType, callerName), ESeverity.Error);
  }

  public void LogErrorBox(string? title, object message, Type? callerType = default, [CallerMemberName] string callerName = "") {
    if (SeverityLimit > ESeverity.Error) {
      return;
    }
    Log(_BuildBox(title, message), _BuildCaller(callerType, callerName), ESeverity.Error);
  }

  public void LogErrorBox(string? title, IEnumerable<object> objects, Type? callerType = default, [CallerMemberName] string callerName = "") {
    if (SeverityLimit > ESeverity.Error) {
      return;
    }
    Log(_BuildBoxMessageFromEnumerable(title, objects), _BuildCaller(callerType, callerName), ESeverity.Error);
  }
  public void LogErrorBox(string? title, Exception ex, bool withStackTrace = false, Type? callerType = default, [CallerMemberName] string callerName = "") {
    if (SeverityLimit > ESeverity.Error) {
      return;
    }
    Log(_BuildBoxMessageFromException(title, ex, withStackTrace), _BuildCaller(callerType, callerName), ESeverity.Error);
  }
  #endregion --- LogError --------------------------------------------

  #region --- LogDebug --------------------------------------------
  public void LogDebug(string message, Type? callerType = default, [CallerMemberName] string callerName = "", ESeverity severity = ESeverity.Debug) {
    if (SeverityLimit > ESeverity.Debug) {
      return;
    }
    Log(message, _BuildCaller(callerType, callerName), ESeverity.Debug);
  }
  public void LogDebugBox(string? title, string message, Type? callerType = default, [CallerMemberName] string callerName = "") {
    if (SeverityLimit > ESeverity.Debug) {
      return;
    }
    Log(_BuildBox(title, message), _BuildCaller(callerType, callerName), ESeverity.Debug);
  }

  public void LogDebugBox(string? title, object message, Type? callerType = default, [CallerMemberName] string callerName = "") {
    if (SeverityLimit > ESeverity.Debug) {
      return;
    }
    Log(_BuildBox(title, message), _BuildCaller(callerType, callerName), ESeverity.Debug);
  }

  public void LogDebugBox(string? title, IEnumerable<object> objects, Type? callerType = default, [CallerMemberName] string callerName = "") {
    if (SeverityLimit > ESeverity.Debug) {
      return;
    }
    Log(_BuildBoxMessageFromEnumerable(title, objects), _BuildCaller(callerType, callerName), ESeverity.Debug);
  }
  #endregion --- LogDebug --------------------------------------------

  #region --- LogDebugEx --------------------------------------------
  public void LogDebugEx(string message, Type? callerType = default, [CallerMemberName] string callerName = "", ESeverity severity = ESeverity.DebugEx) {
    if (SeverityLimit > ESeverity.DebugEx) {
      return;
    }
    Log(message, _BuildCaller(callerType, callerName), ESeverity.DebugEx);
  }

  public void LogDebugExBox(string? title, string message, Type? callerType = default, [CallerMemberName] string callerName = "") {
    if (SeverityLimit > ESeverity.DebugEx) {
      return;
    }
    Log(_BuildBox(title, message), _BuildCaller(callerType, callerName), ESeverity.DebugEx);
  }

  public void LogDebugExBox(string? title, object message, Type? callerType = default, [CallerMemberName] string callerName = "") {
    if (SeverityLimit > ESeverity.DebugEx) {
      return;
    }
    Log(_BuildBox(title, message), _BuildCaller(callerType, callerName), ESeverity.DebugEx);
  }

  public void LogDebugExBox(string? title, IEnumerable<object> objects, Type? callerType = default, [CallerMemberName] string callerName = "") {
    if (SeverityLimit > ESeverity.DebugEx) {
      return;
    }
    Log(_BuildBoxMessageFromEnumerable(title, objects), _BuildCaller(callerType, callerName), ESeverity.DebugEx);
  }
  #endregion --- LogDebugEx --------------------------------------------

  #region --- LogFatal --------------------------------------------
  public void LogFatal(string message, Type? callerType = default, [CallerMemberName] string callerName = "", ESeverity severity = ESeverity.Fatal) {
    if (SeverityLimit > ESeverity.Fatal) {
      return;
    }
    Log(message, _BuildCaller(callerType, callerName), ESeverity.Fatal);
  }

  public void LogFatalBox(string? title, string message, Type? callerType = default, [CallerMemberName] string callerName = "") {
    if (SeverityLimit > ESeverity.Fatal) {
      return;
    }
    Log(_BuildBox(title, message), _BuildCaller(callerType, callerName), ESeverity.Fatal);
  }

  public void LogFatalBox(string? title, object message, Type? callerType = default, [CallerMemberName] string callerName = "") {
    if (SeverityLimit > ESeverity.Fatal) {
      return;
    }
    Log(_BuildBox(title, message), _BuildCaller(callerType, callerName), ESeverity.Fatal);
  }

  public void LogFatalBox(string? title, IEnumerable<object> objects, Type? callerType = default, [CallerMemberName] string callerName = "") {
    if (SeverityLimit > ESeverity.Fatal) {
      return;
    }
    Log(_BuildBoxMessageFromEnumerable(title, objects), _BuildCaller(callerType, callerName), ESeverity.Fatal);
  }
  #endregion --- LogFatal --------------------------------------------

  #region --- Protected members --------------------------------------------
  protected string _BuildBoxMessageFromEnumerable(string? title, IEnumerable<object> objects) {

    if (objects is null) {
      return _BuildBox(title, "IEnumerable objects is null");
    }

    if (objects.IsEmpty()) {
      return _BuildBox(title, $"{objects.GetType().Name} is empty");
    }

    StringBuilder RetVal = new();
    foreach (object ObjectItem in objects) {
      RetVal.AppendLine(ObjectItem.ToString());
    }
    return _BuildBox(title, RetVal.ToString());
  }

  protected string _BuildBoxMessageFromException(string? title, Exception ex, bool withStackTrace) {

    if (ex is null) {
      return _BuildBox(title, "Exception is null");
    }

    StringBuilder sb = new();
    sb.AppendLine($"Exception source : {ex.Source}");
    sb.AppendLine($"Message: {ex.Message}");
    sb.AppendLine($"TargetSite : {ex.TargetSite?.ToString()}");
    if (withStackTrace) {
      sb.AppendLine(ex.StackTrace);
    }
    return sb.ToString().BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH);
  }

  protected string _BuildCaller(Type? callerType, string callerMethod) {
    if (callerType is not null) {
      return $"{callerType.Name}.{callerMethod}";
    } else {
      return $"(unknown type).{callerMethod}";
    }
  }

  protected string _BuildBox(string? title = "", string message = "") {
    return message.BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH);
  }

  protected string _BuildBox(string? title, object? message) {
    title ??= "";
    string? ProcessedMessage = message is null ? "(null)" : message.ToString();
    return ProcessedMessage.BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH);
  }
  #endregion --- Protected members --------------------------------------------
}

/// <summary>
/// Extension of ILogger. Allows more logging methods and fills the source with the type and the method of the caller
/// </summary>
/// <typeparam name="T">The type of the source</typeparam>
public interface IMediaSearchLogger<T> : IMediaSearchLogger, ICloneable {

  #region --- Log --------------------------------------------
  public void Log(string message, [CallerMemberName] string caller = "") {
    Log(message, _BuildCaller(typeof(T), caller), ESeverity.Info);
  }
  public void LogBox(string? title, string message, [CallerMemberName] string caller = "") {
    Log(_BuildBox(title, message), _BuildCaller(typeof(T), caller), ESeverity.Info);
  }

  public void LogBox(string? title, object message, [CallerMemberName] string caller = "") {
    Log(_BuildBox(title, message), _BuildCaller(typeof(T), caller), ESeverity.Info);
  }

  public void LogBox(string? title, IEnumerable<object> objects, [CallerMemberName] string caller = "") {
    Log(_BuildBoxMessageFromEnumerable(title, objects), _BuildCaller(typeof(T), caller), ESeverity.Info);
  }
  #endregion --- Log --------------------------------------------

  #region --- LogWarning --------------------------------------------
  public void LogWarning(string message, [CallerMemberName] string caller = "", ESeverity severity = ESeverity.Warning) {
    if (SeverityLimit > ESeverity.Warning) {
      return;
    }
    Log(message, _BuildCaller(typeof(T), caller), ESeverity.Warning);
  }

  public void LogWarningBox(string? title, string message, [CallerMemberName] string caller = "") {
    if (SeverityLimit > ESeverity.Warning) {
      return;
    }
    Log(_BuildBox(title, message), _BuildCaller(typeof(T), caller), ESeverity.Warning);
  }

  public void LogWarningBox(string? title, object message, [CallerMemberName] string caller = "") {
    if (SeverityLimit > ESeverity.Warning) {
      return;
    }
    Log(_BuildBox(title, message), _BuildCaller(typeof(T), caller), ESeverity.Warning);
  }

  public void LogWarningBox(string? title, IEnumerable<object> objects, [CallerMemberName] string caller = "") {
    if (SeverityLimit > ESeverity.Warning) {
      return;
    }
    Log(_BuildBoxMessageFromEnumerable(title, objects), _BuildCaller(typeof(T), caller), ESeverity.Warning);
  }
  #endregion --- LogWarning --------------------------------------------

  #region --- LogError --------------------------------------------
  public void LogError(string message, [CallerMemberName] string caller = "", ESeverity severity = ESeverity.Error) {
    if (SeverityLimit > ESeverity.Error) {
      return;
    }
    Log(message, _BuildCaller(typeof(T), caller), ESeverity.Error);
  }

  public void LogErrorBox(string? title, string message, [CallerMemberName] string caller = "") {
    if (SeverityLimit > ESeverity.Error) {
      return;
    }
    Log(_BuildBox(title, message), _BuildCaller(typeof(T), caller), ESeverity.Error);
  }

  public void LogErrorBox(string? title, object message, [CallerMemberName] string caller = "") {
    if (SeverityLimit > ESeverity.Error) {
      return;
    }
    Log(_BuildBox(title, message), _BuildCaller(typeof(T), caller), ESeverity.Error);
  }

  public void LogErrorBox(string? title, IEnumerable<object> objects, [CallerMemberName] string caller = "") {
    if (SeverityLimit > ESeverity.Error) {
      return;
    }
    Log(_BuildBoxMessageFromEnumerable(title, objects), _BuildCaller(typeof(T), caller), ESeverity.Error);
  }

  public void LogErrorBox(string? title, Exception ex, bool withStackTrace = false, [CallerMemberName] string caller = "") {
    if (SeverityLimit > ESeverity.Error) {
      return;
    }
    Log(_BuildBoxMessageFromException(title, ex, withStackTrace), _BuildCaller(typeof(T), caller), ESeverity.Error);
  }
  #endregion --- LogError --------------------------------------------

  #region --- LogDebug --------------------------------------------
  public void LogDebug(string message, [CallerMemberName] string caller = "", ESeverity severity = ESeverity.Debug) {
    if (SeverityLimit > ESeverity.Debug) {
      return;
    }
    Log(message, _BuildCaller(typeof(T), caller), ESeverity.Debug);
  }
  public void LogDebugBox(string? title, string message, [CallerMemberName] string caller = "") {
    if (SeverityLimit > ESeverity.Debug) {
      return;
    }
    Log(_BuildBox(title, message), _BuildCaller(typeof(T), caller), ESeverity.Debug);
  }

  public void LogDebugBox(string? title, object message, [CallerMemberName] string caller = "") {
    if (SeverityLimit > ESeverity.Debug) {
      return;
    }
    Log(_BuildBox(title, message), _BuildCaller(typeof(T), caller), ESeverity.Debug);
  }

  public void LogDebugBox(string? title, IEnumerable<object> objects, [CallerMemberName] string caller = "") {
    if (SeverityLimit > ESeverity.Debug) {
      return;
    }
    Log(_BuildBoxMessageFromEnumerable(title, objects), _BuildCaller(typeof(T), caller), ESeverity.Debug);
  }
  #endregion --- LogDebug --------------------------------------------

  #region --- LogDebugEx --------------------------------------------
  public void LogDebugEx(string message, [CallerMemberName] string caller = "", ESeverity severity = ESeverity.DebugEx) {
    if (SeverityLimit > ESeverity.DebugEx) {
      return;
    }
    Log(message, _BuildCaller(typeof(T), caller), ESeverity.DebugEx);
  }

  public void LogDebugExBox(string? title, string message, [CallerMemberName] string caller = "") {
    if (SeverityLimit > ESeverity.DebugEx) {
      return;
    }
    Log(_BuildBox(title, message), _BuildCaller(typeof(T), caller), ESeverity.DebugEx);
  }

  public void LogDebugExBox(string? title, object message, [CallerMemberName] string caller = "") {
    if (SeverityLimit > ESeverity.DebugEx) {
      return;
    }
    Log(_BuildBox(title, message), _BuildCaller(typeof(T), caller), ESeverity.DebugEx);
  }

  public void LogDebugExBox(string? title, IEnumerable<object> objects, [CallerMemberName] string caller = "") {
    if (SeverityLimit > ESeverity.DebugEx) {
      return;
    }
    Log(_BuildBoxMessageFromEnumerable(title, objects), _BuildCaller(typeof(T), caller), ESeverity.DebugEx);
  }
  #endregion --- LogDebugEx --------------------------------------------

  #region --- LogFatal --------------------------------------------
  public void LogFatal(string message, [CallerMemberName] string caller = "", ESeverity severity = ESeverity.Fatal) {
    if (SeverityLimit > ESeverity.Fatal) {
      return;
    }
    Log(message, _BuildCaller(typeof(T), caller), ESeverity.Fatal);
  }

  public void LogFatalBox(string? title, string message, [CallerMemberName] string caller = "") {
    if (SeverityLimit > ESeverity.Fatal) {
      return;
    }
    Log(_BuildBox(title, message), _BuildCaller(typeof(T), caller), ESeverity.Fatal);
  }

  public void LogFatalBox(string? title, object message, [CallerMemberName] string caller = "") {
    if (SeverityLimit > ESeverity.Fatal) {
      return;
    }
    Log(_BuildBox(title, message), _BuildCaller(typeof(T), caller), ESeverity.Fatal);
  }

  public void LogFatalBox(string? title, IEnumerable<object> objects, [CallerMemberName] string caller = "") {
    if (SeverityLimit > ESeverity.Fatal) {
      return;
    }
    Log(_BuildBoxMessageFromEnumerable(title, objects), _BuildCaller(typeof(T), caller), ESeverity.Fatal);
  }
  #endregion --- LogFatal --------------------------------------------

}
