namespace MediaSearch.Database;

public abstract class AMSTable<RECORD> : IDisposable, IMSTable<RECORD> where RECORD : class, IMSRecord {

  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TMSTable>();

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

  public IMSTableHeader? Header { get; protected set; }

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  protected AMSTable() {
    Header = TMSTableHeader.Create(Name, typeof(RECORD));
  }

  protected AMSTable(string tableName) {
    Name = tableName;
    Header = TMSTableHeader.Create(Name, typeof(RECORD));
  }

  protected AMSTable(string tableName, IMSDatabase database) {
    Name = tableName;
    Database = database;
    Header = TMSTableHeader.Create(Name, typeof(RECORD));
  }

  protected AMSTable(IMSTableHeader header, IMSDatabase database) {
    Name = header.Name;
    Database = database;
    Header = header;
  }
  public void Dispose() {
    Database = null;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    StringBuilder RetVal = new();
    RetVal.AppendIndent($"- {nameof(Name)} = {Name.WithQuotes()}", indent);
    RetVal.AppendIndent($"- {nameof(Database)} = {Database?.Name?.WithQuotes() ?? "No database"}", indent);
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

  #region --- Infos --------------------------------------------
  public virtual bool IsEmpty() {
    if (Database is null) {
      Logger.LogError("Unable to count record : database is not specified");
      return true;
    }
    return Database.IsEmpty(this);
  }

  public virtual bool Any() {
    if (Database is null) {
      Logger.LogError("Unable to count record : database is not specified");
      return false;
    }
    return Database.Any(this);
  }

  public virtual long Count() {
    if (Database is null) {
      Logger.LogError("Unable to count record : database is not specified");
      return 0;
    }
    return Database.Count(this);
  }

  public virtual void Clear() {
    if (Database is null) {
      Logger.LogError($"Unable to clear table {Name.WithQuotes()} : database is not specified");
      return;
    }
    Database.Clear(this);
  }
  #endregion --- Infos --------------------------------------------

  public List<IMSIndex<RECORD>> Indexes { get; } = new List<IMSIndex<RECORD>>();

  #region --- Records --------------------------------------------
  public void Add(RECORD item) {
    if (Database is null) {
      Logger.LogError("Unable to add record : database is not specified");
      return;
    }
    Database.Write(this, item);
  }

  public void AddOrUpdate(RECORD item) {
    throw new NotImplementedException();
  }

  public void Update(RECORD item) {
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
  #endregion --- Records --------------------------------------------


}

