
namespace MediaSearch.Database.Test;
[TestClass]
public class DatabaseJsonTests {

  [ClassInitialize]
  public static void ClassInitialize(TestContext context) {
    //GlobalSettings.LoggerPool.AddLogger(new TConsoleLogger());
    //GlobalSettings.Initialize().Wait();
    //Models.GlobalSettings.LoggerPool.SetDefaultLogger(new TConsoleLogger());
    //Models.GlobalSettings.Initialize().Wait();
    //Database.GlobalSettings.LoggerPool.AddLogger(new TConsoleLogger<TMSDatabaseJson>() { SeverityLimit = ESeverity.Debug });
    //Database.GlobalSettings.LoggerPool.AddLogger(new TConsoleLogger<TMSTable>() { SeverityLimit = ESeverity.Debug });
    //Database.GlobalSettings.Initialize().Wait();
  }

  [TestMethod]
  public void Instanciate_TMSDatabaseJson_Empty() {
    IMSDatabase Target = new TMSDatabaseJson();
    Dump(Target);

    Assert.IsNotNull(Target);
    Ok();
  }

  [TestMethod]
  public void Instanciate_TMSDatabaseJson_WithName() {
    IMSDatabase Target = new TMSDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}", Description = "Missing database" };
    Dump(Target);

    Assert.IsFalse(Target.Exists());
    Message("Table does not exist");
    Ok();
  }

  [TestMethod]
  public void TMSDatabaseJson_CreateThenRemove() {
    IMSDatabase Target = new TMSDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };
    Dump(Target);

    Assert.IsFalse(Target.Exists());
    Message("Table does not exist and needs creation");
    Assert.IsTrue(Target.Create());
    Message("Table is created");
    Assert.IsTrue(Target.Exists());
    Message("Table exists");
    Assert.IsTrue(Target.Remove());
    Message("Table is removed");
    Assert.IsFalse(Target.Exists());
    Message("Table does not exist");
    Ok();
  }

  [TestMethod]
  public void TMSDatabaseJson_CreateTable() {
    IMSDatabase Target = new TMSDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };
    IMSTable<IMovie> MovieTable = new TMSTable<IMovie>() { Name = "Movies" };

    Message("Creating database Json");
    Assert.IsTrue(Target.Create());
    Assert.IsTrue(Target.Exists());
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
    Dump(Target);
  }
}