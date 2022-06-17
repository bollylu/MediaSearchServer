﻿namespace MediaSearch.Database;

public class TSchema : ISchema, IDisposable {

  private readonly List<IMSTable> Tables = new();
  private readonly object _Lock = new object();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TSchema() { }
  public void Dispose() {
    Tables?.Clear();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{Tables.Count} item(s)");
    foreach (IMSTableGeneric TableItem in Tables) {
      RetVal.AppendIndent($"- {TableItem.Name.WithQuotes()}", 2);
    }
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public ILogger Logger { get; set; } = new TConsoleLogger<TSchema>();

  public IEnumerable<IMSTable> GetAll() {
    IMSTable[] Retval;
    lock (_Lock) {
      Retval = Tables.ToArray();
    }
    foreach (IMSTable TableItem in Retval) {
      yield return TableItem;
    }
  }

  public IMSTable? Get(string name) {
    lock (_Lock) {
      IMSTable? RetVal = Tables.SingleOrDefault(t => t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
      if (RetVal is null) {
        Logger.LogErrorBox($"Table {name.WithQuotes()} is missing from database schema", Tables);
      }
      return RetVal;
    }
  }

  public IEnumerable<string> List() {
    IList<IMSTable> Retval;
    lock (_Lock) {
      Retval = new List<IMSTable>(Tables);
    }
    foreach (IMSTable<IMSRecord> TableItem in Retval) {
      yield return TableItem.Name;
    }
  }

  public bool Add(IMSTable table) {
    lock (_Lock) {

      if (Tables.Any(t => t.Name.Equals(table.Name, StringComparison.InvariantCultureIgnoreCase))) {
        Logger.LogError($"Unable to add table {table.Name.WithQuotes()} to database schema : a table with the same name already exists");
        return false;
      }

      Tables.Add(table);
      return true;
    }
  }

  public bool Remove(IMSTable table) {
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


}
