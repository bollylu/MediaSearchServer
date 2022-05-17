namespace MediaSearch.Database;

public partial class TMSDatabaseMemory {

  public bool Create() {
    return true;
  }

  public bool Create(string schema) {
    return true;
  }

  public bool Remove() {
    return true;
  }

  public bool Exists() {
    return true;
  }

  public bool Reindex(IMSTable table) {
    if (Tables.Any(t => t.Name == table.Name)) {
      return true;
    }
    Logger.LogWarningBox($"Unable to reindex table : {table.Name.WithQuotes()} is missing", table);
    return false;
  }

  public bool DbCheck() {
    return true;
  }

  

}
