using BLTools.Text;

namespace MediaSearch.Database;

public partial class TDatabaseJson : ADatabase, ILoggable {

  /// <summary>
  /// The logger for this class
  /// </summary>
  public override ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TDatabaseJson>();

  /// <summary>
  /// The root path of the database, i.e. the path to the database, without the database name
  /// </summary>
  public string RootPath { get; set; } = ".\\";

  /// <summary>
  /// The full name of the database, including rootpath and name
  /// </summary>
  public string DatabaseFullName => Path.Join(RootPath, Name);

  /// <summary>
  /// The Serializer options for the database. Use it to specify :
  /// <list type="bullet">
  /// <item>JsonConverters</item>
  /// <item>JsonEncoder</item>
  /// <item>...</item>
  /// </list>
  /// </summary>
  public JsonSerializerOptions SerializerOptions {
    get {
      return _SerializerOptions ??= new JsonSerializerOptions(IJson.DefaultJsonSerializerOptions);
    }
  }
  private JsonSerializerOptions? _SerializerOptions;

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new();
    RetVal.AppendIndent($"- {nameof(Name)} = {Name.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(Description)} = {Description.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(RootPath)} = {RootPath.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(DatabaseFullName)} = {DatabaseFullName.WithQuotes()}", indent);

    if (Schema.GetAllTables().Any()) {
      RetVal.AppendIndent($"- {nameof(Schema)}", indent);
      foreach (ITable TableItem in Schema.GetAllTables()) {
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
  /// <summary>
  /// A new anonymous TDatabaseJson
  /// </summary>
  public TDatabaseJson() {
    SerializerOptions.Converters.Add(new TTableHeaderJsonConverter());
    SerializerOptions.Converters.Add(new TTableJsonConverter());
    SerializerOptions.Converters.Add(new TSchemaJsonConverter());
  }

  /// <summary>
  /// A new named TDatabaseJson
  /// </summary>
  /// <param name="rootPath">The location of the database, without its name</param>
  /// <param name="name">The name of the database</param>
  public TDatabaseJson(string rootPath, string name) : this() {
    RootPath = rootPath;
    Name = name;
  }

  /// <summary>
  /// Cleanup of the database
  /// </summary>
  public override void Dispose() {
    if (IsOpened) {
      Close();
    }
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  /// <inheritdoc/>
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
