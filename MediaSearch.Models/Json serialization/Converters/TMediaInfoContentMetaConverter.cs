using static MediaSearch.Models.JsonConverterResources;

namespace MediaSearch.Models;
public class TMediaInfoContentMetaConverter : JsonConverter<TMediaInfoContentMeta>, IMediaSearchLoggable<TMediaInfoContentMetaConverter> {
  public IMediaSearchLogger<TMediaInfoContentMetaConverter> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMediaInfoContentMetaConverter>();

  public override bool CanConvert(Type typeToConvert) {
    return typeToConvert == typeof(TMediaInfoContentMeta);
  }

  public override TMediaInfoContentMeta Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    TMediaInfoContentMeta RetVal = new TMediaInfoContentMeta();

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndObject && reader.CurrentDepth == 1) {
          Logger.IfDebugMessageEx("Converted TMediaInfoContentMeta", RetVal);
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {

            case nameof(TMediaInfoContentMeta.Size):
              RetVal.Size = reader.GetInt32();
              break;

            case nameof(TMediaInfoContentMeta.Titles):
              TLanguageDictionary<string>? Titles = JsonSerializer.Deserialize<TLanguageDictionary<string>>(ref reader, options);
              if (Titles is not null) {
                foreach (KeyValuePair<ELanguage, string> TitleItem in Titles) {
                  RetVal.Titles.Add(TitleItem.Key, TitleItem.Value);
                }
              }
              break;

            case nameof(TMediaInfoContentMeta.Descriptions):
              TLanguageDictionary<string>? Descriptions = JsonSerializer.Deserialize<TLanguageDictionary<string>>(ref reader, options);
              if (Descriptions is not null) {
                foreach (KeyValuePair<ELanguage, string> DescriptionItem in Descriptions) {
                  RetVal.Descriptions.Add(DescriptionItem.Key, DescriptionItem.Value);
                }
              }
              break;

            default:
              Logger.LogWarningBox(ERROR_INVALID_PROPERTY, Property);
              break;
          }
        }
      }

      Logger.IfDebugMessageEx("Converted TMediaInfoContentMeta", RetVal);
      return RetVal;

    } catch (Exception ex) {
      Logger.LogErrorBox(ERROR_CONVERSION, ex);
      throw;
    }
  }

  public override void Write(Utf8JsonWriter writer, TMediaInfoContentMeta value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }
    writer.WriteStartObject();

    writer.WritePropertyName(nameof(TMediaInfoContentMeta.Titles));
    JsonSerializer.Serialize(writer, value.Titles, options);

    writer.WritePropertyName(nameof(TMediaInfoContentMeta.Descriptions));
    JsonSerializer.Serialize(writer, value.Descriptions, options);

    writer.WriteNumber(nameof(TMediaInfoContentMeta.Size), value.Size);

    writer.WriteEndObject();
  }
}
