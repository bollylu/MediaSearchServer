﻿namespace MediaSearch.Database;

public class TSchema : ISchema, IDisposable {

  private readonly List<ITable> Tables = new();
  private readonly object _Lock = new object();

  public IDatabase? Database { get; set; }

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TSchema() { }
  public TSchema(IDatabase? database) {
    Database = database;
  }

  public void Dispose() {
    Tables?.Clear();
    Database = null;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{Tables.Count} item(s)");
    foreach (ITable TableItem in Tables) {
      RetVal.AppendIndent($"- {TableItem.Name.WithQuotes()}", 2);
    }
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public ILogger Logger { get; set; } = new TConsoleLogger<TSchema>();

  public IEnumerable<ITable> GetAllTables() {
    ITable[] Retval;
    lock (_Lock) {
      Retval = Tables.ToArray();
    }
    foreach (ITable TableItem in Retval) {
      yield return TableItem;
    }
  }

  public ITable? Get(string name) {
    lock (_Lock) {
      ITable? RetVal = Tables.SingleOrDefault(t => t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
      if (RetVal is null) {
        Logger.LogErrorBox($"Table {name.WithQuotes()} is missing from database schema", Tables);
      }
      return RetVal;
    }
  }

  public IEnumerable<string> List() {
    IList<ITable> Retval;
    lock (_Lock) {
      Retval = new List<ITable>(Tables);
    }
    foreach (ITable<IRecord> TableItem in Retval) {
      yield return TableItem.Name;
    }
  }

  public bool AddTable(ITable table) {
    if (Database is null) {
      return false;
    }

    lock (_Lock) {

      if (Tables.Any(t => t.Name.Equals(table.Name, StringComparison.InvariantCultureIgnoreCase))) {
        Logger.LogError($"Unable to add table {table.Name.WithQuotes()} to database schema : a table with the same name already exists");
        return false;
      }

      table.Database = Database;
      Tables.Add(table);
      return true;
    }
  }

  public bool Remove(ITable table) {
    return Remove(table.Name);
  }

  public bool Remove(string tableName) {
    lock (_Lock) {

      int TableIndex = Tables.FindIndex(t => t.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase));
      if (TableIndex < 0) {
        Logger.LogWarning($"Unable to remove table {tableName.WithQuotes()} from database schema : table is missing");
        return false;
      }

      Tables.RemoveAt(TableIndex);
      return true;
    }
  }

  public void Clear() {
    lock (_Lock) {
      Tables.Clear();
    }
  }

  public bool Exists() {
    if (Database is null) {
      return false;
    }
    return Database.SchemaExists();
  }

  public bool Read() {
    if (Database is null) {
      return false;
    }
    return Database.SchemaRead();
  }

  public bool Save() {
    if (Database is null) {
      return false;
    }
    return Database.SchemaSave();
  }

  public bool Build() {
    if (Database is null) {
      return false;
    }
    return Database.SchemaBuild();
  }
}