namespace MediaSearch.Database;

public interface ISchema : IDisposable {

  IEnumerable<ITable> GetAll();
  ITable? Get(string name);
  IEnumerable<string> List();
  bool Add(ITable table);
  bool Remove(ITable table);
  bool Remove(string tableName);

  void Clear();

  ///// <summary>
  ///// Convert to schema to a Json representation
  ///// </summary>
  ///// <returns>A Json string representation</returns>
  //JsonObject ToJson() {

  //  JsonArray Tables = new();

  //  foreach (ITable TableItem in GetAll()) {
  //    Tables.Add(TableItem.ToJson());
  //  }

  //  return new JsonObject(Tables);



  //  //JsonWriterOptions JsonWriterOptions = new JsonWriterOptions() {
  //  //  Indented = true,
  //  //  Encoder = IJson.DefaultJsonSerializerOptions.Encoder
  //  //};

  //  //using (MemoryStream RetVal = new MemoryStream()) {
  //  //  using (Utf8JsonWriter Writer = new Utf8JsonWriter(RetVal, JsonWriterOptions)) {
  //  //    Writer.WriteStartArray("Tables");
  //  //    foreach (ITable TableItem in GetAll()) {
  //  //      Writer.WriteRawValue(TableItem.ToJson());
  //  //    }
  //  //    Writer.WriteEndArray();
  //  //    Writer.Flush();
  //  //  }
  //  //  return Encoding.UTF8.GetString(RetVal.ToArray());
  //  //}
  //}

  ///// <summary>
  ///// Convert a Json string representing a schema into an actual schema object
  ///// </summary>
  ///// <param name="source">The Json source</param>
  ///// <returns>A new ISchema</returns>
  ///// <exception cref="JsonConverterInvalidDataException"></exception>
  //static ISchema FromJson(string source) {
  //  try {
  //    ISchema? Retval = new TSchema();
  //    JsonDocument Json = JsonDocument.Parse(source);
  //    JsonElement Root = Json.RootElement;
  //    foreach (JsonElement TableItem in Root.EnumerateArray()) {
  //      ITable? Table = IJson.FromJson<ITable>(TableItem.GetRawText());
  //      if (Table is not null) {
  //        Retval.Add(Table);
  //      }
  //    }
  //    return Retval;
  //  } catch (Exception ex) {
  //    throw new JsonConverterInvalidDataException("Error converting Schema", source, ex);
  //  }
  //}

}
