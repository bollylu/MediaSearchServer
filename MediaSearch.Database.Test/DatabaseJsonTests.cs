using MediaSearch.Models.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaSearch.Database.Test;
[TestClass]
public class DatabaseJsonTests {

  [ClassInitialize]
  public static void ClassInitialize(TestContext context) {
    GlobalSettings.LoggerPool.AddLogger(new TMediaSearchLoggerConsole());
    GlobalSettings.Initialize().Wait();
    Models.GlobalSettings.LoggerPool.AddDefaultLogger(new TMediaSearchLoggerConsole());
    Models.GlobalSettings.Initialize().Wait();
    Database.GlobalSettings.LoggerPool.AddLogger(new TMediaSearchLoggerConsole<TMSDatabaseJson>() { SeverityLimit = ESeverity.Debug });
    Database.GlobalSettings.LoggerPool.AddLogger(new TMediaSearchLoggerConsole<TMSTable>() { SeverityLimit = ESeverity.Debug });
    Database.GlobalSettings.Initialize().Wait();
  }

  [TestMethod]
  public void Instanciate_TMSDatabaseJson_Empty() {
    IMSDatabase Target = new TMSDatabaseJson();
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);

    Assert.IsNotNull(Target);
  }

  [TestMethod]
  public void Instanciate_TMSDatabaseJson_WithName() {
    IMSDatabase Target = new TMSDatabaseJson() { RootPath = Path.GetTempPath(), Name=$"{Random.Shared.Next()}", Description = "Missing database" };
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);

    Assert.IsFalse(Target.Exists());
    TraceMessage("Table does not exist");
  }

  [TestMethod]
  public void TMSDatabaseJson_CreateThenRemove() {
    IMSDatabase Target = new TMSDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);

    Assert.IsFalse(Target.Exists());
    TraceMessage("Table does not exist and needs creation");
    Assert.IsTrue(Target.Create());
    TraceMessage("Table is created");
    Assert.IsTrue(Target.Exists());
    TraceMessage("Table exists");
    Assert.IsTrue(Target.Remove());
    TraceMessage("Table is removed");
    Assert.IsFalse(Target.Exists());
    TraceMessage("Table does not exist");
  }

  [TestMethod]
  public void TMSDatabaseJson_CreateTable() {
    IMSDatabase Target = new TMSDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };
    IMSTable<IMovie> MovieTable = new TMSTable<IMovie>() { Name = "Movies" };

    TraceMessage("Creating database Json");
    Assert.IsTrue(Target.Create());
    Assert.IsTrue(Target.Exists());
    TraceMessage("Opening database Json");
    Assert.IsTrue(Target.Open());
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);

    TraceMessage("Adding table");
    Assert.IsTrue(Target.TableCreate(MovieTable));
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);

    TraceMessage("Checking table");
    Assert.IsTrue(Target.TableCheck(MovieTable));

    TraceMessage("Closing database Json");
    Assert.IsTrue(Target.Close());
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);

    TraceMessage("Opening database Json");
    Assert.IsTrue(Target.Open());
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);

    TraceMessage("Checking table");
    Assert.IsTrue(Target.TableCheck(MovieTable));

    TraceMessage("Closing database Json");
    Assert.IsTrue(Target.Close());

    TraceMessage("Removing database Json");
    Assert.IsTrue(Target.Remove());
    Assert.IsFalse(Target.Exists());
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }
}