namespace MediaSearch.Database;

public partial class TMSDatabaseMemory {

  public override bool Write(IMSTable table, IMSRecord record) {
    if (!IsOpened) {
      Logger.LogErrorBox($"Unable to write record in table {table.Name.WithQuotes()} : Database must be opened first", record);
      return false;
    }
    if (!TableExists(table)) {
      Logger.LogErrorBox($"Unable to write record in table {table.Name.WithQuotes()} : Table does not exist", record);
      return false;
    }

    try {

      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to write record in table {table.Name.WithQuotes()}", ex);
      return false;
    }
  }


  public override RECORD Read<RECORD>(IMSTable table, string key)  {
    throw new NotImplementedException();
  }

  public override RECORD Read<RECORD>(string table, string key) {
    throw new NotImplementedException();
  }
}
