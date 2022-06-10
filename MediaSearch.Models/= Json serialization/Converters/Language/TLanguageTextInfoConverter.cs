using static BLTools.Json.JsonConverterResources;

namespace MediaSearch.Models;

public class TLanguageTextInfoConverter : JsonConverter<TLanguageTextInfo>, ILoggable {
  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TLanguageTextInfoConverter>();

  public override bool CanConvert(Type typeToConvert) {
    if (typeToConvert == typeof(TLanguageTextInfo)) {
      return true;
    }
    if (typeToConvert.IsInterface && typeToConvert.Name == nameof(ILanguageTextInfo)) {
      return true;
    }
    return false;
  }

  public override TLanguageTextInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }
    bool HasContent = false;
    TLanguageTextInfo RetVal = new TLanguageTextInfo();

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndObject) {
          if (HasContent) {
            Logger.IfDebugMessageExBox($"Converted {nameof(TLanguageTextInfo)}", RetVal);
            return RetVal;
          } else {
            return null;
          }
        }

        if (TokenType == JsonTokenType.PropertyName) {

          HasContent = true;
          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {

            case nameof(TLanguageTextInfo.Language):
              RetVal.Language = JsonSerializer.Deserialize<ELanguage>(ref reader, options);
              break;

            case nameof(TLanguageTextInfo.Value):
              RetVal.Value = reader.GetString() ?? "";
              break;

            case nameof(TLanguageTextInfo.IsPrincipal):
              RetVal.IsPrincipal = reader.GetBoolean();
              break;
            default:
              Logger.LogWarningBox(ERROR_INVALID_PROPERTY, Property);
              break;
          }
        }
      }

      Logger.IfDebugMessageExBox($"Converted {nameof(TLanguageTextInfo)}", RetVal);
      return RetVal;

    } catch (Exception ex) {
      Logger.LogErrorBox(ERROR_CONVERSION, ex);
      throw;
    }

  }

  public override void Write(Utf8JsonWriter writer, TLanguageTextInfo value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }
    writer.WriteStartObject();

    writer.WritePropertyName(nameof(value.Language));
    JsonSerializer.Serialize(writer, value.Language, options);
    writer.WriteString(nameof(value.Value), value.Value);
    writer.WriteBoolean(nameof(value.IsPrincipal), value.IsPrincipal);

    writer.WriteEndObject();
  }


}
