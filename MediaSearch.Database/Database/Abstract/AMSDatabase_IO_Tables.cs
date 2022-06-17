namespace MediaSearch.Database;

public abstract partial class AMSDatabase {

  public abstract bool TableCreate(IMSTable table);
  public abstract bool TableExists(string table);
  public virtual bool TableExists(IMSTable table) {
    return TableExists(table.Name);
  }
  public virtual bool TableCheck(IMSTable table) {
    return TableCheck(table.Name);
  }
  public abstract bool TableCheck(string table);
  public virtual bool TableRemove(IMSTable table) {
    return TableRemove(table.Name);
  }
  public abstract bool TableRemove(string table);
  public virtual bool TableReindex(IMSTable table) {
    return TableReindex(table.Name);
  }
  public abstract bool TableReindex(string table);

  public abstract bool TableWriteHeader(IMSTable table);

  public virtual IMSTableHeader? TableReadHeader(IMSTable table) {
    return TableReadHeader(table.Name);
  }
  public abstract IMSTableHeader? TableReadHeader(string table);

  public virtual string TableDump(IMSTable table) {
    StringBuilder RetVal = new();

    return RetVal.ToString();
  }

  public abstract IEnumerable<IMSTable> TableList();
  public abstract IEnumerable<IMSTable> TableSystemList();

}
