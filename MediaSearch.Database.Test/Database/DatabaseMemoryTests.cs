namespace MediaSearch.Test.Database;

[TestClass]
public class DatabaseMemoryTests {

  [TestMethod]
  public void Instanciate_TMSDatabaseMemory_Empty() {
    IDatabase Target = new TDatabaseMemory();
    Dump(Target);

    Assert.IsNotNull(Target);
  }

  [TestMethod]
  public void Instanciate_TMSDatabaseMemory_WithName() {
    IDatabase Target = new TDatabaseMemory() { Name = "missing.db", Description = "Missing database" };
    Dump(Target);

    Assert.IsTrue(Target.Exists());
  }

  [TestMethod]
  public void TMSDatabase_CreateThenRemove() {
    IDatabase Target = new TDatabaseMemory() { Name = "missing.db" };
    Dump(Target);

    Assert.IsTrue(Target.Exists());
    Assert.IsTrue(Target.Create());
    Assert.IsTrue(Target.Exists());
    Assert.IsTrue(Target.Remove());
    Assert.IsTrue(Target.Exists());
  }
}