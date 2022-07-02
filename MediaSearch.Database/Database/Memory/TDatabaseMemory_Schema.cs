namespace MediaSearch.Database;

public partial class TDatabaseMemory {

  public override bool BuildSchema() {
    return true;
  }

  public override bool ReadSchema() {
    return true;
  }
  public override bool SaveSchema() {
    return true;
  }

}
