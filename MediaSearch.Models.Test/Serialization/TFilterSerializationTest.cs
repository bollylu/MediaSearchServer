using BLTools.Text;

namespace MediaSearch.Models.Test;

[TestClass]
public class TFilterSerializationTest {

  [ClassInitialize]
  public static async Task ClassInitialize(TestContext context) {
    await MediaSearch.Models.GlobalSettings.Initialize().ConfigureAwait(false);
  }

  [TestMethod]
  public void SerializeFilterWithConverters() {
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

    Console.WriteLine(Source.ToString().BoxFixedWidth("Source", GlobalSettings.DEBUG_BOX_WIDTH));

    string Target = Source.ToJson();
    Console.WriteLine(Target.BoxFixedWidth("Serialized", GlobalSettings.DEBUG_BOX_WIDTH));
    Assert.IsTrue(Target.Contains("\"Keywords\": \"maman tous\""));
    Assert.IsTrue(Target.Contains("\"KeywordsSelection\": \"All\""));
    Assert.IsTrue(Target.Contains("\"Tags\": \"Com\\u00E9die Famille\""));
    Assert.IsTrue(Target.Contains("\"TagSelection\": \"Any\""));
    Assert.IsNotNull(Target);
  }

  [TestMethod]
  public void DeserializeFilterWithConverters() {
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
    Console.WriteLine(JsonSource.BoxFixedWidth("Source", GlobalSettings.DEBUG_BOX_WIDTH));

    IFilter? Target = TFilter.FromJson(JsonSource);
    Console.WriteLine(Target?.ToString().BoxFixedWidth("Deserialized", GlobalSettings.DEBUG_BOX_WIDTH));

    Assert.IsNotNull(Target);
    Assert.AreEqual(2, Target.GroupMemberships.Count);
  }

}

