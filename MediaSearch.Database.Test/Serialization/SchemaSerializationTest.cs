namespace MediaSearch.Test.Database.Serialization;

[TestClass]
public class SchemaSerializationTests {

  [TestMethod]
  public void Schema_Serialize() {
    const int TableCount = 3;
    Message($"Creating a database with {TableCount} tables");
    TDatabaseJson Database = TDatabaseSource.CreateJsonTestDatabaseWithMultipleTables(TableCount, 2);

    Message("Opening database");
    Assert.IsTrue(Database.Open());

    string Target = IJson.ToJson(Database.Schema, Database.SerializerOptions);
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