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


  public override RECORD? Read<RECORD>(IMSTable table, string key) where RECORD : class {
    throw new NotImplementedException();
  }

  public override bool Any(IMSTable table) {
    try {
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to access table {table.Name.WithQuotes()}", ex);
      return false;
    }
  }

  public override long Count(IMSTable table) {
    try {
      return 0;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to access table {table.Name.WithQuotes()}", ex);
      return 0;
    }
  }

  public override void Clear(IMSTable table) {
    throw new NotImplementedException();
  }
}
