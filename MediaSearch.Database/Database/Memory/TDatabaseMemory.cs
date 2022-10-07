namespace MediaSearch.Database;

public partial class TDatabaseMemory : ADatabase, ILoggable {

  /// <summary>
  /// The logger for this class
  /// </summary>
  public override ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TDatabaseMemory>();

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new();
    RetVal.AppendIndent($"- {nameof(Name)} = {Name.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(Description)} = {Description.WithQuotes()}", indent);
    if (Schema.GetAllTables().Any()) {
      RetVal.AppendIndent($"- {nameof(Schema)}", indent);
      foreach (ITable TableItem in Schema.GetAllTables()) {
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
  }
}
