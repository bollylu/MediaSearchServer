namespace MediaSearch.Database;

public partial class TMSDatabaseMemory : AMSDatabase, ILoggable {

  public override ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TMSDatabaseMemory>();

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new();
    RetVal.AppendIndent($"- {nameof(Name)} = {Name.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(Description)} = {Description.WithQuotes()}", indent);
    if (Schema.GetAll().Any()) {
      RetVal.AppendIndent($"- {nameof(Schema)}", indent);
      foreach (IMSTableGeneric TableItem in Schema.GetAll()) {
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

  public override void Dispose() {
    if (IsOpened) {
      Close();
    }

    if (Schema.GetAll().Any()) {
      foreach (IMSTableGeneric TableItem in Schema.GetAll()) {
        TableItem.Dispose();
      }
    }
  }
}
