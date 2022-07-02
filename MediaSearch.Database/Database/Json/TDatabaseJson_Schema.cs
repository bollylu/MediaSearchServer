namespace MediaSearch.Database;

public partial class TDatabaseJson {

  public const string DATABASE_SCHEMA_NAME = "=schema=.json";

  public override bool BuildSchema() {
    try {
      Schema.Clear();
      foreach (ITable TableItem in TableList()) {
        TableItem.Database = this;
        Schema.Add(TableItem);
      }
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to build schema for database {Name}", ex);
      return false;
    }
  }

  public override bool ReadSchema() {
    try {
      string SchemaFullName = Path.Join(DatabaseFullName, DATABASE_SCHEMA_NAME);
      string RawContent = File.ReadAllText(SchemaFullName);
      ISchema? ConvertedSchema = IJson.FromJson<ISchema>(RawContent);
      if (ConvertedSchema is null) {
        Logger.LogErrorBox("Unable yo read schema", RawContent);
        return false;
      }
      foreach (ITable TableItem in ConvertedSchema.GetAll()) {
        Schema.Add(TableItem);
      }
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox("Unable to save schema", ex);
      return false;
    }
  }
  public override bool SaveSchema() {
    try {
      string SchemaFullName = Path.Join(DatabaseFullName, DATABASE_SCHEMA_NAME);
      string JsonSchema = IJson.ToJson(Schema);
      File.WriteAllText(SchemaFullName, JsonSchema);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox("Unable to save schema", ex);
      return false;
    }

  }
}
