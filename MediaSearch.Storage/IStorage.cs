namespace MediaSearch.Storage;

public interface IStorage {

  bool Exists();
  bool Create();
  bool Remove();

}
