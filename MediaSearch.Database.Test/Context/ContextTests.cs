namespace MediaSearch.Test.Database;

[TestClass]
public class ContextTests {

  [TestMethod]
  public void Instanciate_TContext_Empty() {
    IDatabase Source = new TDatabaseJson();

    IContext<IRecord> Target = new TContext<IRecord>(Source, "movies");
    Dump(Target);

    Assert.IsNotNull(Target);
  }

  [TestMethod]
  public void Instanciate_TContextOperation_Get() {
    IContextOperation Target = new TContextGet<IRecord>("1234567890");

    Dump(Target);

    Assert.IsNotNull(Target);
  }
}