
namespace MediaSearch.Test.Database;
[TestClass]
public class DatabaseJsonTests {

  [ClassInitialize]
  public static void ClassInitialize(TestContext context) {

  }

  [TestMethod]
  public void Instanciate_TMSDatabaseJson_Empty() {
    IDatabase Target = new TDatabaseJson();
    Dump(Target);

    Assert.IsNotNull(Target);
    Ok();
  }

  [TestMethod]
  public void Instanciate_TMSDatabaseJson_WithName() {
    IDatabase Target = new TDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}", Description = "Missing database" };
    Dump(Target);

    Assert.IsFalse(Target.Exists());
    Message("Table does not exist");
    Ok();
  }

  [TestMethod]
  public void TMSDatabaseJson_CreateDatabaseThenRemove() {
    TDatabaseJson Target = new TDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };
    Dump(Target);

    Assert.IsFalse(Target.Exists());
    Message("Database does not exist and needs creation");

    Assert.IsTrue(Target.Create());
    Message("Database is created");

    DumpWithMessage("Database schema", Target.Schema);

    Assert.IsTrue(Target.Exists());
    Message("Database exists");

    Assert.IsTrue(Target.Remove());
    Message("Database is removed");

    Assert.IsFalse(Target.Exists());
    Message("Database does not exist");

    Ok();
  }

  [TestMethod]
  public void TMSDatabaseJson_CreateDatabase_AddTable() {
    TDatabaseJson Target = new TDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };
    ITable MovieTable = new TTableMovie() { Name = "Movies" };

    Message("Creating database Json");
    Assert.IsTrue(Target.Create());

    Assert.IsTrue(Target.Exists());
    Message("Database exists");

    Message("Opening database Json");
    Assert.IsTrue(Target.Open());
    Dump(Target);

    Message("Adding table");
    Assert.IsTrue(Target.TableCreate(MovieTable));
    Dump(Target);

    Message("Checking table");
    Assert.IsTrue(Target.TableCheck(MovieTable));

    Message("Closing database Json");
    Assert.IsTrue(Target.Close());
    Dump(Target);

    Message("Opening database Json");
    Assert.IsTrue(Target.Open());
    Dump(Target);

    Message("Checking table");
    Assert.IsTrue(Target.TableCheck(MovieTable));

    Message("Closing database Json");
    Assert.IsTrue(Target.Close());

    Message("Removing database Json");
    Assert.IsTrue(Target.Remove());

    Assert.IsFalse(Target.Exists());
    Message("Database does not exist");

    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void TMSDatabaseJson_CreateDatabase_AddTables() {
    TDatabaseJson Target = new TDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };
    Target.Logger.SeverityLimit = ESeverity.Debug;

    ITable MovieTable = new TTableMovie() { Name = "Movies" };
    ITable TheatreTable = new TTableMovie() { Name = "Theatre" };

    Message("Creating database Json");
    Assert.IsTrue(Target.Create());

    Assert.IsTrue(Target.Exists());
    Message("Database exists");

    Message("Opening database Json");
    Assert.IsTrue(Target.Open());
    Dump(Target);

    Message($"Adding table {nameof(MovieTable)}");
    Assert.IsTrue(Target.TableCreate(MovieTable));

    Message($"Adding table {nameof(TheatreTable)}");
    Assert.IsTrue(Target.TableCreate(TheatreTable));

    Dump(Target);

    DumpWithMessage("Schema", Target.Schema);

    Message("Checking table");
    Assert.IsTrue(Target.TableCheck(MovieTable));

    Message("Closing database Json");
    Assert.IsTrue(Target.Close());
    Dump(Target);

    Message("Opening database Json");
    Assert.IsTrue(Target.Open());
    Dump(Target);

    Message("Checking table");
    Assert.IsTrue(Target.TableCheck(MovieTable));

    Message("Closing database Json");
    Assert.IsTrue(Target.Close());

    Message("Removing database Json");
    Assert.IsTrue(Target.Remove());

    Assert.IsFalse(Target.Exists());
    Message("Database does not exist");

    Dump(Target);

    Ok();
  }
}