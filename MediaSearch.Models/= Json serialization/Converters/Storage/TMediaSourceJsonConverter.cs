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

    IMediaSource MediaSource = new TMediaSource();

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndObject) {
          Logger.IfDebugMessageExBox($"Converted {nameof(IMediaSource)}", MediaSource);
          if (MediaSource is null) {
            Logger.LogErrorBox(ERROR_CONVERSION, "(null)");
            return null;
          }
          Type? FinalType = MediaSearch.Models.GlobalSettings.GetType(MediaSource.MediaType?.Name ?? "");
          if (FinalType is null) {
            throw new JsonConverterInvalidDataException("MediaType", MediaSource);
          }
          var RetVal = TMediaSource.Create(MediaSource.RootStorage, FinalType);
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {

            case nameof(IMediaSourceGeneric.MediaType):
              string TypeOfMedia = reader.GetString() ?? "";
              MediaSource.MediaType = Type.GetType(TypeOfMedia);
              break;

            case nameof(IMediaSourceGeneric.RootStorage):
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

    writer.WriteString(nameof(TMediaSource.MediaType), value.MediaType?.Name ?? "");
    writer.WriteString(nameof(TMediaSource.RootStorage), value.RootStorage);

    writer.WriteEndObject();
  }


}
