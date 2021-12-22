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
  #endregion --- ILoggable --------------------------------------------

  protected AController(ILogger logger) {
    SetLogger(logger);
  }

}
