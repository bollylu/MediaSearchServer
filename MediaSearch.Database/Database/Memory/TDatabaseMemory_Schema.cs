namespace MediaSearch.Database;

public partial class TDatabaseMemory {

  public override bool SchemaBuild() {
    return true;
  }

  public override bool SchemaRead() {
    return true;
  }
  public override bool SchemaSave() {
    return true;
  }

  public override bool SchemaExists() {
    return true;
  }
}
