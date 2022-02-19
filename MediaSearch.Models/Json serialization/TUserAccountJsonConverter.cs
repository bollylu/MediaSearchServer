using BLTools.Text;

namespace MediaSearch.Models;
public class TUserAccountJsonConverter : JsonConverter<TUserAccount>, ILoggable {
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
  public TUserAccountJsonConverter() {
    Logger = ALogger.Create(GlobalSettings.GlobalLogger);
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override bool CanConvert(Type typeToConvert) {
    return typeof(TUserAccount).IsAssignableFrom(typeToConvert);
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
          Logger.LogDebug(RetVal.ToString().BoxFixedWidth("Converted UserAccountSecret", GlobalSettings.DEBUG_BOX_WIDTH));
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string? Property = reader.GetString();
          reader.Read();

          switch (Property) {

            case nameof(TUserAccount.Name):
              RetVal.Name = reader.GetString() ?? "";
              break;

            case nameof(TUserAccount.Description):
              RetVal.Description = reader.GetString() ?? "";
              break;

            case nameof(TUserAccount.Password):
              RetVal.Password = reader.GetString() ?? "";
              break;

            case nameof(TUserAccount.MustChangePassword):
              RetVal.MustChangePassword = reader.GetBoolean();
              break;

            case nameof(TUserAccount.Token):
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

  public override void Write(Utf8JsonWriter writer, TUserAccount value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }
    writer.WriteStartObject();

    writer.WriteString(nameof(TUserAccount.Name), value.Name);
    writer.WriteString(nameof(TUserAccount.Description), value.Description);
    writer.WriteString(nameof(TUserAccount.Password), value.Password);
    writer.WriteBoolean(nameof(TUserAccount.MustChangePassword), value.MustChangePassword);
    writer.WritePropertyName(nameof(TUserAccount.Token));
    JsonSerializer.Serialize(writer, value.Token, options);

    writer.WriteEndObject();
  }
}
