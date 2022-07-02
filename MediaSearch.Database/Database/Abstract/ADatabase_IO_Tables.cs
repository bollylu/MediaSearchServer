namespace MediaSearch.Database;

public abstract partial class ADatabase {

  public abstract bool TableCreate(ITable table);
  public abstract bool TableExists(string table);
  public virtual bool TableExists(ITable table) {
    return TableExists(table.Name);
  }
  public virtual bool TableCheck(ITable table) {
    return TableCheck(table.Name);
  }
  public abstract bool TableCheck(string table);
  public virtual bool TableRemove(ITable table) {
    return TableRemove(table.Name);
  }
  public abstract bool TableRemove(string table);
  public virtual bool TableReindex(ITable table) {
    return TableReindex(table.Name);
  }
  public abstract bool TableReindex(string table);

  public abstract bool TableWriteHeader(ITable table);

  public virtual ITableHeader? TableReadHeader(ITable table) {
    return TableReadHeader(table.Name);
  }
  public abstract ITableHeader? TableReadHeader(string table);

  public virtual string TableDump(ITable table) {
    StringBuilder RetVal = new();

    return RetVal.ToString();
  }

  public abstract IEnumerable<ITable> TableList();
  public abstract IEnumerable<ITable> TableSystemList();

}
