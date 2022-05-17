namespace MediaSearch.Database;

public partial class TMSDatabaseMemory {

  public bool Open() {
    if (!Exists()) {
      return false;
    }

    IsOpened = true;
    return true;
  }

  public bool Close() {
    IsOpened = false;
    return true;
  }

}
