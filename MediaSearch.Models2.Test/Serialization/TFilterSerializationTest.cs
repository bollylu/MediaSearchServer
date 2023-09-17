using System.Text.RegularExpressions;

namespace MediaSearch.Models2.Test.Serialization;

[TestClass]
public class TFilterSerializationTest {

  private const string KEYWORD_ALL_FIRST = "maman";
  private const string KEYWORD_ALL_SECOND = "tous";

  private const string TAG_ANY_FIRST = "Comédie";
  private const string TAG_ANY_SECOND = "Famille";

  private const string GROUP_FIRST = "Group 1";
  private const string GROUP_SECOND = "Group 2";

  [TestMethod]
  public void Serialize() {
    Message("Creating a filter");
    TFilter Source = new TFilter() {
      DaysBack = 7,
      GroupOnly = false,
      Keywords = new TMultiItemsSelection(EFilterType.All, KEYWORD_ALL_FIRST, KEYWORD_ALL_SECOND),
      Tags = new TMultiItemsSelection(EFilterType.Any, TAG_ANY_FIRST, TAG_ANY_SECOND)
    };
    Source.GroupMemberships.Add(GROUP_FIRST);
    Source.GroupMemberships.Add(GROUP_SECOND);

    Dump(Source);

    Message("Serialize to Json");
    string Target = IJson.ToJson(Source);
    Assert.IsNotNull(Target);
    Dump(Target);

    JsonDocument JsonTarget = JsonDocument.Parse(Target);
    IEnumerable<string> TestTarget = JsonTarget.GetJsonProperties();

    Assert.IsTrue(TestTarget.Skip(3).Take(4).Contains("Selection : All"));
    Assert.IsTrue(TestTarget.Skip(3).Take(4).Contains(KEYWORD_ALL_FIRST.WithQuotes()));
    Assert.IsTrue(TestTarget.Skip(3).Take(4).Contains(KEYWORD_ALL_SECOND.WithQuotes()));

    Assert.IsTrue(TestTarget.Skip(7).Take(4).Contains("Selection : Any"));
    Assert.IsTrue(TestTarget.Skip(7).Take(4).Contains(TAG_ANY_FIRST.WithQuotes()));
    Assert.IsTrue(TestTarget.Skip(7).Take(4).Contains(TAG_ANY_SECOND.WithQuotes()));

    Ok();
  }

  [TestMethod]
  public void Deserialize() {
    Message("Creating a filter and serialize it in Json");
    TFilter Source = new TFilter() {
      DaysBack = 7,
      GroupOnly = false,
      Keywords = new TMultiItemsSelection(EFilterType.All, KEYWORD_ALL_FIRST, KEYWORD_ALL_SECOND),
      Tags = new TMultiItemsSelection(EFilterType.Any, TAG_ANY_FIRST, TAG_ANY_SECOND)
    };
    Source.GroupMemberships.Add(GROUP_FIRST);
    Source.GroupMemberships.Add(GROUP_SECOND);
    string JsonSource = IJson.ToJson(Source);
    Dump(JsonSource);

    Message("Deserialize Json into a TFilter object");
    IFilter? Target = IJson.FromJson<TFilter>(JsonSource);
    Assert.IsNotNull(Target);

    Dump(Target);
    Assert.AreEqual(2, Target.GroupMemberships.Count);

    Ok();
  }

}

