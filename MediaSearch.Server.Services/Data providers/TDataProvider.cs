
namespace MediaSearch.Server.Services;

public class TDataProvider : ALoggable, IDataProvider {
  public string RootStoragePath { get; } = "";
  public string Name { get; } = "";
  public string Description { get; } = "";

  public bool Any() {
    throw new NotImplementedException();
  }

  public void Clear() {
    throw new NotImplementedException();
  }

  public int Count() {
    throw new NotImplementedException();
  }

  public IMedia Get(string id) {
    throw new NotImplementedException();
  }

  public IEnumerable<IMedia> GetAll() {
    throw new NotImplementedException();
  }

  public IEnumerable<IMedia> GetFiltered(TFilter filter) {
    throw new NotImplementedException();
  }

  public bool IsEmpty() {
    throw new NotImplementedException();
  }

  public Task Load(CancellationToken token) {
    throw new NotImplementedException();
  }

}
