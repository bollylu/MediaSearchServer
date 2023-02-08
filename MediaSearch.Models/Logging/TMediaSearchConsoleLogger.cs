using BLTools.Diagnostic.Logging;

namespace MediaSearch.Models.Logging;
public class TMediaSearchConsoleLogger : TConsoleLogger, IMediaSearchLogger {

  public TMediaSearchConsoleLogger() { }
  public TMediaSearchConsoleLogger(IMediaSearchLogger logger) : base(logger) { }

  public object Clone() {
    return (TMediaSearchConsoleLogger)Clone();
  }
  public string Description { get; set; } = "";
}

public class TMediaSearchLoggerConsole<T> : TConsoleLogger, IMediaSearchLogger<T> {

  public TMediaSearchLoggerConsole() { }
  public TMediaSearchLoggerConsole(TConsoleLogger logger) : base(logger) {
    SeverityLimit = logger.SeverityLimit;
  }

  public object Clone() {
    return (TMediaSearchLoggerConsole<T>)Clone();
  }
  public string Description { get; set; } = "";
}
