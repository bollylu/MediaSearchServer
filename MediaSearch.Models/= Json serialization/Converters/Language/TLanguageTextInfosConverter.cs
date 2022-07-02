using static BLTools.Json.JsonConverterResources;

namespace MediaSearch.Models;

public class TLanguageTextInfosJsonConverter : JsonConverter<TLanguageTextInfos>, ILoggable {
  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TLanguageTextInfosJsonConverter>();

  public override bool CanConvert(Type typeToConvert) {
    if (typeToConvert == typeof(TLanguageTextInfos)) {
      return true;
    }
    if (typeToConvert.IsInterface && typeToConvert.Name == nameof(ILanguageTextInfos)) {
      return true;
    }
    return false;
  }

  public override TLanguageTextInfos Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

    if (reader.TokenType != JsonTokenType.StartArray) {
      throw new JsonException();
    }

    TLanguageTextInfos RetVal = new TLanguageTextInfos();

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndArray) {
          Logger.IfDebugMessageExBox($"Converted {nameof(TLanguageTextInfos)}", RetVal);
          return RetVal;
        }

        ILanguageTextInfo? LanguageTextInfo = JsonSerializer.Deserialize<TLanguageTextInfo>(ref reader, options);
        if (LanguageTextInfo is not null) {
          RetVal.Add(LanguageTextInfo);
        }
      }

      Logger.IfDebugMessageExBox($"Converted {nameof(TLanguageTextInfos)}", RetVal);
      return RetVal;

    } catch (Exception ex) {
      Logger.LogErrorBox(ERROR_CONVERSION, ex);
      throw;
    }

  }

  public override void Write(Utf8JsonWriter writer, TLanguageTextInfos value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }

    writer.WriteStartArray();

    foreach (ILanguageTextInfo LanguageTextInfoItem in value.GetAll()) {
      JsonSerializer.Serialize(writer, LanguageTextInfoItem, options);
    }

    writer.WriteEndArray();
  }


}
