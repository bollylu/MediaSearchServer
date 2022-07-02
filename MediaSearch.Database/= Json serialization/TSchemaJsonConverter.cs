using MediaSearch.Database;

using static BLTools.Json.JsonConverterResources;

namespace MediaSearch.Models;

public class TSchemaJsonConverter : JsonConverter<ISchema>, ILoggable {

  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TSchemaJsonConverter>();

  public override bool CanConvert(Type typeToConvert) {
    if (typeToConvert == typeof(TSchema)) { return true; }
    if (typeToConvert == typeof(ISchema)) { return true; }
    if (typeToConvert.GetInterface(typeof(ISchema).Name) is not null) { return true; }
    return false;
  }

  public override ISchema Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

    // check that first token is ok
    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    ISchema RetVal = new TSchema();

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        // check if reading is complete
        if (TokenType == JsonTokenType.EndObject) {
          return RetVal;
        }

        // Do we have a new property token ?
        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {



            default:
              Logger.LogWarningBox(ERROR_INVALID_PROPERTY, Property);
              break;
          }
        }
      }

      Logger.LogErrorBox(ERROR_CONVERSION, RetVal);
      throw new JsonConverterInvalidDataException(nameof(ISchema), RetVal);

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

    writer.WriteStartObject();
    writer.WriteStartArray("Tables");
    foreach (ITable TableItem in value.GetAll()) {
      JsonSerializer.Serialize(writer, TableItem, options);
    }
    writer.WriteEndArray();
    writer.WriteEndObject();
  }
}
