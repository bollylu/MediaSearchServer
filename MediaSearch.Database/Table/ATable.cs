namespace MediaSearch.Database;

public abstract class ATable<RECORD> : IDisposable, ITable<RECORD> where RECORD : class, IRecord {

  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<ITable>();

  public IDatabase? Database {
    get {
      return _Database;
    }
    set {
      _Database = value;
    }
  }
  protected IDatabase? _Database;

  public string Name {
    get {
      return Header?.Name ?? "";
    }
    set {
      if (Header is null) {
        Header ??= TTableHeader.Create(typeof(RECORD));
        if (Header is null) {
          Logger.LogFatal("Unable to create header");
          throw new ApplicationException("Unable to create header");
        }
      }
      Header.Name = value;
    }
  }

  public ITableHeader? Header { get; protected set; }

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  protected ATable() {
    Header = TTableHeader.Create(Name, typeof(RECORD));
  }

  protected ATable(string tableName) {
    Name = tableName;
    Header = TTableHeader.Create(Name, typeof(RECORD));
  }

  protected ATable(string tableName, IDatabase database) {
    Name = tableName;
    Database = database;
    Header = TTableHeader.Create(Name, typeof(RECORD));
  }

  protected ATable(ITableHeader header, IDatabase database) {
    Name = header.Name;
    Database = database;
    Header = header;
  }
  public void Dispose() {
    Database = null;
    Header = null;
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

  public List<IIndex<RECORD>> Indexes { get; } = new List<IIndex<RECORD>>();

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

