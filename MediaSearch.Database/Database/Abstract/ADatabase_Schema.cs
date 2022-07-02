namespace MediaSearch.Database;

public abstract partial class ADatabase {

  public ISchema Schema { get; } = new TSchema();
  protected readonly object _LockTable = new object();

  public abstract bool BuildSchema();
  public abstract bool ReadSchema();
  public abstract bool SaveSchema();

  //public virtual bool AddTableToSchema(IMSTable table) {
  //  lock (_LockTable) {

  //    if (Schema.Any(t => t.Name.Equals(table.Name, StringComparison.InvariantCultureIgnoreCase))) {
  //      Logger.LogError($"Unable to add table {table.Name.WithQuotes()} to database : a table with the same name already exists");
  //      return false;
  //    }

  //    table.Database = this;
  //    Schema.Add(table);
  //    return true;
  //  }
  //}


  //public virtual bool RemoveTable(IMSTable table) {
  //  return Schema.Remove(table);
  //}

  //public virtual bool RemoveTable(string tableName) {

  //  lock (_LockTable) {
  //    if (!IsOpened) {
  //      Logger.LogError($"Unable to add table {tableName.WithQuotes()} to database : database must be opened first");
  //      return false;
  //    }

  //    int TableIndex = Schema.FindIndex(t => t.Name.Equals(t.Name, StringComparison.InvariantCultureIgnoreCase));
  //    if (TableIndex < 0) {
  //      Logger.LogWarning($"Unable to remove table {tableName.WithQuotes()} from database : table is missing");
  //      return false;
  //    }

  //    Schema[TableIndex].Database = null;
  //    Schema.RemoveAt(TableIndex);
  //    return true;
  //  }
  //}


  //public virtual IMSTable? GetTable(string name) {
  //  lock (_LockTable) {
  //    IMSTable? RetVal = Schema.SingleOrDefault(t => t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
  //    if (RetVal is null) {
  //      Logger.LogErrorBox($"Table {name.WithQuotes()} is missing from database", Schema);
  //    }
  //    return RetVal;
  //  }
  //}

  //public virtual IEnumerable<IMSTable> GetTables() {
  //  IMSTable[] RetVal;
  //  lock (_LockTable) {
  //    RetVal = Schema.ToArray();
  //  }
  //  foreach (IMSTable TableItem in RetVal) {
  //    yield return TableItem;
  //  }
  //}
}
