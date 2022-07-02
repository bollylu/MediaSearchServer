namespace MediaSearch.Test.Database.Serialization;

[TestClass]
public class TableSerializationTests {

  [ClassInitialize]
  public static async Task ClassInitialize(TestContext context) {
    await MediaSearch.Test.Database.GlobalSettings.Initialize().ConfigureAwait(false);
  }

  [TestMethod]
  public void Table_Serialize() {
    ITable Source = TTableSource.InstanciateRandomTable<IMovie>();
    Dump(Source);

    JsonSerializerOptions Options = new JsonSerializerOptions(IJson.DefaultJsonSerializerOptions);
    Options.Converters.Add(new TTableHeaderJsonConverter());
    Options.Converters.Add(new TMediaSourceJsonConverter());
    Options.Converters.Add(new TTableJsonConverter());

    string Target = IJson.ToJson(Source, Options);
    Assert.IsNotNull(Target);

    Dump(Target);
  }

  [TestMethod]
  public void Table_Deserialize() {

    ITable Source = TTableSource.InstanciateRandomTable<IMovie>();
    Dump(Source);

    JsonSerializerOptions Options = new JsonSerializerOptions(IJson.DefaultJsonSerializerOptions);
    Options.Converters.Add(new TTableHeaderJsonConverter());
    Options.Converters.Add(new TMediaSourceJsonConverter());
    Options.Converters.Add(new TTableJsonConverter());

    string JsonSource = IJson.ToJson(Source, Options);
    Dump(JsonSource);

    ITable? Target = IJson.FromJson<ITable>(JsonSource, Options);

    Assert.IsNotNull(Target);
    Dump(Target);
  }
}