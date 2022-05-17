using BLTools.Diagnostic.Logging;

namespace MediaSearch.Models.Logging;
public class TMediaSearchLoggerConsole : TConsoleLogger, IMediaSearchLogger {

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSearchLoggerConsole() { }
  public TMediaSearchLoggerConsole(TConsoleLogger logger) : base(logger) { } 
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public virtual object Clone() {
    return (TMediaSearchLoggerConsole)Clone();
  }
  public string Name { get; set; } = "";
  public string Description { get; set; } = "";
}

public class TMediaSearchLoggerConsole<T> : TMediaSearchLoggerConsole, IMediaSearchLogger<T> {

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSearchLoggerConsole() { }
  public TMediaSearchLoggerConsole(TConsoleLogger logger) : base(logger) {
    SeverityLimit = logger.SeverityLimit;
  } 
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override object Clone() {
    return (TMediaSearchLoggerConsole<T>)Clone();
  }
  
}
