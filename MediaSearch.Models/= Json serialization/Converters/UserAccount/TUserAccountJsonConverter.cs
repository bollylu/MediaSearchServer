using System.Net;

using static BLTools.Json.JsonConverterResources;

namespace MediaSearch.Models;
public class TUserAccountJsonConverter : JsonConverter<TUserAccount>, ILoggable {
  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TUserAccountJsonConverter>();

  public override bool CanConvert(Type typeToConvert) {
    return typeToConvert == typeof(TUserAccount) || typeToConvert.GetInterface(nameof(IUserAccount)) is not null;
  }

  public override TUserAccount Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    TUserAccount RetVal = new();

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndObject) {
          Logger.IfDebugMessageExBox("Converted UserAccount", RetVal);
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {

            case nameof(TUserAccount.Name):
              RetVal.Name = reader.GetString() ?? "";
              break;

            case nameof(TUserAccount.Description):
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

            case nameof(TUserAccount.Secret):
              IUserAccountSecret Secret = JsonSerializer.Deserialize<TUserAccountSecret>(ref reader, options) ?? new TUserAccountSecret();
              RetVal.Secret.Duplicate(Secret);
              break;

            default:
              Logger.LogWarning(ERROR_INVALID_PROPERTY, Property);
              break;
          }
        }
      }

      Logger.IfDebugMessageExBox("Converted UserAccount", RetVal);
      return RetVal;

    } catch (Exception ex) {
      Logger.LogErrorBox(ERROR_CONVERSION, ex);
      throw;
    }
  }

  public override void Write(Utf8JsonWriter writer, TUserAccount value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }
    writer.WriteStartObject();

    writer.WriteString(nameof(TUserAccount.Name), value.Name);
    writer.WriteString(nameof(TUserAccount.Description), value.Description);
    writer.WritePropertyName(nameof(TUserAccountInfo.RemoteIp));
    JsonSerializer.Serialize(writer, value.RemoteIp, options);
    writer.WriteString(nameof(TUserAccountInfo.LastSuccessfulLogin), value.LastSuccessfulLogin.ToUniversalTime().ToDMYHMS());
    writer.WriteString(nameof(TUserAccountInfo.LastFailedLogin), value.LastFailedLogin.ToUniversalTime().ToDMYHMS());
    writer.WritePropertyName(nameof(TUserAccount.Secret));
    JsonSerializer.Serialize(writer, value.Secret, options);

    writer.WriteEndObject();
  }
}
