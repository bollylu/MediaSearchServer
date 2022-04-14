namespace MediaSearch.Models;
using static MediaSearch.Models.JsonConverterResources;

public class TMovieJsonConverter : JsonConverter<TMovie> {

  public IMediaSearchLogger<TMovieJsonConverter> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMovieJsonConverter>();

  public override bool CanConvert(Type typeToConvert) {
    return typeToConvert == typeof(TMovie) || typeToConvert.UnderlyingSystemType.Name == nameof(IMovie);
  }

  public override TMovie Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    TMovie RetVal = new TMovie();

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndObject) {
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {

            case nameof(TMovie.Id):
              reader.GetString();
              break;

            case nameof(IMovie.Group):
              RetVal.Group = reader.GetString() ?? "";
              break;

            case nameof(IMovie.StoragePath):
              RetVal.StoragePath = reader.GetString() ?? "";
              break;

            case nameof(IMovie.FileName):
              RetVal.FileName = reader.GetString() ?? "";
              break;

            case nameof(IMovie.FileExtension):
              RetVal.FileExtension = reader.GetString() ?? "";
              break;

            case nameof(IMovie.Size):
              RetVal.Size = reader.GetInt64();
              break;

            case nameof(IMovie.CreationYear):
              int Year = reader.GetInt32();
              if (Year > 0) {
                RetVal.CreationDate = new DateOnly(Year, 1, 1);
              }
              break;

            case nameof(IMovie.DateAdded):
              RetVal.DateAdded = JsonSerializer.Deserialize<DateOnly>(ref reader, options);
              break;

            case nameof(IMovie.Titles):
              ILanguageTextInfos? Titles = JsonSerializer.Deserialize<TLanguageTextInfos>(ref reader, options);
              if (Titles is not null) {
                foreach (var TitleItem in Titles.GetAll()) {
                  RetVal.Titles.Add(TitleItem);
                }
              }
              break;

            case nameof(IMovie.Descriptions):
              ILanguageTextInfos? Descriptions = JsonSerializer.Deserialize<TLanguageTextInfos>(ref reader, options);
              if (Descriptions is not null) {
                foreach (var DescriptionItem in Descriptions.GetAll()) {
                  RetVal.Descriptions.Add(DescriptionItem);
                }
              }
              break;

            case nameof(IMovie.Tags):
              while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) {
                RetVal.Tags.Add(reader.GetString() ?? "");
              }
              break;

            case nameof(IMovie.Soundtracks):
              while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) {
                RetVal.Soundtracks.Add(JsonSerializer.Deserialize<ELanguage>(ref reader, options));
              }
              break;

            case nameof(IMovie.Subtitles):
              while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) {
                RetVal.Subtitles.Add(JsonSerializer.Deserialize<ELanguage>(ref reader, options));
              }
              break;

            default:
              Logger.LogWarningBox(ERROR_INVALID_PROPERTY, Property);
              break;
          }
        }
      }

      return RetVal;

    } catch (Exception ex) {
      Logger.LogErrorBox(ERROR_CONVERSION, ex);
      throw;
    }

  }


  public override void Write(Utf8JsonWriter writer, TMovie value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }
    writer.WriteStartObject();

    writer.WriteString(nameof(IMovie.Id), value.Id);
    writer.WriteString(nameof(IMovie.Group), value.Group);
    writer.WriteString(nameof(IMovie.StoragePath), value.StoragePath);
    writer.WriteString(nameof(IMovie.FileName), value.FileName);
    writer.WriteString(nameof(IMovie.FileExtension), value.FileExtension);
    writer.WriteNumber(nameof(IMovie.Size), value.Size);
    writer.WriteNumber(nameof(IMovie.CreationYear), value.CreationYear);
    writer.WritePropertyName(nameof(IMovie.DateAdded));
    JsonSerializer.Serialize(writer, value.DateAdded, options);

    writer.WritePropertyName(nameof(IMovie.Titles));
    JsonSerializer.Serialize(writer, value.Titles, options);

    writer.WritePropertyName(nameof(IMovie.Descriptions));
    JsonSerializer.Serialize(writer, value.Descriptions, options);

    writer.WriteStartArray(nameof(IMovie.Tags));
    foreach (string TagItem in value.Tags) {
      writer.WriteStringValue(TagItem);
    }
    writer.WriteEndArray();

    writer.WriteStartArray(nameof(IMovie.Soundtracks));
    foreach (ELanguage SoundtrackItem in value.Soundtracks) {
      JsonSerializer.Serialize(writer, SoundtrackItem, options);
    }
    writer.WriteEndArray();

    writer.WriteStartArray(nameof(IMovie.Subtitles));
    foreach (ELanguage SubtitleItem in value.Subtitles) {
      JsonSerializer.Serialize(writer, SubtitleItem, options);
    }
    writer.WriteEndArray();

    writer.WriteEndObject();
  }
}
