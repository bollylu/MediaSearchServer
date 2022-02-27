using BLTools.Diagnostic.Logging;

namespace MediaSearch.Models.Logging;

public abstract class AMediaSearchLogger : ALogger, IMediaSearchLogger {

  public static IMediaSearchLogger Create() {
    return new TMediaSearchLoggerConsole();
  }

  public static IMediaSearchLogger Create(IMediaSearchLogger logger) {
    switch (logger) {
      case TMediaSearchLoggerFile FileLogger:
        return new TMediaSearchLoggerFile(FileLogger);

      case TMediaSearchLoggerConsole ConsoleLogger:
        return new TMediaSearchLoggerConsole(logger);

      default:
        throw new ApplicationException($"Invalid IMediaSearchLogger type : {logger.GetType().Name}");
    }
  }

  public object Clone() {
    return (AMediaSearchLogger)Clone();
  }

  public string Name { get; set; } = "";
  public string Description { get; set; } = "";
}

public abstract class AMediaSearchLogger<T> : ALogger, IMediaSearchLogger<T> {

  public static IMediaSearchLogger<T> Create() {
    return new TMediaSearchLoggerConsole<T>();
  }

  public static IMediaSearchLogger<T> Create(IMediaSearchLogger<T> logger) {

    return (IMediaSearchLogger<T>)logger.Clone();

    //switch (logger) {
    //  case TMediaSearchLoggerFile<T> FileLogger:
    //    return new TMediaSearchLoggerFile<T>(FileLogger);

    //  case TMediaSearchLoggerConsole<T> ConsoleLogger:
    //    return new TMediaSearchLoggerConsole<T>(ConsoleLogger);

    //  default:
    //    throw new ApplicationException($"Invalid IMediaSearchLogger<T> type : {logger.GetType().Name}");
    //}
  }

  public object Clone() {
    return (AMediaSearchLogger<T>)Clone();
  }

  public string Name { get; set; } = "";
  public string Description { get; set; } = "";
}
