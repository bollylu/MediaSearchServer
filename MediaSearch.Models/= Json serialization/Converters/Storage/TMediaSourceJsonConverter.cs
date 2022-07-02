using static BLTools.Json.JsonConverterResources;

namespace MediaSearch.Models;

public class TMediaSourceJsonConverter : JsonConverter<IMediaSource>, ILoggable {
  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TMediaSourceJsonConverter>();

  public override bool CanConvert(Type typeToConvert) {
    if (typeToConvert == typeof(TMediaSource)) { return true; }
    if (typeToConvert == typeof(IMediaSource)) { return true; }
    return false;
  }

  public override IMediaSource? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

    // check that first token is ok
    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    #region --- Identify the kind of IMediaSource by finding the MediaType token --------------------------------------------
    Utf8JsonReader SecondReader = reader;
    string? MediaTypeValue = null;

    while (SecondReader.Read()) {
      if (SecondReader.TokenType == JsonTokenType.EndObject) {
        break;
      }

      if (SecondReader.TokenType == JsonTokenType.PropertyName) {
        string? PropertyName = SecondReader.GetString();
        if (string.IsNullOrEmpty(PropertyName)) {
          continue;
        }
        if (PropertyName == nameof(IMediaSource.MediaType)) {
          SecondReader.Read();
          MediaTypeValue = SecondReader.GetString();
          break;
        }
      }
    }
    if (MediaTypeValue is null) {
      Logger.LogError("Unable to identify IMediaSource from null value");
      return null;
    }

    #endregion --- Identify the kind of IMediaSource by finding the MediaType token --------------------------------------------
    IMediaSource? MediaSource;

    try {
      MediaSource = TMediaSource.Create(MediaTypeValue);
      if (MediaSource is null) {
        throw new JsonConverterInvalidDataException(nameof(IMediaSource.MediaType), MediaTypeValue);
      }
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to identify IMediaSource from {MediaTypeValue.WithQuotes()}", ex);
      throw new JsonConverterInvalidDataException(nameof(IMediaSource.MediaType), MediaTypeValue, ex);
    }

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndObject) {
          Logger.IfDebugMessageExBox($"Converted {nameof(IMediaSource)}", MediaSource);
          return MediaSource;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {

            case nameof(IMediaSource.MediaType):
              string TypeOfMedia = reader.GetString() ?? "";
              break;

            case nameof(IMediaSource.RootStorage):
              MediaSource.RootStorage = reader.GetString() ?? "";
              break;

            default:
              Logger.LogWarningBox(ERROR_INVALID_PROPERTY, Property);
              break;
          }
        }
      }

      Logger.LogErrorBox(ERROR_CONVERSION, MediaSource);
      return null;

    } catch (Exception ex) {
      Logger.LogErrorBox(ERROR_CONVERSION, ex);
      throw;
    }

  }

  public override void Write(Utf8JsonWriter writer, IMediaSource value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }

    writer.WriteStartObject();

    writer.WriteString(nameof(IMediaSource.MediaType), value.MediaType?.Name ?? "");
    writer.WriteString(nameof(IMediaSource.RootStorage), value.RootStorage);

    writer.WriteEndObject();
  }


}
