namespace MediaSearch.Database;

public abstract partial class AMSDatabase {

  public abstract bool Write(IMSTable table, IMSRecord record);
  public virtual bool Write(string tableName, IMSRecord record) {
    IMSTable? Table = GetTable(tableName);
    if (Table is null) {
      Logger.LogErrorBox($"Unable to write record to table {tableName.WithQuotes()} : table is missing", record);
      return false;
    }
    return Write(Table, record);
  }

  public abstract RECORD Read<RECORD>(IMSTable table, string key) where RECORD : IMSRecord;
  public abstract RECORD Read<RECORD>(string table, string key) where RECORD : IMSRecord;

  public virtual string RecordDump(IMSTable table, string recordId) {
    StringBuilder RetVal = new();

    return RetVal.ToString();
  }
}
