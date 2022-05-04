namespace MediaSearch.Database;
public class TContext<RECORD> : IContext<RECORD>, IDisposable where RECORD : IMSRecord {
  public IMSDatabase Database { get; init; }
  public string Table { get; init; }
  public List<RECORD> Records { get; } = new();

  public TContext(IMSDatabase database, string table) {
    Database = database;
    Table = table;
  }

  public void Dispose() {
    //Records.Clear();
  }


  public string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendIndent($"- {nameof(Database)}", indent);
    RetVal.AppendIndent(Database.ToString(indent), indent + 2);
    RetVal.AppendIndent($"- {nameof(Table)} = {Table.WithQuotes()}", indent);
    RetVal.AppendIndent($"- {nameof(Records)} = {Records.Count} item(s)", indent);
    return RetVal.ToString();
  }


  public override string ToString() {
    return ToString(0);
  }

  public void Execute(IContextOperation operation) {
    switch (operation.Operation) {
      case EContextOperation.Get:
        TContextGet<RECORD> GetOperation = (TContextGet<RECORD>)operation;
        RECORD? Record = Database.Get<RECORD>(Table, GetOperation.Key);
        if (Record is not null) {
          Records.Add(Record);
        }
        break;

        //case EContextOperation.Add:
        //  Database.Save(Table, operation.Record);
        //  break;
    }
  }

  public void Commit() {
    throw new NotImplementedException();
  }
}
