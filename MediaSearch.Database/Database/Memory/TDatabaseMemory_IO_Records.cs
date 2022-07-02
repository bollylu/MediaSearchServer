namespace MediaSearch.Database;

public partial class TDatabaseMemory {

  public override bool Write(ITable table, IRecord record) {
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


  public override RECORD? Read<RECORD>(ITable table, string key) where RECORD : class {
    throw new NotImplementedException();
  }

  public override bool Any(ITable table) {
    try {
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to access table {table.Name.WithQuotes()}", ex);
      return false;
    }
  }

  public override long Count(ITable table) {
    try {
      return 0;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to access table {table.Name.WithQuotes()}", ex);
      return 0;
    }
  }

  public override void Clear(ITable table) {
    throw new NotImplementedException();
  }
}
