using BLTools.Diagnostic.Logging;

namespace MediaSearch.Models.Logging;

public class TMediaSearchLoggerFile : TFileLogger, IMediaSearchLogger {

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSearchLoggerFile(string filename) : base(filename) {
  }

  public TMediaSearchLoggerFile(TFileLogger logger) : base(logger) {
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public virtual object Clone() {
    return (TMediaSearchLoggerFile)Clone();
  }

  public new string Name { get; set; } = "";
  public string Description { get; set; } = "";
}

public class TMediaSearchLoggerFile<T> : TMediaSearchLoggerFile, IMediaSearchLogger<T> {

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSearchLoggerFile(string filename) : base(filename) {
  }

  public TMediaSearchLoggerFile(TFileLogger logger) : base(logger) {
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override object Clone() {
    return (TMediaSearchLoggerFile<T>)Clone();
  }

}
