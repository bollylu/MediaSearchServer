namespace MediaSearch.Database;

public partial class TMSDatabaseJson {

  public const string DATABASE_SCHEMA_NAME = "=schema=.json";

  public override bool BuildSchema() {
    return true;
  }

  public override bool ReadSchema() {
    return true;
  }
  public override bool SaveSchema() {
    try {
      string SchemaFullName = Path.Join(DatabaseFullName, DATABASE_SCHEMA_NAME);
      StringBuilder SchemaContent = new();
      foreach (IMSTableGeneric TableItem in Schema.GetAll()) {
        SchemaContent.AppendLine(TableItem.ToString());
      }
      File.WriteAllText(SchemaFullName, SchemaContent.ToString());
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox("Unable to save schema", ex);
      return false;
    }

  }
}
