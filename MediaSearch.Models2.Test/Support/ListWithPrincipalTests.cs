namespace MediaSearch.Models2.Test.Support;
[TestClass]
public class ListWithPrincipalTests {

  [TestMethod]
  public void InstanciateListWithPrincipal() {
    Message("Instanciate a ListWithPrincipal");
    IListWithPrincipal<int> Target = new TListWithPrincipal<int>();
    Assert.IsNotNull(Target);

    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void InstanciateListWithPrincipal_AddItems() {
    Message("Instanciate a ListWithPrincipal");
    IListWithPrincipal<int> Target = new TListWithPrincipal<int>();
    Assert.IsNotNull(Target);

    Target.Add(10);
    Target.Add(20);
    Target.Add(30);
    Target.Add(0);
    Target.Add(150);

    Dump(Target);

    MessageBox("No principal set", Target.ToString() ?? "");

    Ok();
  }

  [TestMethod]
  public void InstanciateListWithPrincipal_SetPrincipal() {
    Message("Instanciate a ListWithPrincipal");
    IListWithPrincipal<int> Target = new TListWithPrincipal<int>();
    Assert.IsNotNull(Target);
    Target.Add(10);
    Target.Add(20);
    Target.Add(30);
    Target.Add(0);
    Target.Add(150);
    Dump(Target);

    Assert.AreNotEqual(TListWithPrincipal.NO_PRINCIPAL, Target.SetPrincipal(30));
    MessageBox("Principal is 30", Target.ToString() ?? "");

    Assert.AreEqual(30, Target.GetPrincipal());
    Message($"GetPrincipal() returns 30 : {Target.GetPrincipal()}");

    Ok();
  }

  [TestMethod]
  public void InstanciateListWithPrincipal_SetPrincipalWrongValue() {
    Message("Instanciate a ListWithPrincipal");
    IListWithPrincipal<int> Target = new TListWithPrincipal<int>();
    Assert.IsNotNull(Target);
    Target.Add(10);
    Target.Add(20);
    Target.Add(30);
    Target.Add(0);
    Target.Add(150);
    Dump(Target);

    Assert.AreEqual(TListWithPrincipal.NO_PRINCIPAL, Target.SetPrincipal(35));
    MessageBox("Principal not set", Target.ToString() ?? "");

    Message($"GetPrincipal() returns first value : {Target.GetPrincipal()}");
    Assert.AreEqual(10, Target.GetPrincipal());

    Ok();
  }

  [TestMethod]
  public void InstanciateListWithPrincipal_GetPrincipalFromEmptyList() {
    Message("Instanciate a ListWithPrincipal");
    IListWithPrincipal<int> Target = new TListWithPrincipal<int>();
    Assert.IsNotNull(Target);
    Dump(Target);

    Message("Calling GetPrincipal() throws and InvalidOperationException");
    Assert.ThrowsException<InvalidOperationException>(() => Target.GetPrincipal());

    Ok();
  }
}
