using BLTools.Text;

namespace MediaSearch.Models;
public class TUserAccountSecretJsonConverter : JsonConverter<TUserAccountSecret>, ILoggable {
  #region --- ILoggable --------------------------------------------
  public ILogger Logger { get; set; }

  public void SetLogger(ILogger logger) {
    if (logger is null) {
      Logger = new TConsoleLogger();
    } else {
      Logger = ALogger.Create(logger);
    }
  }
  #endregion --- ILoggable --------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TUserAccountSecretJsonConverter() {
    Logger = ALogger.Create(GlobalSettings.GlobalLogger);
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override bool CanConvert(Type typeToConvert) {
    return typeof(TUserAccountSecret).IsAssignableFrom(typeToConvert);
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
          Logger.LogDebug(RetVal.ToString().BoxFixedWidth("Converted UserAccountSecret", GlobalSettings.DEBUG_BOX_WIDTH));
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string? Property = reader.GetString();
          reader.Read();

          switch (Property) {

            case nameof(TUserAccountSecret.Name):
              RetVal.Name = reader.GetString() ?? "";
              break;

            case nameof(TUserAccountSecret.Password):
              RetVal.Password = reader.GetString() ?? "";
              break;

            case nameof(TUserAccountSecret.MustChangePassword):
              RetVal.MustChangePassword = reader.GetBoolean();
              break;

            case nameof(TUserAccountSecret.Token):
              RetVal.Token = JsonSerializer.Deserialize<TUserToken>(ref reader, options) ?? TUserToken.ExpiredUserToken;
              break;

            default:
              Logger.LogWarning($"Invalid Json property name : {Property}", GetType().Name);
              break;
          }
        }
      }

      Logger.LogDebug(RetVal.ToString().BoxFixedWidth("Converted UserAccountSecret", GlobalSettings.DEBUG_BOX_WIDTH));
      return RetVal;

    } catch (Exception ex) {
      Logger.LogError($"Problem during Json conversion : {ex.Message}");
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
    writer.WriteString(nameof(TUserAccountSecret.Password), value.Password);
    writer.WriteBoolean(nameof(TUserAccountSecret.MustChangePassword), value.MustChangePassword);
    writer.WritePropertyName(nameof(TUserAccountSecret.Token));
    JsonSerializer.Serialize(writer, value.Token, options);

    writer.WriteEndObject();
  }
}
