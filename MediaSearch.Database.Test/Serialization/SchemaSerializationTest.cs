namespace MediaSearch.Test.Database.Serialization;

[TestClass]
public class SchemaSerializationTests {

  //[ClassInitialize]
  //public static async Task ClassInitialize(TestContext context) {
  //  await MediaSearch.Test.Database.GlobalSettings.Initialize().ConfigureAwait(false);
  //}

  [TestMethod]
  public void Schema_Serialize() {
    IDatabase Database = TDatabaseSource.CreateJsonTestDatabase();
    Dump(Database);

    _ = TTableSource.CreateTestTable<IMovie>(Database, "Theatre");

    JsonSerializerOptions Options = new JsonSerializerOptions(IJson.DefaultJsonSerializerOptions);
    Options.Converters.Add(new TTableHeaderJsonConverter());
    Options.Converters.Add(new TMediaSourceJsonConverter());
    Options.Converters.Add(new TTableJsonConverter());
    Options.Converters.Add(new TSchemaJsonConverter());

    Database.Open();
    string Target = IJson.ToJson(Database.Schema, Options);
    Assert.IsNotNull(Target);

    Dump(Target);

    Message("Cleanup");
    Assert.IsTrue(Database.Remove());

    Ok();
  }

  [TestMethod]
  public void Schema_Deserialize() {

    Ok();
  }
}