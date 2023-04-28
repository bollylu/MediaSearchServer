using MediaSearch.Models2.Support.Filter;

namespace MediaSearch.Models2.Serialization.Test;

[TestClass]
public class TFilterSerializationTest {

  [TestMethod]
  public void Serialize() {
    Message("Creating a filter");
    TFilter Source = new TFilter() {
      DaysBack = 7,
      GroupOnly = false,
      Keywords = new TMultiItemsSelection(EFilterType.All, "maman", "tous"),
      Tags = new TMultiItemsSelection(EFilterType.Any, "Comédie", "Famille")
    };
    Source.GroupMemberships.Add("Group 1");
    Source.GroupMemberships.Add("Group 2");

    Dump(Source, "Source");

    Message("Serialize to Json");
    string Target = IJson.ToJson(Source);
    Assert.IsNotNull(Target);
    Dump(Target, "Target");

    Assert.IsTrue(Target.Contains("\"Keywords\":\"maman tous\""));
    Assert.IsTrue(Target.Contains("\"KeywordsSelection\":\"All\""));
    Assert.IsTrue(Target.Contains("\"Tags\":\"Comédie Famille\""));
    Assert.IsTrue(Target.Contains("\"TagSelection\":\"Any\""));

    Ok();
  }

  [TestMethod]
  public void Deserialize() {
    Message("Creating a filter and serialize it in Json");
    TFilter Source = new TFilter() {
      DaysBack = 7,
      GroupOnly = false,
      Keywords = new TMultiItemsSelection(EFilterType.All, "maman", "tous"),
      Tags = new TMultiItemsSelection(EFilterType.Any, "Comédie", "Famille")
    };
    Source.GroupMemberships.Add("Group 1");
    Source.GroupMemberships.Add("Group 2");
    string JsonSource = IJson.ToJson(Source);
    Dump(JsonSource, "Source");

    Message("Deserialize Json into a TFilter object");
    IFilter? Target = IJson.FromJson<TFilter>(JsonSource);
    Assert.IsNotNull(Target);

    Dump(Target, "Target");
    Assert.AreEqual(2, Target.GroupMemberships.Count);

    Ok();
  }

}

