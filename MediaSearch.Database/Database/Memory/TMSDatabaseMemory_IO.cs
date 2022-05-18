namespace MediaSearch.Database;

public partial class TMSDatabaseMemory {

  public override bool Open() {
    if (!Exists()) {
      return false;
    }

    IsOpened = true;
    return true;
  }

  public override bool Close() {
    IsOpened = false;
    return true;
  }

}
