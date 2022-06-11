using static BLTools.Json.JsonConverterResources;

namespace MediaSearch.Models;

public class TAboutJsonConverter : JsonConverter<TAbout>, ILoggable {
  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TAboutJsonConverter>();

  public override bool CanConvert(Type typeToConvert) {
    if (typeToConvert == typeof(TAbout)) { return true; }
    if (typeToConvert == typeof(IAbout)) { return true; }
    return false;
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
          Logger.IfDebugMessageExBox($"Converted {nameof(TAbout)}", RetVal);
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
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
              Logger.IfDebugMessageExBox($"Found {nameof(RetVal.CurrentVersion)}", RetVal.CurrentVersion);
              break;

            case nameof(TAbout.ChangeLog):
              RetVal.ChangeLog = reader.GetString() ?? "";
              Logger.IfDebugMessageExBox($"Found {nameof(RetVal.ChangeLog)}", RetVal.ChangeLog);
              break;

            default:
              Logger.LogWarningBox(ERROR_INVALID_PROPERTY, Property);
              break;
          }
        }
      }

      Logger.IfDebugMessageExBox($"Converted {nameof(TAbout)}", RetVal);
      return RetVal;

    } catch (Exception ex) {
      Logger.LogErrorBox(ERROR_CONVERSION, ex);
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
