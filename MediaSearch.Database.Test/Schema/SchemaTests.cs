namespace MediaSearch.Test.Database.Schema;

[TestClass]
public class SchemaTests {

  [TestMethod]
  public void InstanciateSchema() {
    ISchema Target = new TSchema();
    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void BuildSchemaFromEmptyDatabase() {

    IDatabase Database = TDatabaseSource.CreateJsonTestDatabaseEmpty();
    Assert.IsTrue(Database.SchemaBuild());

    Dump(Database);
    Dump(Database.Schema);

    Assert.IsTrue(Database.Remove());
    Ok();
  }

  [TestMethod]
  public void BuildSchemaFromDatabase() {

    IDatabase Database = TDatabaseSource.CreateJsonTestDatabaseWithOneTable();

    Message("Building the schema");
    Assert.IsTrue(Database.SchemaBuild());

    Dump(Database);
    Dump(Database.Schema);

    Assert.IsTrue(Database.Remove());
    Ok();
  }

  [TestMethod]
  public void SaveSchemaFromDatabase() {

    TDatabaseJson Database = TDatabaseSource.CreateJsonTestDatabaseWithOneTable();

    Message("Building the schema");
    Assert.IsTrue(Database.SchemaBuild());

    Dump(Database);
    Dump(Database.Schema);

    Assert.IsTrue(Database.Open());
    Assert.IsTrue(Database.SchemaSave());

    string RawSchemaContent = File.ReadAllText(Path.Join(Database.DatabaseFullName, TDatabaseJson.DATABASE_SCHEMA_NAME));
    MessageBox("Raw schema", RawSchemaContent);

    Assert.IsTrue(Database.Remove());
    Ok();
  }
}
