namespace MediaSearch.Models;

public class TSchemaJsonConverter : JsonConverter<ISchema>, ILoggable {

  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TSchemaJsonConverter>();

  public override bool CanConvert(Type typeToConvert) {
    if (typeToConvert == typeof(TSchema)) { return true; }
    if (typeToConvert == typeof(ISchema)) { return true; }
    if (typeToConvert.GetInterface(typeof(ISchema).Name) is not null) { return true; }
    return false;
  }

  public override ISchema? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

    // check that first token is ok
    if (reader.TokenType != JsonTokenType.StartArray) {
      throw new JsonException();
    }

    ISchema? RetVal = new TSchema();

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        // check if reading is complete
        if (TokenType == JsonTokenType.EndArray) {
          return RetVal;
        }

        if (TokenType == JsonTokenType.StartObject) {


        }

        // Do we have a new property token ?
        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {

            case nameof(IMSTable.Name):
              TableHeader.Name = reader.GetString() ?? "";
              break;

            case nameof(IMSTableHeader.Description):
              TableHeader.Description = reader.GetString() ?? "";
              break;

            case nameof(IMSTableHeader.LastUpdate):
              TableHeader.LastUpdate = DateTime.Parse(reader.GetString() ?? DateTime.MinValue.ToYMDHMS());
              break;

            case nameof(IMSTableHeader<IMSRecord>.MediaSource):
              IMediaSource? MediaSource = JsonSerializer.Deserialize<IMediaSource>(ref reader, options);
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


  public override void Write(Utf8JsonWriter writer, ISchema value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }

    writer.WriteStartArray();
    foreach (IMSTable TableItem in value.GetAll()) {
      JsonSerializer.Serialize(writer, TableItem.Header, options);
    }
    writer.WriteEndArray();
  }
}
