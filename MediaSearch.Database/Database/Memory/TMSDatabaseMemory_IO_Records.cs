﻿namespace MediaSearch.Database;

public partial class TMSDatabaseMemory {

  public bool Write(IMSTable? table, IMSRecord record) {
    if (table is null) {
      Logger.LogErrorBox($"Unable to write record : Table is null", record);
      return false;
    }

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

  public bool Write(string tableName, IMSRecord record) {
    return Write(GetTable(tableName), record);
    

  }

  public RECORD Read<RECORD>(IMSTable table, string key)
    where RECORD : IMSRecord {
    throw new NotImplementedException();
  }

  public RECORD Read<RECORD>(string table, string key)
    where RECORD : IMSRecord {
    throw new NotImplementedException();
  }
}
