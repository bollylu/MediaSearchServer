namespace MediaSearch.Models;
using MediaSearch.Database;

using static MediaSearch.Models.JsonConverterResources;

public class TMSTableHeaderJsonConverter : JsonConverter<TMSTableHeader> {

  public IMediaSearchLogger<TMSTableHeaderJsonConverter> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMSTableHeaderJsonConverter>();

  public override bool CanConvert(Type typeToConvert) {
    return typeToConvert == typeof(TMSTableHeader) || typeToConvert.GetInterface(nameof(IMSTableHeader)) is not null;
  }

  public override TMSTableHeader Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    TMSTableHeader RetVal = new TMSTableHeader();

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

            case nameof(IMSTableHeader.Name):
              RetVal.Name = reader.GetString() ?? "";
              break;

            case nameof(IMSTableHeader.Description):
              RetVal.Description = reader.GetString() ?? "";
              break;

            case nameof(IMSTableHeader.LastUpdate):
              RetVal.LastUpdate = DateTime.Parse(reader.GetString() ?? DateTime.MinValue.ToYMDHMS());
              break;

            case nameof(IMSTableHeader.MediaSource):
              TMediaSource? MediaSource = JsonSerializer.Deserialize<TMediaSource>(ref reader, options);
              if (MediaSource is not null) {
                RetVal.SetMediaSource(MediaSource);
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


  public override void Write(Utf8JsonWriter writer, TMSTableHeader value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }

    writer.WriteStartObject();
    writer.WriteString(nameof(IMSTableHeader.Name), value.Name);
    writer.WriteString(nameof(IMSTableHeader.Description), value.Description);
    writer.WriteString(nameof(IMSTableHeader.LastUpdate), value.LastUpdate.ToYMDHMS());

    writer.WritePropertyName(nameof(IMSTableHeader.MediaSource));
    JsonSerializer.Serialize(writer, value.MediaSource, options);

    writer.WriteEndObject();
  }
}
