using BLTools.Text;

namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TFilterSerializationTest {

  [TestMethod]
  public void Serialize() {
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

    TraceBox("Source", Source);

    string Target = Source.ToJson();
    Assert.IsNotNull(Target);
    TraceBox("Target", Target);

    Assert.IsTrue(Target.Contains("\"Keywords\": \"maman tous\""));
    Assert.IsTrue(Target.Contains("\"KeywordsSelection\": \"All\""));
    Assert.IsTrue(Target.Contains("\"Tags\": \"Comédie Famille\""));
    Assert.IsTrue(Target.Contains("\"TagSelection\": \"Any\""));
    
  }

  [TestMethod]
  public void Deserialize() {
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
    TraceBox("Source", JsonSource);

    IFilter? Target = IJson<TFilter>.FromJson(JsonSource);
    Assert.IsNotNull(Target);

    TraceBox("Target", Target);
    Assert.AreEqual(2, Target.GroupMemberships.Count);
  }

}

