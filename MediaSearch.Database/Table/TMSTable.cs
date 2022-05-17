namespace MediaSearch.Database;

public class TMSTable : IMSTable, IMediaSearchLoggable<TMSTable> {

  public IMediaSearchLogger<TMSTable> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMSTable>();

  public IMSDatabase? Database {
    get {
      return _Database;
    }
    set {
      _Database = value;
    }
  }
  protected IMSDatabase? _Database;

  public string Name { get; set; } = "";

  public IMSTableHeader Header { get; protected set; }

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMSTable() {
    Header = new TMSTableHeader();
  }
  public TMSTable(string tableName) {
    Header = new TMSTableHeader();
    Name = tableName;
  }
  public TMSTable(string tableName, IMSDatabase database) {
    Name = tableName;
    Database = database;
    Header = new TMSTableHeader();
  }

  public virtual void Dispose() {
    Database = null;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public virtual string ToString(int indent) {
    StringBuilder RetVal = new();
    RetVal.AppendIndent($"- {nameof(Name)} = {Name.WithQuotes()}", indent);
    RetVal.AppendIndent($"- {nameof(Database)} = {Database?.Name?.WithQuotes() ?? "No database"}", indent);
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public virtual bool IsEmpty() {
    throw new NotImplementedException();
  }

  public virtual bool Any() {
    throw new NotImplementedException();
  }

  public virtual int Count() {
    throw new NotImplementedException();
  }
}

public class TMSTable<RECORD> : TMSTable, IMSTable<RECORD>
  where RECORD : IID<string> {

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMSTable() : base() {
    Header = new TMSTableHeader<RECORD>();
  }
  public TMSTable(string tableName) : base(tableName) {
    Header = new TMSTableHeader<RECORD>();
  }

  public override void Dispose() {
    Database = null;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new(base.ToString(indent));
    RetVal.AppendIndent($"- Table Type = {typeof(RECORD)}", indent);
    if (Header is not null) {
      RetVal.AppendIndent($"- Header", indent);
      RetVal.AppendIndent(Header.ToString(indent), indent + 2);
    } else {
      RetVal.AppendIndent($"- Header is missing", indent);
    }
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




}
