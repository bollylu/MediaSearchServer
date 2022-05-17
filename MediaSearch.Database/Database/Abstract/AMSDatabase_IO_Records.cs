namespace MediaSearch.Database;

public abstract partial class AMSDatabase {

  public abstract bool Write(IMSTable? table, IMSRecord record);
  public virtual bool Write(string tableName, IMSRecord record) {
    return Write(GetTable(tableName), record);
  }

  public abstract RECORD Read<RECORD>(IMSTable table, string key) where RECORD : IMSRecord;
  public abstract RECORD Read<RECORD>(string table, string key) where RECORD : IMSRecord;

}
