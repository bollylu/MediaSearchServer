using static MediaSearch.Models.JsonConverterResources;

namespace MediaSearch.Models;
public class TLanguageDictionaryStringConverter : JsonConverter<TLanguageDictionary<string>>, IMediaSearchLoggable<TLanguageDictionaryStringConverter> {

  public const string PROPERTY_KEY = "key";
  public const string PROPERTY_VALUE = "value";

  public IMediaSearchLogger<TLanguageDictionaryStringConverter> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TLanguageDictionaryStringConverter>();

  public override bool CanConvert(Type typeToConvert) {
    return typeToConvert == typeof(TLanguageDictionary<string>);
  }

  public override TLanguageDictionary<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
    if (reader.TokenType != JsonTokenType.StartArray) {
      throw new JsonException();
    }

    TLanguageDictionary<string> RetVal = new();

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndArray) {
          Logger.IfDebugMessageEx("Converted TLanguageDictionary", RetVal);
          return RetVal;
        }

        if (TokenType == JsonTokenType.StartObject) {

          ELanguage Language = ELanguage.Unknown;
          string Text = "";

          while (reader.Read() && reader.TokenType != JsonTokenType.EndObject) {

            if (reader.TokenType == JsonTokenType.PropertyName) {
              string Property = reader.GetString() ?? "";
              reader.Read();

              switch (Property) {
                case PROPERTY_KEY:
                  Language = JsonSerializer.Deserialize<ELanguage>(ref reader, options);
                  break;

                case PROPERTY_VALUE:
                  Text = reader.GetString() ?? string.Empty;
                  break;

                default:
                  Logger.LogWarningBox(ERROR_INVALID_PROPERTY, Property);
                  break;
              }
            }
          }
          RetVal.Add(Language, Text);
        }
      }

      Logger.IfDebugMessageEx("Converted TLanguageDictionary", RetVal);
      return RetVal;

    } catch (Exception ex) {
      Logger.LogErrorBox(ERROR_CONVERSION, ex);
      throw;
    }
  }

  public override void Write(Utf8JsonWriter writer, TLanguageDictionary<string> value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }
    writer.WriteStartArray();

    foreach (KeyValuePair<ELanguage, string> KvpItem in value) {
      writer.WriteStartObject();
      writer.WriteString(PROPERTY_KEY, KvpItem.Key.ToString());
      writer.WriteString(PROPERTY_VALUE, KvpItem.Value.ToString());
      writer.WriteEndObject();
    }

    writer.WriteEndArray();
  }
}
