using Microsoft.AspNetCore.Mvc;

namespace MediaSearch.Server.Controllers;

public abstract class AController : ControllerBase, IMediaSearchLoggable {

  #region --- ILoggable --------------------------------------------
  public IMediaSearchLogger Logger {
    get {
      return _Logger ??= AMediaSearchLogger.Create(GlobalSettings.GlobalLogger);
    }
    set {
      _Logger = value;
    }
  }
  private IMediaSearchLogger? _Logger;

  #endregion --- ILoggable --------------------------------------------

  protected AController(IMediaSearchLogger logger) {
    Logger = AMediaSearchLogger.Create(logger);
  }

}
