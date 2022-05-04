namespace MediaSearch.Database.Test;

public class Mockup_Table : IMSTable<Mockup_Record_IID> {
  public IMSTableHeader<Mockup_Record_IID> Header { get; } = new TMSTableHeader<Mockup_Record_IID>();

  public void Add(Mockup_Record_IID item) {
    throw new NotImplementedException();
  }

  public void AddOrUpdate(Mockup_Record_IID item) {
    throw new NotImplementedException();
  }

  public void Update(Mockup_Record_IID item) {
    throw new NotImplementedException();
  }

  public void Clear() {
    throw new NotImplementedException();
  }

  public void Delete(Mockup_Record_IID item) {
    throw new NotImplementedException();
  }

  public IEnumerable<Mockup_Record_IID> GetAll(int maxRecords = 0) {
    throw new NotImplementedException();
  }

  public IEnumerable<Mockup_Record_IID> GetFiltered(IFilter filter, int maxRecords = 0) {
    throw new NotImplementedException();
  }

  public Mockup_Record_IID? Get(string id) {
    throw new NotImplementedException();
  }

  public Task AddAsync(Mockup_Record_IID item, CancellationToken token) {
    throw new NotImplementedException();
  }

  public Task AddOrUpdateAsync(Mockup_Record_IID item, CancellationToken token) {
    throw new NotImplementedException();
  }

  public Task UpdateAsync(Mockup_Record_IID item, CancellationToken token) {
    throw new NotImplementedException();
  }

  public Task ClearAsync(CancellationToken token) {
    throw new NotImplementedException();
  }

  public Task DeleteAsync(Mockup_Record_IID item, CancellationToken token) {
    throw new NotImplementedException();
  }

  public IAsyncEnumerable<Mockup_Record_IID> GetAllAsync(CancellationToken token) {
    throw new NotImplementedException();
  }

  public IAsyncEnumerable<Mockup_Record_IID> GetFilteredAsync(TFilter filter, CancellationToken token) {
    throw new NotImplementedException();
  }

  public string ToString(int indent) {
    throw new NotImplementedException();
  }

  public bool IsEmpty() {
    throw new NotImplementedException();
  }

  public bool Any() {
    throw new NotImplementedException();
  }

  public int Count() {
    throw new NotImplementedException();
  }

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
  public List<IMSIndex<Mockup_Record_IID>> Indexes { get; } = new();
  public string Name { get; set; } = "MockupTable";
}