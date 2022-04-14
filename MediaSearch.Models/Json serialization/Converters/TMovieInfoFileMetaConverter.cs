using static MediaSearch.Models.JsonConverterResources;

namespace MediaSearch.Models;

public class TMovieInfoFileMetaConverter : JsonConverter<TMovieInfoFileMeta>, IMediaSearchLoggable<TMovieInfoFileMetaConverter> {
  public IMediaSearchLogger<TMovieInfoFileMetaConverter> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMovieInfoFileMetaConverter>();

  public override bool CanConvert(Type typeToConvert) {
    return typeToConvert == typeof(TMovieInfoFileMeta);
  }

  public override TMovieInfoFileMeta Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    TMovieInfoFileMeta RetVal = new();
    
    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndObject) {
          Logger.IfDebugMessageEx($"Converted {nameof(TMovieInfoFileMeta)}", RetVal);
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {

            case nameof(TMovieInfoFileMeta.Header):
              TMediaInfoHeader? Header = JsonSerializer.Deserialize<TMediaInfoHeader>(ref reader, options);
              if (Header is not null) {
                RetVal.Header = Header;
              }
              break;

            case nameof(TMovieInfoFileMeta.Content):
              TMovie? Content = JsonSerializer.Deserialize<TMovie>(ref reader, options);
              if (Content is not null) {
                RetVal.Content = Content;
              }
              break;

            default:
              Logger.LogWarningBox(ERROR_INVALID_PROPERTY, Property);
              break;
          }
        }
      }

      Logger.IfDebugMessageEx($"Converted {nameof(TMovieInfoFileMeta)}", RetVal);
      return RetVal;

    } catch (Exception ex) {
      Logger.LogErrorBox(ERROR_CONVERSION, ex);
      throw;
    }
  }

  public override void Write(Utf8JsonWriter writer, TMovieInfoFileMeta value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }
    writer.WriteStartObject();

    writer.WritePropertyName(nameof(TMovieInfoFileMeta.Header));
    JsonSerializer.Serialize(writer, value.Header, options);

    writer.WritePropertyName(nameof(TMovieInfoFileMeta.Content));
    JsonSerializer.Serialize(writer, value.MetaContent, options);

    writer.WriteEndObject();
  }
}
