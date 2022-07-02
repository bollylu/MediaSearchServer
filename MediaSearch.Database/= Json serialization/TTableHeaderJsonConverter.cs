using MediaSearch.Database;

using static BLTools.Json.JsonConverterResources;

namespace MediaSearch.Models;

public class TTableHeaderJsonConverter : JsonConverter<ITableHeader>, ILoggable {

  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TTableHeaderJsonConverter>();

  public override bool CanConvert(Type typeToConvert) {
    if (typeToConvert == typeof(TTableHeader)) { return true; }
    if (typeToConvert == typeof(ITableHeader)) { return true; }
    if (typeToConvert.GetInterface(typeof(ITableHeader).Name) is not null) { return true; }
    return false;
  }

  public override ITableHeader? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

    // check that first token is ok
    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    #region --- Identify the kind of IMSTableHeader by finding the MediaType token --------------------------------------------
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

    #endregion --- Identify the kind of IMSTableHeader by finding the MediaType token --------------------------------------------
    ITableHeader? TableHeader;

    try {
      TableHeader = TTableHeader.Create(MediaTypeValue);
      if (TableHeader is null) {
        throw new JsonConverterInvalidDataException(nameof(IMediaSource.MediaType), MediaTypeValue);
      }
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to identify IMSTableHeader from {MediaTypeValue.WithQuotes()}", ex);
      throw new JsonConverterInvalidDataException(nameof(IMediaSource.MediaType), MediaTypeValue, ex);
    }

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        // check if reading is complete
        if (TokenType == JsonTokenType.EndObject) {
          return TableHeader;
        }

        // Do we have a new property token ?
        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {

            case nameof(ITableHeader.Name):
              TableHeader.Name = reader.GetString() ?? "";
              break;

            case nameof(ITableHeader.TableType):
              break;

            case nameof(ITableHeader.Description):
              TableHeader.Description = reader.GetString() ?? "";
              break;

            case nameof(ITableHeader.LastUpdate):
              TableHeader.LastUpdate = DateTime.Parse(reader.GetString() ?? DateTime.MinValue.ToYMDHMS());
              break;

            case nameof(ITableHeader<IRecord>.MediaSource):
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


  public override void Write(Utf8JsonWriter writer, ITableHeader value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }

    writer.WriteStartObject();
    writer.WriteString(nameof(ITableHeader.Name), value.Name);
    writer.WriteString(nameof(ITableHeader.TableType), value.TableType.GetNameEx());
    writer.WriteString(nameof(ITableHeader.Description), value.Description);
    writer.WriteString(nameof(ITableHeader.LastUpdate), value.LastUpdate.ToYMDHMS());

    writer.WritePropertyName(nameof(ITableHeader<IRecord>.MediaSource));
    JsonSerializer.Serialize(writer, value.MediaSource, options);

    writer.WriteEndObject();
  }
}
