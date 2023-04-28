using static MediaSearch.Models.JsonConverterResources;

namespace MediaSearch.Models;
public class TUserAccountSecretJsonConverter : JsonConverter<TUserAccountSecret>, ILoggable {
  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TUserAccountSecretJsonConverter>();

  public const string SecurityKey = "bla bla bla";

  public override bool CanConvert(Type typeToConvert) {
    return typeToConvert == typeof(TUserAccountSecret);
  }

  public override TUserAccountSecret Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    TUserAccountSecret RetVal = new();

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndObject) {
          Logger.LogDebugExBox("Converted UserAccountSecret", RetVal);
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {

            case nameof(TUserAccountSecret.Name):
              RetVal.Name = reader.GetString() ?? "";
              break;

            case nameof(TUserAccountSecret.PasswordHash):
              RetVal.PasswordHash = DecryptPassword(reader.GetString() ?? "");
              break;

            case nameof(TUserAccountSecret.MustChangePassword):
              RetVal.MustChangePassword = reader.GetBoolean();
              break;

            case nameof(TUserAccountSecret.Token):
              RetVal.Token = JsonSerializer.Deserialize<TUserToken>(ref reader, options) ?? TUserToken.ExpiredUserToken;
              break;

            default:
              Logger.LogWarningBox(ERROR_INVALID_PROPERTY, Property);
              break;
          }
        }
      }

      Logger.LogDebugExBox("Converted UserAccountSecret", RetVal);
      return RetVal;

    } catch (Exception ex) {
      Logger.LogErrorBox(ERROR_CONVERSION, ex);
      throw;
    }
  }

  public override void Write(Utf8JsonWriter writer, TUserAccountSecret value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }
    writer.WriteStartObject();

    writer.WriteString(nameof(TUserAccountSecret.Name), value.Name);
    writer.WriteString(nameof(TUserAccountSecret.PasswordHash), EncryptPassword(value.PasswordHash));
    writer.WriteBoolean(nameof(TUserAccountSecret.MustChangePassword), value.MustChangePassword);
    writer.WritePropertyName(nameof(TUserAccountSecret.Token));
    JsonSerializer.Serialize(writer, value.Token, options);

    writer.WriteEndObject();
  }

  private string EncryptPassword(string source) {
    return Convert.ToBase64String(Encoding.UTF8.GetBytes(source));
  }
  private string DecryptPassword(string source) {
    return Encoding.UTF8.GetString(Convert.FromBase64String(source));
  }
}
