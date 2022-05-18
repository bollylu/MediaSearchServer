namespace MediaSearch.Database;
public interface IContext<RECORD> : IDisposable where RECORD : IMSRecord {

  IMSDatabase Database { get; }
  string Table { get; }

  void Execute(IContextOperation operation);
  void Commit();
}
