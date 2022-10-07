namespace MediaSearch.Database;

public partial class TDatabaseJson {

  string SchemaFullName => Path.Join(DatabaseFullName, DATABASE_SCHEMA_NAME);

  public override bool SchemaBuild() {
    try {
      Schema.Clear();
      foreach (ITable TableItem in TableList()) {
        Schema.AddTable(TableItem);
      }
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to build schema for database {Name}", ex);
      return false;
    }
  }

  public override bool SchemaRead() {
    try {
      string RawContent = File.ReadAllText(SchemaFullName);
      ISchema? ConvertedSchema = IJson.FromJson<ISchema>(RawContent, SerializerOptions);
      if (ConvertedSchema is null) {
        Logger.LogErrorBox("Unable yo read schema", RawContent);
        return false;
      }
      foreach (ITable TableItem in ConvertedSchema.GetAllTables()) {
        Schema.AddTable(TableItem);
      }
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox("Unable to save schema", ex);
      return false;
    }
  }

  public override bool SchemaSave() {
    try {
      string JsonSchema = IJson.ToJson(Schema, SerializerOptions);
      File.WriteAllText(SchemaFullName, JsonSchema);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox("Unable to save schema", ex);
      return false;
    }

  }

  public override bool SchemaExists() {
    try {
      return File.Exists(SchemaFullName);
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to evaluate existence of schema {SchemaFullName}", ex);
      throw;
    }
  }
}
