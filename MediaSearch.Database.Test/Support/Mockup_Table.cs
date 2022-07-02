namespace MediaSearch.Test.Database;

public class Mockup_Table : ATable<Mockup_Record_IID> {

  public bool Exists() {
    throw new NotImplementedException();
  }

  public bool Create() {
    throw new NotImplementedException();
  }

  public void Remove() {
    throw new NotImplementedException();
  }

  public bool OpenOrCreate() {
    throw new NotImplementedException();
  }

  public void Close() {
    throw new NotImplementedException();
  }

  public Task CloseAsync(CancellationToken token) {
    throw new NotImplementedException();
  }

  public bool Load() {
    throw new NotImplementedException();
  }

  public Task<bool> LoadAsync(CancellationToken token) {
    throw new NotImplementedException();
  }

  public bool AutoSave { get; set; }

  public bool Save() {
    throw new NotImplementedException();
  }

  public Task<bool> SaveAsync(CancellationToken token) {
    throw new NotImplementedException();
  }

  public Task<bool> SaveAsync() {
    throw new NotImplementedException();
  }

  public bool IsDirty { get; }
}