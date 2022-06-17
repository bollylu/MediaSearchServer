namespace MediaSearch.Database;

public interface ISchema : IDisposable {

  IEnumerable<IMSTable> GetAll();
  IMSTable? Get(string name);
  IEnumerable<string> List();
  bool Add(IMSTable table);
  bool Remove(IMSTable table);
  bool Remove(string tableName);

}
