using BLTools.Text;

namespace MediaSearch.Database;

public partial class TDatabaseJson : ADatabase, ILoggable {

  public override ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TDatabaseJson>();

  public string RootPath { get; set; } = ".\\";

  /// <summary>
  /// The full name of the database, including rootpath and name
  /// </summary>
  public string DatabaseFullName {
    get {
      return Path.Join(RootPath, Name);
    }
  }

  public JsonSerializerOptions SerializerOptions { get; } = new JsonSerializerOptions(IJson.DefaultJsonSerializerOptions);

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new();
    RetVal.AppendIndent($"- {nameof(Name)} = {Name.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(Description)} = {Description.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(RootPath)} = {RootPath.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(DatabaseFullName)} = {DatabaseFullName.WithQuotes()}", indent);

    if (Schema.GetAll().Any()) {
      RetVal.AppendIndent($"- {nameof(Schema)}", indent);
      foreach (ITable TableItem in Schema.GetAll()) {
        RetVal.AppendIndent($"- {TableItem.Name.WithQuotes()}", indent + 2);
        RetVal.AppendIndent(TableItem.ToString(indent), indent + 4);
      }
    } else {
      RetVal.AppendIndent("- Schema is empty", indent);
    }
    RetVal.AppendIndent($"- {nameof(IsOpened)} : {IsOpened}", indent);
    RetVal.AppendIndent($"- {nameof(Exists)} : {Exists()}", indent);

    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TDatabaseJson() {
    SerializerOptions.Converters.Add(new TTableHeaderJsonConverter());
  }

  public TDatabaseJson(string rootPath, string name) : this() {
    RootPath = rootPath;
    Name = name;
  }

  public override void Dispose() {
    if (IsOpened) {
      Close();
    }

    if (Schema.GetAll().Any()) {
      foreach (ITable TableItem in Schema.GetAll()) {
        TableItem.Dispose();
      }
    }
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override string Dump() {
    StringBuilder RetVal = new(base.Dump());
    RetVal.AppendLine($"{nameof(DatabaseFullName)} = {DatabaseFullName.WithQuotes()}");
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
