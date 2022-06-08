using static BLTools.Json.JsonConverterResources;

namespace MediaSearch.Models;
public class TUserTokenJsonConverter : JsonConverter<TUserToken>, ILoggable {
  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TUserTokenJsonConverter>();

  public override bool CanConvert(Type typeToConvert) {
    return typeToConvert == typeof(TUserToken);
  }

  public override TUserToken Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    TUserToken RetVal = new();

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndObject) {
          Logger.IfDebugMessageExBox($"Converted {nameof(TUserToken)}", RetVal);
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {

            case nameof(TUserToken.TokenId):
              RetVal.TokenId = reader.GetString() ?? "";
              break;

            case nameof(TUserToken.Expiration):
              RetVal.Expiration = DateTime.Parse(reader.GetString() ?? DateTime.MinValue.ToYMDHMS());
              break;

            default:
              Logger.LogWarningBox(ERROR_INVALID_PROPERTY, Property);
              break;
          }
        }
      }

      Logger.IfDebugMessageExBox("Converted UserToken", RetVal);
      return RetVal;

    } catch (Exception ex) {
      Logger.LogErrorBox(ERROR_CONVERSION, ex);
      throw;
    }
  }

  public override void Write(Utf8JsonWriter writer, TUserToken value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }
    writer.WriteStartObject();

    writer.WriteString(nameof(TUserToken.TokenId), value.TokenId);
    writer.WriteString(nameof(TUserToken.Expiration), value.Expiration.ToYMDHMS());

    writer.WriteEndObject();
  }
}
