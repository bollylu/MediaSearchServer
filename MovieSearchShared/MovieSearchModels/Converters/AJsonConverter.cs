using System.Text.Json;
using System.Text.Json.Serialization;

using BLTools.Diagnostic.Logging;

using static BLTools.Text.TextBox;

namespace MovieSearch.Models {
  public abstract class AJsonConverter<T> : JsonConverter<T>, ILoggable {

    #region --- ILoggable --------------------------------------------
    public ILogger Logger { get; set; }

    public void SetLogger(ILogger logger) {
      SetLogger(logger, 0);
    }
    public void SetLogger(ILogger logger, int indent) {
      if (logger is null) {
        Logger = ALogger.SYSTEM_LOGGER;
      } else {
        Logger = ALogger.Create(logger);
      }
      _Indent = indent;
    }
    #endregion --- ILoggable --------------------------------------------

    protected int _Indent;

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    protected AJsonConverter() {
      SetLogger(ALogger.DEFAULT_LOGGER);
    }

    protected AJsonConverter(ILogger logger, int indent = 0) {
      SetLogger(logger, indent);
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public void LogValue(object value) {
      Logger?.Log($"{Spaces(_Indent + 4)} => {value}");
    }

    public void LogProperty(string property) {
      Logger?.Log($"{Spaces(_Indent + 2)} => {property}");
    }

    public void LogToken(JsonTokenType tokenType) {
      Logger?.Log($"{Spaces(_Indent)}Token => {tokenType}");
    }

    
    

  }
}
