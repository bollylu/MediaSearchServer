using MediaSearch.Models.Logging;
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

    TraceBox($"{nameof(Source)} : {Source.GetType().Name}", Source);

    JsonSerializerOptions Options = new JsonSerializerOptions(IJson.DefaultJsonSerializerOptions);
    Options.Converters.Add(new TMSTableHeaderJsonConverter());
    Options.Converters.Add(new TMediaSourceJsonConverter());

    string Target = Source.ToJson(Options);
    Assert.IsNotNull(Target);

    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
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

    TraceBox($"{nameof(Header)} : {Header.GetType().Name}", Header);

    JsonSerializerOptions Options = new JsonSerializerOptions(IJson.DefaultJsonSerializerOptions);
    Options.Converters.Add(new TMSTableHeaderJsonConverter());
    Options.Converters.Add(new TMediaSourceJsonConverter());

    string Source = Header.ToJson(Options);
    Assert.IsNotNull(Source);

    TraceBox($"{nameof(Source)} : {Source.GetType().Name}", Source);

    IMSTableHeader? Target = IJson<TMSTableHeader>.FromJson(Source, Options);
    ;
    Assert.IsNotNull(Target);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }
}