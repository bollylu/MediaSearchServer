using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaSearch.Database.Test;
[TestClass]
public class TableTests {

  [TestMethod]
  public void Instanciate_TMSTable_Empty() {
    IMSTable Target = new TMSTable<IMovie>();
    TraceMessage($"{nameof(Target)} : {Target.GetType().GetGenericName()}", Target);

    Assert.IsNotNull(Target);
  }

  //[TestMethod]
  //public void Instanciate_TMSDatabaseJson_WithName() {
  //  IMSDatabase Target = new TMSDatabaseJson() { RootPath = Path.GetTempPath(), Name="missing.db", Description = "Missing database" };
  //  TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);

  //  Assert.IsFalse(Target.Exists());
  //}

  //[TestMethod]
  //public void TMSDatabaseJson_CreateThenRemove() {
  //  IMSDatabase Target = new TMSDatabaseJson() { RootPath = Path.GetTempPath(), Name = "missing.db" };
  //  TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);

  //  Assert.IsFalse(Target.Exists());
  //  Assert.IsTrue(Target.Create());
  //  Assert.IsTrue(Target.Exists());
  //  Assert.IsTrue(Target.Remove());
  //  Assert.IsFalse(Target.Exists());
  //}
}