using System.Net;

using static MediaSearch.Models.JsonConverterResources;

namespace MediaSearch.Models;
public class TUserAccountInfoJsonConverter : JsonConverter<TUserAccountInfo>, ILoggable {
  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TUserAccountInfoJsonConverter>();

  public override bool CanConvert(Type typeToConvert) {
    return typeToConvert == typeof(TUserAccountInfo);
  }

  public override TUserAccountInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    TUserAccountInfo RetVal = new();

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndObject) {
          Logger.LogDebugExBox("Converted UserAccountInfo", RetVal);
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {

            case nameof(TUserAccountInfo.Name):
              RetVal.Name = reader.GetString() ?? "";
              break;

            case nameof(TUserAccountInfo.Description):
              RetVal.Description = reader.GetString() ?? "";
              break;

            case nameof(TUserAccountInfo.RemoteIp):
              IPAddress? RemoteIp = JsonSerializer.Deserialize<IPAddress>(ref reader, options);
              if (RemoteIp is not null) {
                RetVal.RemoteIp = RemoteIp;
              }
              break;

            case nameof(TUserAccountInfo.LastSuccessfulLogin):
              RetVal.LastSuccessfulLogin = DateTime.Parse(reader.GetString() ?? DateTime.MinValue.ToYMDHMS()).FromUTC();
              break;

            case nameof(TUserAccountInfo.LastFailedLogin):
              RetVal.LastFailedLogin = DateTime.Parse(reader.GetString() ?? DateTime.MinValue.ToYMDHMS()).FromUTC();
              break;

            default:
              Logger.LogWarningBox(ERROR_INVALID_PROPERTY, Property);
              break;
          }
        }
      }

      Logger.LogDebugExBox("Converted UserAccountInfo", RetVal);
      return RetVal;

    } catch (Exception ex) {
      Logger.LogErrorBox(ERROR_CONVERSION, ex);
      throw;
    }
  }

  public override void Write(Utf8JsonWriter writer, TUserAccountInfo value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }
    writer.WriteStartObject();

    writer.WriteString(nameof(TUserAccountInfo.Name), value.Name);
    writer.WriteString(nameof(TUserAccountInfo.Description), value.Description);
    writer.WritePropertyName(nameof(TUserAccountInfo.RemoteIp));
    JsonSerializer.Serialize(writer, value.RemoteIp, options);
    writer.WriteString(nameof(TUserAccountInfo.LastSuccessfulLogin), value.LastSuccessfulLogin.ToUniversalTime().ToYMDHMS());
    writer.WriteString(nameof(TUserAccountInfo.LastFailedLogin), value.LastFailedLogin.ToUniversalTime().ToYMDHMS());

    writer.WriteEndObject();
  }
}
