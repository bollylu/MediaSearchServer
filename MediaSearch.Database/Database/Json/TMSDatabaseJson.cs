namespace MediaSearch.Database;

public partial class TMSDatabaseJson : AMSDatabase, IMediaSearchLoggable<TMSDatabaseJson> {

  public IMediaSearchLogger<TMSDatabaseJson> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMSDatabaseJson>();

  public string RootPath { get; set; } = ".\\";

  public override string DatabaseFullName {
    get {
      return Path.Join(RootPath, Name);
    }
  }

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new();
    RetVal.AppendIndent($"- {nameof(Name)} = {Name.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(Description)} = {Description.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(RootPath)} = {RootPath.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(DatabaseFullName)} = {DatabaseFullName.WithQuotes()}", indent);
    if (Tables.Any()) {
      RetVal.AppendIndent($"- {nameof(Tables)}", indent);
      foreach (IMSTable TableItem in Tables) {
        RetVal.AppendIndent($"- {TableItem.Name.WithQuotes()}", indent + 2);
        RetVal.AppendIndent(TableItem.ToString(indent), indent + 4);
      }
    } else {
      RetVal.AppendIndent("- No table available", indent);
    }
    RetVal.AppendIndent($"- {nameof(IsOpened)} : {IsOpened}", indent);
    RetVal.AppendIndent($"- {nameof(Exists)} : {Exists()}", indent);

    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public override void Dispose() {
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
