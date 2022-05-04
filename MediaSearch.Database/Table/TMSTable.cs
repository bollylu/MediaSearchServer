using BLTools;

namespace MediaSearch.Database;
public class TMSTable<RECORD> : IMSTable<RECORD>, IMediaSearchLoggable<TMSTable<RECORD>>
  where RECORD : IID<string> {

  public IMediaSearchLogger<TMSTable<RECORD>> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMSTable<RECORD>>();

  public string Name { get; set; } = "";

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMSTable() { }
  public TMSTable(string tableName) {
    Name = tableName;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    StringBuilder RetVal = new();
    RetVal.AppendIndent($"- {nameof(Name)} = {Name}", indent);
    RetVal.AppendIndent($"- Table Type = {typeof(RECORD).Name}", indent);
    RetVal.AppendIndent($"- Header", indent);
    RetVal.AppendIndent(Header.ToString(indent), indent + 2);
    if (Indexes.Any()) {
      RetVal.AppendIndent($"- Indexes", indent);
      foreach (var IndexItem in Indexes) {
        RetVal.AppendIndent(IndexItem.ToString(indent), indent + 2);
      }
    } else {
      RetVal.AppendIndent("- No index", indent);
    }
    return RetVal.ToString();
  }


  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public IMSTableHeader<RECORD> Header { get; } = new TMSTableHeader<RECORD>();

  public List<IMSIndex<RECORD>> Indexes { get; } = new List<IMSIndex<RECORD>>();

  public void Add(RECORD item) {
    throw new NotImplementedException();
  }

  public void AddOrUpdate(RECORD item) {
    throw new NotImplementedException();
  }

  public void Update(RECORD item) {
    throw new NotImplementedException();
  }

  public void Clear() {
    throw new NotImplementedException();
  }

  public void Delete(RECORD item) {
    throw new NotImplementedException();
  }

  public IEnumerable<RECORD> GetAll(int maxRecords = 0) {
    throw new NotImplementedException();
  }

  public IEnumerable<RECORD> GetFiltered(IFilter filter, int maxRecords = 0) {
    throw new NotImplementedException();
  }

  public RECORD? Get(string id) {
    throw new NotImplementedException();
  }

  public Task AddAsync(RECORD item, CancellationToken token) {
    throw new NotImplementedException();
  }

  public Task AddOrUpdateAsync(RECORD item, CancellationToken token) {
    throw new NotImplementedException();
  }

  public Task UpdateAsync(RECORD item, CancellationToken token) {
    throw new NotImplementedException();
  }

  public Task ClearAsync(CancellationToken token) {
    throw new NotImplementedException();
  }

  public Task DeleteAsync(RECORD item, CancellationToken token) {
    throw new NotImplementedException();
  }

  public IAsyncEnumerable<RECORD> GetAllAsync(CancellationToken token) {
    throw new NotImplementedException();
  }

  public IAsyncEnumerable<RECORD> GetFilteredAsync(TFilter filter, CancellationToken token) {
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
}
