using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaSearch.Database.Test;
[TestClass]
public class DatabaseMemoryTests {
  
  [TestMethod]
  public void Instanciate_TMSDatabaseMemory_Empty() {
    IMSDatabase Target = new TMSDatabaseMemory();
    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);

    Assert.IsNotNull(Target);
  }

  [TestMethod]
  public void Instanciate_TMSDatabaseMemory_WithName() {
    IMSDatabase Target = new TMSDatabaseMemory() { Name="missing.db", Description = "Missing database" };
    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);

    Assert.IsTrue(Target.Exists());
  }

  [TestMethod]
  public void TMSDatabase_CreateThenRemove() {
    IMSDatabase Target = new TMSDatabaseMemory() {Name = "missing.db" };
    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);

    Assert.IsTrue(Target.Exists());
    Assert.IsTrue(Target.Create());
    Assert.IsTrue(Target.Exists());
    Assert.IsTrue(Target.Remove());
    Assert.IsTrue(Target.Exists());
  }
}