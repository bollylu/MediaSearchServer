namespace MediaSearch.Database;

public abstract partial class ADatabase {

  public abstract bool Write(ITable table, IRecord record);
  public virtual bool Write(string tableName, IRecord record) {
    ITable? Table = Schema.Get(tableName);
    if (Table is null) {
      Logger.LogErrorBox($"Unable to write record to table {tableName.WithQuotes()} : table is missing", record);
      return false;
    }
    return Write(Table, record);
  }

  public abstract RECORD? Read<RECORD>(ITable table, string key) where RECORD : class, IRecord;


  public virtual RECORD? Read<RECORD>(string tableName, string key) where RECORD : class, IRecord {
    ITable? Table = Schema.Get(tableName);
    if (Table is null) {
      Logger.LogError("Unable to read record from table : table name is missing");
      throw new ArgumentException("Unable to read record from table : table name is missing", tableName);
    }
    return Read<RECORD>(Table, key);
  }

  public virtual string RecordDump(ITable table, string recordId) {
    StringBuilder RetVal = new();

    return RetVal.ToString();
  }

  public abstract bool Any(ITable table);

  public virtual bool IsEmpty(ITable table) => !Any(table);

  public abstract long Count(ITable table);

  public abstract void Clear(ITable table);
}
