using BLTools.Text;

namespace MediaSearch.Database;

public partial class TMSDatabaseJson : AMSDatabase, IMediaSearchLoggable<TMSDatabaseJson> {

  public override IMediaSearchLogger<TMSDatabaseJson> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMSDatabaseJson>();

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

  public override string Dump() {
    StringBuilder RetVal = new(base.Dump());
    IEnumerable<string> TableList = Directory.EnumerateDirectories(DatabaseFullName);
    RetVal.AppendLine(".");
    RetVal.AppendLine($"=== Tables raw directories : {TableList.Count()} item(s) ===".AlignedCenter(GlobalSettings.DEBUG_BOX_WIDTH - 4));
    RetVal.AppendLine(".");
    foreach (string TableDirectoryItem in TableList) {
      IEnumerable<string> RecordList = Directory.EnumerateFiles(TableDirectoryItem);
      RetVal.AppendLine($"==> {TableDirectoryItem.WithQuotes()} : {RecordList.Count()} item(s)");
      RetVal.AppendLine(".");
      foreach (string RecordItem in RecordList) {
        string RawContent = File.ReadAllText(RecordItem);
        RetVal.AppendLine(TextBox.BuildFixedWidth(RawContent, RecordItem, GlobalSettings.DEBUG_BOX_WIDTH - 4, TextBox.EStringAlignment.Left));
      }
      RetVal.AppendLine("==================================");
    }
    return RetVal.ToString();
  }
}
