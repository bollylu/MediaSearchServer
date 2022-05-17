namespace MediaSearch.Database;

public partial class TMSDatabaseMemory {

  public List<IMSTable> Tables { get; } = new List<IMSTable>();

  public string GetSchema() {
    return "";
  }

  public bool AddTable(IMSTable table) {
    if (!IsOpened) {
      return false;
    }

    if (Tables.Any(t => t.Name == table.Name)) {
      return false;
    }

    Tables.Add(table);
    return true;

  }

  public IMSTable? GetTable(string name) {
  
  IMSTable? RetVal = Tables.SingleOrDefault(t=>t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
    if (RetVal is null) {
      Logger.LogErrorBox($"More than on table with the same name {name.WithQuotes()} in database", this);
    }
    return RetVal;
  
  }

}
