using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaSearch.Models;

public abstract class AJson<T> : ILoggable, IJson<T> where T : IJson<T>, new() {

  #region --- ILoggable --------------------------------------------
  public ILogger Logger { get; set; }
  public void SetLogger(ILogger logger) {
    Logger = ALogger.Create(logger);
  }
  #endregion --- ILoggable --------------------------------------------

  #region --- ToJson --------------------------------------------
  public virtual string ToJson() {

    return ToJson(new JsonWriterOptions());

  }

  public abstract string ToJson(JsonWriterOptions options);
  #endregion --- ToJson --------------------------------------------

  #region --- ParseJson --------------------------------------------
  public T ParseJson(JsonElement source) {
    return ParseJson(source.GetRawText());
  }

  public abstract T ParseJson(string source);
  #endregion --- ParseJson --------------------------------------------

  #region --- Static FromJson --------------------------------------------
  public static T FromJson(JsonElement source) {
    if (source.ValueKind != JsonValueKind.Object) {
      throw new JsonException("Json movie source is not an object");
    }

    return FromJson(source.GetRawText());
  }

  public static T FromJson(string source) {
    T RetVal = new T();
    return RetVal.ParseJson(source);
  }


  #endregion --- Static FromJson --------------------------------------------

}
