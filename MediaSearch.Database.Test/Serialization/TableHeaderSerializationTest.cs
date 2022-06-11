namespace MediaSearch.Database.Test;

[TestClass]
public class TableHeaderSerializationTests {

  //[ClassInitialize]
  //public static void ClassInitialize(TestContext context) {
  //  GlobalSettings.LoggerPool.AddLogger(new TMediaSearchLoggerConsole());
  //  Models.GlobalSettings.LoggerPool.AddDefaultLogger(new TMediaSearchLoggerConsole());
  //  Database.GlobalSettings.LoggerPool.AddLogger(new TMediaSearchLoggerConsole<TMSTable>() { SeverityLimit = ESeverity.Debug });
  //}

  [TestMethod]
  public void TableHeader_Serialize() {
    IMSTableHeader Source = new TMSTableHeader();
    Source.Name = "Test name";
    Source.Description = "All my movies";

    IMediaSource MediaSource = new TMediaSource<IMedia>() {
      RootStorage = "\\\\Server\\path"
    };

    Source.SetMediaSource(MediaSource);

    Dump(Source);

    JsonSerializerOptions Options = new JsonSerializerOptions(IJson.DefaultJsonSerializerOptions);
    Options.Converters.Add(new TMSTableHeaderJsonConverter());
    Options.Converters.Add(new TMediaSourceJsonConverter());

    string Target = IJson.ToJson(Source, Options);
    Assert.IsNotNull(Target);

    Dump(Target);
  }

  [TestMethod]
  public void TableHeader_Deserialize() {
    IMSTableHeader Header = new TMSTableHeader();
    Header.Name = "Test name";
    Header.Description = "All my movies";

    IMediaSource MediaSource = new TMediaSource<IMedia>() {
      RootStorage = "\\\\Server\\path"
    };
    Header.SetMediaSource(MediaSource);

    Dump(Header);

    JsonSerializerOptions Options = new JsonSerializerOptions(IJson.DefaultJsonSerializerOptions);
    Options.Converters.Add(new TMSTableHeaderJsonConverter());
    Options.Converters.Add(new TMediaSourceJsonConverter());

    string Source = IJson.ToJson(Header, Options);
    Assert.IsNotNull(Source);

    Dump(Source);

    IMSTableHeader? Target = IJson.FromJson<TMSTableHeader>(Source, Options);

    Assert.IsNotNull(Target);
    Dump(Target);
  }
}