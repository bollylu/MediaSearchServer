namespace MediaSearch.Database;

public partial class TMSDatabaseMemory : IMSDatabase, IMediaSearchLoggable<TMSDatabaseMemory> {

  public IMediaSearchLogger<TMSDatabaseMemory> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMSDatabaseMemory>();

  public string Name { get; set; } = "";

  public string Description { get; set; } = "";

  public string DatabaseFullName {
    get {
      return Name;
    }
  }

  public bool IsOpened { get; private set; }

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
    } else {
      RetVal.AppendIndent("- No table available", indent);
    }
    RetVal.AppendIndent($"- {nameof(IsOpened)} : {IsOpened}");
    RetVal.AppendIndent($"- {nameof(Exists)} : {Exists()}", indent);
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public void Dispose() {
    if (IsOpened) {
      Close();
    }

    if (Tables.Any()) {
      foreach (IMSTable TableItem in Tables) {
        TableItem.Dispose();
      }
    }
  }
}
