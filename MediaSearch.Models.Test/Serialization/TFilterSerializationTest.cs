namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TFilterSerializationTest {

  [TestMethod]
  public void Serialize() {
    Message("Creating a filter");
    IFilter Source = new TFilter() {
      DaysBack = 7,
      GroupOnly = false,
      Keywords = "maman tous",
      KeywordsSelection = EFilterType.All,
      Tags = "Comédie Famille",
      TagSelection = EFilterType.Any
    };
    Source.GroupMemberships.Add("Group 1");
    Source.GroupMemberships.Add("Group 2");

    Dump(Source, "Source");

    Message("Serialize to Json");
    string Target = Source.ToJson();
    Assert.IsNotNull(Target);
    Dump(Target, "Target");

    Assert.IsTrue(Target.Contains("\"Keywords\": \"maman tous\""));
    Assert.IsTrue(Target.Contains("\"KeywordsSelection\": \"All\""));
    Assert.IsTrue(Target.Contains("\"Tags\": \"Comédie Famille\""));
    Assert.IsTrue(Target.Contains("\"TagSelection\": \"Any\""));

    Ok();
  }

  [TestMethod]
  public void Deserialize() {
    Message("Creating a filter and serialize it in Json");
    IFilter Source = new TFilter() {
      DaysBack = 7,
      GroupOnly = false,
      Keywords = "maman tous",
      KeywordsSelection = EFilterType.All,
      Tags = "Comédie Famille",
      TagSelection = EFilterType.Any
    };
    Source.GroupMemberships.Add("Group 1");
    Source.GroupMemberships.Add("Group 2");
    string JsonSource = Source.ToJson();
    Dump(JsonSource, "Source");

    Message("Deserialize Json into a TFilter object");
    IFilter? Target = IJson<TFilter>.FromJson(JsonSource);
    Assert.IsNotNull(Target);

    Dump(Target, "Target");
    Assert.AreEqual(2, Target.GroupMemberships.Count);

    Ok();
  }

}

