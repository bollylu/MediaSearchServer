using BLTools.Text;

namespace MediaSearch.Models;
public class TLanguageDictionaryStringConverter : JsonConverter<TLanguageDictionary<string>>, IMediaSearchLoggable<TLanguageDictionaryStringConverter> {
  public IMediaSearchLogger<TLanguageDictionaryStringConverter> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TLanguageDictionaryStringConverter>();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TLanguageDictionaryStringConverter() { }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override bool CanConvert(Type typeToConvert) {
    return typeof(ILanguageDictionary).IsAssignableFrom(typeToConvert);
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
          Logger.LogDebugExBox("Converted TLanguageDictionary", RetVal);
          return RetVal;
        }

        if (TokenType == JsonTokenType.StartObject) {

          ELanguage Language = ELanguage.Unknown;
          string Text = "";

          while (reader.Read() && reader.TokenType != JsonTokenType.EndObject) {

            if (reader.TokenType == JsonTokenType.PropertyName) {
              string? Property = reader.GetString();
              reader.Read();

              switch (Property) {
                case "key":
                  Language = JsonSerializer.Deserialize<ELanguage>(ref reader, options);
                  break;

                case "value":
                  Text = reader.GetString() ?? string.Empty;
                  break;
              }
            }
          }
          RetVal.Add(Language, Text);
        }
      }

      Logger.LogDebugExBox("Converted TLanguageDictionary", RetVal);
      return RetVal;

    } catch (Exception ex) {
      Logger.LogErrorBox($"Problem during Json conversion", ex);
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
      writer.WriteString("key", KvpItem.Key.ToString());
      writer.WriteString("value", KvpItem.Value.ToString());
      writer.WriteEndObject();
    }

    writer.WriteEndArray();
  }
}
