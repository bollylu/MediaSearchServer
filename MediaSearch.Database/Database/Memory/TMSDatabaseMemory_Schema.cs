namespace MediaSearch.Database;

public partial class TMSDatabaseMemory {

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
