namespace MediaSearch.Test.Database.Serialization;

[TestClass]
public class TableHeaderSerializationTests {

  [ClassInitialize]
  public static async Task ClassInitialize(TestContext context) {
    await MediaSearch.Test.Database.GlobalSettings.Initialize().ConfigureAwait(false);
  }

  [TestMethod]
  public void TableHeader_Serialize() {
    ITableHeader Source = new TMSTableHeaderMovie {
      Name = "Test name",
      Description = "All my movies"
    };

    IMediaSource MediaSource = new TMediaSourceMovie("\\\\Server\\path");

    Source.SetMediaSource(MediaSource);

    Dump(Source);

    JsonSerializerOptions Options = new JsonSerializerOptions(IJson.DefaultJsonSerializerOptions);
    Options.Converters.Add(new TTableHeaderJsonConverter());
    Options.Converters.Add(new TMediaSourceJsonConverter());

    string Target = IJson.ToJson(Source, Options);
    Assert.IsNotNull(Target);

    Dump(Target);
  }

  [TestMethod]
  public void TableHeader_Deserialize() {
    ITableHeader Header = new TMSTableHeaderMovie {
      Name = "Test name",
      Description = "All my movies"
    };

    IMediaSource MediaSource = new TMediaSourceMovie("\\\\Server\\path");
    Header.SetMediaSource(MediaSource);

    Dump(Header);

    JsonSerializerOptions Options = new JsonSerializerOptions(IJson.DefaultJsonSerializerOptions);
    Options.Converters.Add(new TTableHeaderJsonConverter());
    Options.Converters.Add(new TMediaSourceJsonConverter());

    string Source = IJson.ToJson(Header, Options);
    Assert.IsNotNull(Source);

    Dump(Source);

    ITableHeader? Target = IJson.FromJson<ITableHeader>(Source, Options);

    Assert.IsNotNull(Target);
    Dump(Target);
  }
}