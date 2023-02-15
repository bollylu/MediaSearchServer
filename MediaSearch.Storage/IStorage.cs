namespace MediaSearch.Storage;

public interface IStorage {

  string PhysicalDataPath { get; }

  bool Exists();
  bool Create();
  bool Remove();

}
