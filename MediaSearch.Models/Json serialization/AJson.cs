using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MediaSearch.Models;

public abstract class AJson<T> : ILoggable, IJson<T> where T : IJson<T>, new() {

  #region --- ILoggable --------------------------------------------
  [JsonIgnore]
  public ILogger Logger { get; set; }
  public void SetLogger(ILogger logger) {
    Logger = ALogger.Create(logger);
  }
  #endregion --- ILoggable --------------------------------------------

  

  #region --- ToJson --------------------------------------------
  public virtual string ToJson() {
    return ToJson(new JsonSerializerOptions());
  }

  public abstract string ToJson(JsonSerializerOptions options);
  #endregion --- ToJson --------------------------------------------

  #region --- ParseJson --------------------------------------------
  public T ParseJson(JsonElement source) {
    return ParseJson(source.GetRawText());
  }
  public T ParseJson(JsonElement source, JsonSerializerOptions options) {
    return ParseJson(source.GetRawText(), options);
  }

  public virtual T ParseJson(string source) {
    return ParseJson(source, new JsonSerializerOptions());
  }
  public abstract T ParseJson(string source, JsonSerializerOptions options);
  #endregion --- ParseJson --------------------------------------------

  #region --- Static FromJson --------------------------------------------
  public static T FromJson(JsonElement source) {
    if (source.ValueKind != JsonValueKind.Object) {
      throw new JsonException("Json movie source is not an object");
    }

    return FromJson(source.GetRawText());
  }

  public static T FromJson(JsonElement source, JsonSerializerOptions options) {
    if (source.ValueKind != JsonValueKind.Object) {
      throw new JsonException("Json movie source is not an object");
    }

    return FromJson(source.GetRawText(), options);
  }

  public static T FromJson(string source) {
    T RetVal = new T();
    return RetVal.ParseJson(source);
  }

  public static T FromJson(string source, JsonSerializerOptions options) {
    if (string.IsNullOrWhiteSpace(source)) {
      throw new ArgumentNullException(nameof(source));
    }

    return JsonSerializer.Deserialize<T>(source, options);
  }
  #endregion --- Static FromJson --------------------------------------------

}
