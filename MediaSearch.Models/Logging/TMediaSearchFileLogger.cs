using BLTools.Diagnostic.Logging;

namespace MediaSearch.Models.Logging;

public class TMediaSearchFileLogger : TFileLogger, IMediaSearchLogger {

  public TMediaSearchFileLogger(string filename) : base(filename) {
  }

  public TMediaSearchFileLogger(TFileLogger logger) : base(logger) {
  }

  public object Clone() {
    return (TMediaSearchFileLogger)Clone();
  }

  //public string Name { get; set; } = "";
  public string Description { get; set; } = "";
}

public class TMediaSearchLoggerFile<T> : TFileLogger, IMediaSearchLogger<T> {

  public TMediaSearchLoggerFile(string filename) : base(filename) {
  }

  public TMediaSearchLoggerFile(TFileLogger logger) : base(logger) {
  }

  public object Clone() {
    return (TMediaSearchLoggerFile<T>)Clone();
  }

  //public string Name { get; protected set; } = "";
  public string Description { get; set; } = "";
}
