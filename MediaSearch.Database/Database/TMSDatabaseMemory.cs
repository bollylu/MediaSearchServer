namespace MediaSearch.Database;

public class TMSDatabaseMemory : IMSDatabase, IMediaSearchLoggable<TMSDatabaseMemory> {

  public IMediaSearchLogger<TMSDatabaseMemory> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMSDatabaseMemory>();

  public string Name { get; set; } = "";

  public string Description { get; set; } = "";

  public string DatabaseFullName {
    get {
      return Name;
    }
  }

  public List<IMSTable> Tables { get; } = new List<IMSTable>();

  public string GetSchema() {
    return "";
  }

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    StringBuilder RetVal = new();
    RetVal.AppendIndent($"- {nameof(Name)} = {Name.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(Description)} = {Description.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(DatabaseFullName)} = {DatabaseFullName.WithQuotes()}", indent);
    if (Tables.Any()) {
      RetVal.AppendIndent($"- {nameof(Tables)}", indent);
      foreach (IMSTable TableItem in Tables) {
        RetVal.AppendIndent(TableItem.ToString(indent), indent + 2);
      }
    }else {
      RetVal.AppendIndent("- No table available", indent);
    }
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public bool Create() {
    return true;
  }

  public bool Create(string schema) {
    return true;
  }

  public bool Remove() {
    return true;
  }

  public bool Exists() {
    return true;
  }

  public bool Reindex() {
    return true;
  }

  public bool DbCheck() {
    return true;
  }

  public bool AddTable(IMSTable Table) {
    Tables.Add(Table);
    return true;
  }

  public bool Save(IMSTable table) {
    throw new NotImplementedException();
  }

  public bool Save(IMSTable table, IMSRecord record) {
    throw new NotImplementedException();
  }

  public RECORD Get<RECORD>(string table, string key)
    where RECORD : IMSRecord {
    throw new NotImplementedException();
  }
}
