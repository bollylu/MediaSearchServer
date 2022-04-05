using static MediaSearch.Models.JsonConverterResources;

namespace MediaSearch.Models;

public class TMediaInfoHeaderConverter : JsonConverter<TMediaInfoHeader>, IMediaSearchLoggable<TMediaInfoHeaderConverter> {
  public IMediaSearchLogger<TMediaInfoHeaderConverter> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMediaInfoHeaderConverter>();

  public override bool CanConvert(Type typeToConvert) {
    return typeToConvert == typeof(TMediaInfoHeader) || typeToConvert.UnderlyingSystemType.Name == nameof(IMediaInfoHeader);
  }

  public override TMediaInfoHeader Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    TMediaInfoHeader RetVal = new TMediaInfoHeader();
    
    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndObject) {
          Logger.IfDebugMessageEx($"Converted {nameof(TMediaInfoHeader)}", RetVal);
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {

            case nameof(TMediaInfoHeader.Name):
              RetVal.Name = reader.GetString() ?? "";
              break;

            case nameof(TMediaInfoHeader.Description):
              RetVal.Description = reader.GetString() ?? "";
              break;

            default:
              Logger.LogWarningBox(ERROR_INVALID_PROPERTY, Property);
              break;
          }
        }
      }

      Logger.IfDebugMessageEx($"Converted {nameof(TMediaInfoHeader)}", RetVal);
      return RetVal;

    } catch (Exception ex) {
      Logger.LogErrorBox(ERROR_CONVERSION, ex);
      throw;
    }
  }

  public override void Write(Utf8JsonWriter writer, TMediaInfoHeader value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }
    writer.WriteStartObject();

    writer.WriteString(nameof(TMediaInfoHeader.Name), value.Name);
    writer.WriteString(nameof(TMediaInfoHeader.Description), value.Description);

    writer.WriteEndObject();
  }
}
