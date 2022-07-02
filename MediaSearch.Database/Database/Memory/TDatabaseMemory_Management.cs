namespace MediaSearch.Database;

public partial class TDatabaseMemory {

  public override bool Create() {
    return true;
  }

  public override bool Create(string schema) {
    return true;
  }

  public override bool Remove() {
    return true;
  }

  public override bool Exists() {
    return true;
  }

}
