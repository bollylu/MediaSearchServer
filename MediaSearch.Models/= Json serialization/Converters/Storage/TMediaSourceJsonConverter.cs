using static BLTools.Json.JsonConverterResources;

namespace MediaSearch.Models;

public class TMediaSourceJsonConverter : JsonConverter<TMediaSource>, ILoggable {
  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TMediaSourceJsonConverter>();

  public override bool CanConvert(Type typeToConvert) {
    if (typeToConvert == typeof(TMediaSource)) { return true; }
    if (typeToConvert == typeof(IMediaSource)) { return true; }
    return false;
  }

  public override TMediaSource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    TMediaSource RetVal = new TMediaSource();

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndObject) {
          Logger.IfDebugMessageExBox($"Converted {nameof(TMediaSource)}", RetVal);
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {

            case nameof(TMediaSource.MediaType):
              reader.GetString();
              break;

            case nameof(TMediaSource.RootStorage):
              RetVal.RootStorage = reader.GetString() ?? "";
              break;

            default:
              Logger.LogWarningBox(ERROR_INVALID_PROPERTY, Property);
              break;
          }
        }
      }

      Logger.IfDebugMessageExBox($"Converted {nameof(TMediaSource)}", RetVal);
      return RetVal;

    } catch (Exception ex) {
      Logger.LogErrorBox(ERROR_CONVERSION, ex);
      throw;
    }

  }

  public override void Write(Utf8JsonWriter writer, TMediaSource value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }
    writer.WriteStartObject();

    writer.WriteString(nameof(TMediaSource.MediaType), value.MediaType?.Name ?? "");
    writer.WriteString(nameof(TMediaSource.RootStorage), value.RootStorage);

    writer.WriteEndObject();
  }


}
