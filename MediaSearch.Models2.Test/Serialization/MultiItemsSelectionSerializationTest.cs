namespace MediaSearch.Models2.Serialization.Test;

[TestClass]
public class MultiItemsSelectionSerializationTest {
  [ClassInitialize]
  public static void ClassInitialize(TestContext context) {
    IJson.AddJsonConverter(new TMultiItemsSelectionJsonConverter());
  }

  [TestMethod]
  public void Serialize() {
    Message("Instanciate source");
    TMultiItemsSelection Source = new TMultiItemsSelection(EFilterType.All, "first", "second");
    Dump(Source);
    Message("Serialize to Json");
    string Target = IJson.ToJson(Source);
    Assert.IsNotNull(Target);
    Dump(Target);
    Assert.IsTrue(Target.Contains("[\"second\",\"first\"]"));
    Ok();
  }

  [TestMethod]
  public void Deserialize() {
    Message("Instanciate source");
    TMultiItemsSelection Source = new TMultiItemsSelection(EFilterType.All, "first", "second");
    Dump(Source);

    Message("Serialize to Json");
    string SourceJson = IJson.ToJson(Source);
    Assert.IsNotNull(SourceJson);
    Dump(SourceJson);

    Message("Deserialize back to TMultiItemsSelection");
    TMultiItemsSelection? Target = IJson.FromJson<TMultiItemsSelection>(SourceJson);
    Assert.IsNotNull(Target);
    Assert.AreEqual(2, Target.Items.Count);
    Assert.AreEqual(EFilterType.All, Target.Selection);
    Dump(Target);

    Ok();
  }
}
