namespace MediaSearch.Models2.Test.Serialization;

[TestClass]
public class MultiItemsSelectionSerializationTest {

  private const string FIRST_ITEM = "first";
  private const string SECOND_ITEM = "second";

  [TestMethod]
  public void Serialize() {
    Message("Instanciate source");
    TMultiItemsSelection Source = new TMultiItemsSelection(EFilterType.All, FIRST_ITEM, SECOND_ITEM);
    Dump(Source);
    Message("Serialize to Json");
    string Target = IJson.ToJson(Source);
    Assert.IsNotNull(Target);
    Dump(Target);

    JsonDocument JsonTarget = JsonDocument.Parse(Target);
    IEnumerable<string> TestTarget = JsonTarget.GetJsonProperties();

    Dump(TestTarget);

    Assert.IsTrue(TestTarget.Contains(FIRST_ITEM.WithQuotes()));
    Assert.IsTrue(TestTarget.Contains(SECOND_ITEM.WithQuotes()));
    Ok();
  }

  [TestMethod]
  public void Deserialize() {
    Message("Instanciate source");
    TMultiItemsSelection Source = new TMultiItemsSelection(EFilterType.All, FIRST_ITEM, SECOND_ITEM);
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
