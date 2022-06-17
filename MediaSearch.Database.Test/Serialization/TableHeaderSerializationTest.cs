namespace MediaSearch.Test.Database;

[TestClass]
public class TableHeaderSerializationTests {

  [ClassInitialize]
  public static async Task ClassInitialize(TestContext context) {
    await MediaSearch.Test.Database.GlobalSettings.Initialize().ConfigureAwait(false);
  }

  [TestMethod]
  public void TableHeader_Serialize() {
    IMSTableHeader Source = new TMSTableHeaderMovie {
      Name = "Test name",
      Description = "All my movies"
    };

    IMediaSource MediaSource = new TMediaSourceMovie("\\\\Server\\path");

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
    IMSTableHeader Header = new TMSTableHeaderMovie {
      Name = "Test name",
      Description = "All my movies"
    };

    IMediaSource MediaSource = new TMediaSourceMovie("\\\\Server\\path");
    Header.SetMediaSource(MediaSource);

    Dump(Header);

    JsonSerializerOptions Options = new JsonSerializerOptions(IJson.DefaultJsonSerializerOptions);
    Options.Converters.Add(new TMSTableHeaderJsonConverter());
    Options.Converters.Add(new TMediaSourceJsonConverter());

    string Source = IJson.ToJson(Header, Options);
    Assert.IsNotNull(Source);

    Dump(Source);

    IMSTableHeader? Target = IJson.FromJson<IMSTableHeader>(Source, Options);

    Assert.IsNotNull(Target);
    Dump(Target);
  }
}