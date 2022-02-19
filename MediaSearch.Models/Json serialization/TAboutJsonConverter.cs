using BLTools.Text;

namespace MediaSearch.Models;

public class TAboutJsonConverter : JsonConverter<TAbout>, ILoggable {
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
  public TAboutJsonConverter() {
    Logger = ALogger.Create(GlobalSettings.GlobalLogger);
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override bool CanConvert(Type typeToConvert) {
    return typeof(TAbout).IsAssignableFrom(typeToConvert);
  }

  public override TAbout Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    TAbout RetVal = new TAbout();

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndObject) {
          Logger.LogDebug(RetVal.ToString().BoxFixedWidth("Converted about", GlobalSettings.DEBUG_BOX_WIDTH));
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string? Property = reader.GetString();
          reader.Read();

          switch (Property) {

            case nameof(TAbout.Name):
              RetVal.Name = reader.GetString() ?? "";
              break;

            case nameof(TAbout.Description):
              RetVal.Description = reader.GetString() ?? "";
              break;

            case nameof(TAbout.CurrentVersion):
              RetVal.CurrentVersion = Version.Parse(reader.GetString() ?? "0.0");
              Logger.LogDebugEx($"Found {nameof(RetVal.CurrentVersion)} = {RetVal.CurrentVersion}", GetType().Name);
              break;

            case nameof(TAbout.ChangeLog):
              RetVal.ChangeLog = reader.GetString() ?? "";
              Logger.LogDebugEx($"Found {nameof(RetVal.ChangeLog)} = {RetVal.ChangeLog}", GetType().Name);
              break;

            default:
              Logger.LogWarning($"Invalid Json property name : {Property}", GetType().Name);
              break;
          }
        }
      }

      Logger.LogDebug(RetVal.ToString().BoxFixedWidth("Converted about", GlobalSettings.DEBUG_BOX_WIDTH));
      return RetVal;

    } catch (Exception ex) {
      Logger.LogError($"Problem during Json conversion : {ex.Message}");
      throw;
    }

  }

  public override void Write(Utf8JsonWriter writer, TAbout value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }
    writer.WriteStartObject();

    writer.WriteString(nameof(TAbout.Name), value.Name);
    writer.WriteString(nameof(TAbout.Description), value.Description);
    writer.WriteString(nameof(TAbout.CurrentVersion), value.CurrentVersion.ToString());
    writer.WriteString(nameof(TAbout.ChangeLog), value.ChangeLog);

    writer.WriteEndObject();
  }


}
