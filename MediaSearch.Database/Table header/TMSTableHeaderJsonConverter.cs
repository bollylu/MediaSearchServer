namespace MediaSearch.Models;
using MediaSearch.Database;

using static BLTools.Json.JsonConverterResources;

public class TMSTableHeaderJsonConverter : JsonConverter<IMSTableHeader>, ILoggable {

  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TMSTableHeaderJsonConverter>();

  public override bool CanConvert(Type typeToConvert) {
    if (typeToConvert == typeof(TMSTableHeader)) { return true; }
    if (typeToConvert == typeof(IMSTableHeader)) { return true; }
    if (typeToConvert.IsAssignableFrom(typeof(IMSTableHeader))) { return true; }
    return false;
  }

  public override IMSTableHeader? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

    // check that first token is ok
    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    IMSTableHeader TableHeader = new TMSTableHeader();

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        // check if reading is complete
        if (TokenType == JsonTokenType.EndObject) {
          Type? FinalType = MediaSearch.Models.GlobalSettings.GetType(TableHeader.MediaSource?.MediaType?.Name ?? "");
          if (FinalType is null) {
            throw new JsonConverterInvalidDataException("MediaType", TableHeader);
          }
          var RetVal = TMSTableHeader.Create(TableHeader.Name, FinalType);
          return RetVal;
        }

        // Do we have a new property token ?
        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {

            case nameof(IMSTableHeader.Name):
              TableHeader.Name = reader.GetString() ?? "";
              break;

            case nameof(IMSTableHeader.Description):
              TableHeader.Description = reader.GetString() ?? "";
              break;

            case nameof(IMSTableHeader.LastUpdate):
              TableHeader.LastUpdate = DateTime.Parse(reader.GetString() ?? DateTime.MinValue.ToYMDHMS());
              break;

            case nameof(IMSTableHeader<IMSRecord>.MediaSource):
              AMediaSource<IMSRecord>? MediaSource = JsonSerializer.Deserialize<AMediaSource<IMSRecord>>(ref reader, options);
              if (MediaSource is not null) {
                TableHeader.SetMediaSource(MediaSource);
              }
              break;

            default:
              Logger.LogWarningBox(ERROR_INVALID_PROPERTY, Property);
              break;
          }
        }
      }

      Logger.LogErrorBox(ERROR_CONVERSION, TableHeader);
      return null;

    } catch (Exception ex) {
      Logger.LogErrorBox(ERROR_CONVERSION, ex);
      throw;
    }

  }


  public override void Write(Utf8JsonWriter writer, IMSTableHeader value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }

    writer.WriteStartObject();
    writer.WriteString(nameof(IMSTableHeader.Name), value.Name);
    writer.WriteString(nameof(IMSTableHeader.Description), value.Description);
    writer.WriteString(nameof(IMSTableHeader.LastUpdate), value.LastUpdate.ToYMDHMS());

    writer.WritePropertyName(nameof(IMSTableHeader<IMSRecord>.MediaSource));
    JsonSerializer.Serialize(writer, value.MediaSource, options);

    writer.WriteEndObject();
  }
}
