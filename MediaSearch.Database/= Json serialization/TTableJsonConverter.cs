using MediaSearch.Database;

using static BLTools.Json.JsonConverterResources;

namespace MediaSearch.Models;

public class TTableJsonConverter : JsonConverter<ITable>, ILoggable {

  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TTableJsonConverter>();

  public override bool CanConvert(Type typeToConvert) {
    if (typeToConvert == typeof(TTable)) { return true; }
    if (typeToConvert == typeof(ITable)) { return true; }
    if (typeToConvert.GetInterface(typeof(ITable).Name) is not null) { return true; }
    return false;
  }

  public override ITable? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

    // check that first token is ok
    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    string? TableName = "";
    ITableHeader? TableHeader = null;
    string? TableType = "";


    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        // check if reading is complete
        if (TokenType == JsonTokenType.EndObject) {
          try {

            if (TableHeader is null) {
              throw new JsonConverterInvalidDataException(nameof(IMediaSource.MediaType), "Table header is (null)");
            }

            ITable? RetVal = TTable.Create(TableName, TableHeader.TableType);
            if (RetVal is null) {
              throw new JsonConverterInvalidDataException(nameof(IMediaSource.MediaType), TableHeader.TableType);
            }

            return RetVal;

          } catch (Exception ex) {
            Logger.LogErrorBox($"Unable to identify ITable from {TableType.WithQuotes()}", ex);
            throw new JsonConverterInvalidDataException(nameof(IMediaSource.MediaType), TableType, ex);
          }

        }

        // Do we have a new property token ?
        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {

            case nameof(ITable.Name):
              TableName = reader.GetString() ?? "";
              break;

            case nameof(ITable.Header):
              TableHeader = JsonSerializer.Deserialize<ITableHeader>(ref reader, options);
              break;

            default:
              Logger.LogWarningBox(ERROR_INVALID_PROPERTY, Property);
              break;
          }
        }
      }

      Logger.LogErrorBox(ERROR_CONVERSION, TableName ?? "Invalid table");
      return null;

    } catch (Exception ex) {
      Logger.LogErrorBox(ERROR_CONVERSION, ex);
      throw;
    }

  }

  public override void Write(Utf8JsonWriter writer, ITable value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }

    writer.WriteStartObject();
    writer.WriteString(nameof(ITable.Name), value.Name);
    //writer.WriteString(nameof(ITable.Database), value.Database?.Name ?? "(no database)");
    writer.WritePropertyName(nameof(ITable.Header));
    JsonSerializer.Serialize(writer, value.Header, options);

    writer.WriteEndObject();
  }
}
