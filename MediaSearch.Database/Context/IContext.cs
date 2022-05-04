namespace MediaSearch.Database;
public interface IContext<RECORD> : IDisposable where RECORD : IMSRecord {

  IMSDatabase Database { get; }
  string Table { get; }

  List<RECORD> Records { get; }

  void Execute(IContextOperation operation);
  void Commit();
}
