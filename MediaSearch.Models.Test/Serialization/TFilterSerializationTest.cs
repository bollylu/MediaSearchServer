using BLTools.Text;

namespace MediaSearch.Models.Test;

[TestClass]
public class TFilterSerializationTest {

  [TestMethod]
  public void SerializeFilterWithConverters() {
    TFilter Source = new TFilter() {
      DaysBack = 7,
      GroupOnly = false,
      Keywords = "maman tous",
      KeywordsSelection = EFilterType.All,
      Tags = "Comédie Famille",
      TagSelection = EFilterType.Any
    };
    Source.GroupMemberships.Add("Group 1");
    Source.GroupMemberships.Add("Group 2");
    string Target = Source.ToJson();
    Console.WriteLine(Target);
    Assert.IsTrue(Target.Contains("\"Keywords\": \"maman tous\""));
    Assert.IsTrue(Target.Contains("\"KeywordsSelection\": \"All\""));
    Assert.IsTrue(Target.Contains("\"Tags\": \"Com\\u00E9die Famille\""));
    Assert.IsTrue(Target.Contains("\"TagSelection\": \"Any\""));
    Assert.IsNotNull(Target);
  }

  [TestMethod]
  public void DeserializeFilterWithConverters() {
    TFilter Source = new TFilter() {
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

    IFilter? Target = TFilter.FromJson(JsonSource);
    Console.WriteLine(Target?.ToString().BoxFixedWidth(120));

    Assert.IsNotNull(Target);
    Assert.AreEqual(2, Target.GroupMemberships.Count);
  }

}

