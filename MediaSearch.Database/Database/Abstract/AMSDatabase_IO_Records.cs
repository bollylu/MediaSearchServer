namespace MediaSearch.Database;

public abstract partial class AMSDatabase {

  public abstract bool Write(IMSTable table, IMSRecord record);
  public virtual bool Write(string tableName, IMSRecord record) {
    IMSTable? Table = Schema.Get(tableName);
    if (Table is null) {
      Logger.LogErrorBox($"Unable to write record to table {tableName.WithQuotes()} : table is missing", record);
      return false;
    }
    return Write(Table, record);
  }

  public abstract RECORD? Read<RECORD>(IMSTable table, string key) where RECORD : class, IMSRecord;


  public virtual RECORD? Read<RECORD>(string tableName, string key) where RECORD : class, IMSRecord {
    IMSTable? Table = Schema.Get(tableName);
    if (Table is null) {
      Logger.LogError("Unable to read record from table : table name is missing");
      throw new ArgumentException("Unable to read record from table : table name is missing", tableName);
    }
    return Read<RECORD>(Table, key);
  }

  public virtual string RecordDump(IMSTable table, string recordId) {
    StringBuilder RetVal = new();

    return RetVal.ToString();
  }

  public abstract bool Any(IMSTable table);

  public virtual bool IsEmpty(IMSTable table) => !Any(table);

  public abstract long Count(IMSTable table);

  public abstract void Clear(IMSTable table);
}
