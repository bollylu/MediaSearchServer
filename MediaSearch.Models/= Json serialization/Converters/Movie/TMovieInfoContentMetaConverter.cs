using static MediaSearch.Models.JsonConverterResources;

namespace MediaSearch.Models;

public class TMovieInfoContentMetaConverter : JsonConverter<TMovieInfoContentMeta>, IMediaSearchLoggable<TMovieInfoContentMetaConverter> {
  public IMediaSearchLogger<TMovieInfoContentMetaConverter> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMovieInfoContentMetaConverter>();

  public override bool CanConvert(Type typeToConvert) {
    return typeToConvert == typeof(TMovieInfoContentMeta);
  }

  public override TMovieInfoContentMeta Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    TMovieInfoContentMeta RetVal = new TMovieInfoContentMeta();
    
    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndObject && reader.CurrentDepth == 1) {
          Logger.IfDebugMessageExBox($"Converted {nameof(TMovieInfoContentMeta)}", RetVal);
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {

            case nameof(TMovieInfoContentMeta.Size):
              RetVal.Size = reader.GetInt64();
              break;

            case nameof(TMovieInfoContentMeta.CreationYear):
              RetVal.CreationYear = reader.GetInt32();
              break;

            case nameof(TMovieInfoContentMeta.Titles):
              ILanguageTextInfos? Titles = JsonSerializer.Deserialize<TLanguageTextInfos>(ref reader, options);
              if (Titles is not null) {
                foreach (ILanguageTextInfo TitleItem in Titles.GetAll()) {
                  RetVal.Titles.Add(TitleItem);
                }
              }
              break;

            case nameof(TMovieInfoContentMeta.Descriptions):
              ILanguageTextInfos? Descriptions = JsonSerializer.Deserialize<TLanguageTextInfos>(ref reader, options);
              if (Descriptions is not null) {
                foreach (ILanguageTextInfo DescriptionItem in Descriptions.GetAll()) {
                  RetVal.Descriptions.Add(DescriptionItem);
                }
              }
              break;

            case nameof(TMovieInfoContentMeta.Soundtracks):
              IList<ELanguage>? Soundtracks = JsonSerializer.Deserialize<List<ELanguage>>(ref reader, options);
              if (Soundtracks is not null) {
                foreach (ELanguage LanguageItem in Soundtracks) {
                  RetVal.Soundtracks.Add(LanguageItem);
                }
              }
              break;

            case nameof(TMovieInfoContentMeta.Subtitles):
              IList<ELanguage>? Subtitles = JsonSerializer.Deserialize<List<ELanguage>>(ref reader, options);
              if (Subtitles is not null) {
                foreach (ELanguage LanguageItem in Subtitles) {
                  RetVal.Subtitles.Add(LanguageItem);
                }
              }
              break;

            case nameof(TMovieInfoContentMeta.Genres):
              IList<string>? Genres = JsonSerializer.Deserialize<List<string>>(ref reader, options);
              if (Genres is not null) {
                foreach (string GenreItem in Genres) {
                  RetVal.Genres.Add(GenreItem);
                }
              }
              break;

            default:
              Logger.LogWarningBox(ERROR_INVALID_PROPERTY, Property);
              break;
          }
        }
      }

      Logger.IfDebugMessageExBox($"Converted {nameof(TMovieInfoContentMeta)}", RetVal);
      return RetVal;

    } catch (Exception ex) {
      Logger.LogErrorBox(ERROR_CONVERSION, ex);
      throw;
    }
  }

  public override void Write(Utf8JsonWriter writer, TMovieInfoContentMeta value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }

    writer.WriteStartObject();

    writer.WritePropertyName(nameof(TMovieInfoContentMeta.Titles));
    JsonSerializer.Serialize(writer, value.Titles, options);

    writer.WritePropertyName(nameof(TMovieInfoContentMeta.Descriptions));
    JsonSerializer.Serialize(writer, value.Descriptions, options);

    writer.WriteNumber(nameof(TMovieInfoContentMeta.Size), value.Size);
    writer.WriteNumber(nameof(TMovieInfoContentMeta.CreationYear), value.CreationYear);

    writer.WritePropertyName(nameof(TMovieInfoContentMeta.Soundtracks));
    JsonSerializer.Serialize(writer, value.Soundtracks, options);

    writer.WritePropertyName(nameof(TMovieInfoContentMeta.Subtitles));
    JsonSerializer.Serialize(writer, value.Subtitles, options);

    writer.WritePropertyName(nameof(TMovieInfoContentMeta.Genres));
    JsonSerializer.Serialize(writer, value.Genres, options);

    writer.WriteEndObject();
  }
}
