using BLTools.Text;

namespace MediaSearch.Models;
public class TMediaInfoContentMetaConverter : JsonConverter<TMediaInfoContentMeta>, IMediaSearchLoggable<TMediaInfoContentMetaConverter> {
  public IMediaSearchLogger<TMediaInfoContentMetaConverter> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMediaInfoContentMetaConverter>();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaInfoContentMetaConverter() { }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override bool CanConvert(Type typeToConvert) {
    return typeof(TMediaInfoContentMeta).IsAssignableFrom(typeToConvert);
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
          Logger.LogDebugExBox("Converted TMediaInfoContentMeta", RetVal);
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string? Property = reader.GetString();
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
              Logger.LogWarning($"Invalid Json property name : {Property}", GetType().Name);
              break;
          }
        }
      }

      Logger.LogDebugExBox("Converted TMediaInfoContentMeta", RetVal);
      return RetVal;

    } catch (Exception ex) {
      Logger.LogErrorBox("Problem during Json conversion", ex);
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
