namespace MediaSearch.Database;

public abstract partial class AMSDatabase {

  protected readonly List<IMSTable> Tables = new List<IMSTable>();
  protected readonly object _LockTable = new object();

  public abstract string GetSchema();

  public virtual bool AddTable(IMSTable table) {
    lock (_LockTable) {
      if (!IsOpened) {
        Logger.LogError($"Unable to add table {table.Name.WithQuotes()} to database : database must be opened first");
        return false;
      }

      if (Tables.Any(t => t.Name.Equals(table.Name, StringComparison.InvariantCultureIgnoreCase))) {
        Logger.LogError($"Unable to add table {table.Name.WithQuotes()} to database : a table with tyhe same name already exists");
        return false;
      }

      table.Database = this;
      Tables.Add(table);
      return true;
    }
  }


  public virtual bool RemoveTable(IMSTable table) {
    return RemoveTable(table.Name);
  }

  public virtual bool RemoveTable(string tableName) {

    lock (_LockTable) {
      if (!IsOpened) {
        Logger.LogError($"Unable to add table {tableName.WithQuotes()} to database : database must be opened first");
        return false;
      }

      int TableIndex = Tables.FindIndex(t => t.Name.Equals(t.Name, StringComparison.InvariantCultureIgnoreCase));
      if (TableIndex < 0) {
        Logger.LogWarning($"Unable to remove table {tableName.WithQuotes()} from database : table is missing");
        return false;
      }

      Tables[TableIndex].Database = null;
      Tables.RemoveAt(TableIndex);
      return true;
    }
  }


  public virtual IMSTable? GetTable(string name) {
    lock (_LockTable) {
      IMSTable? RetVal = Tables.SingleOrDefault(t => t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
      if (RetVal is null) {
        Logger.LogErrorBox($"Table {name.WithQuotes()} is missing from database", Tables);
      }
      return RetVal;
    }
  }
}
