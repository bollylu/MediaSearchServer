using BLTools.Text;

using MediaSearch.Server.Services;

namespace MediaSearch.Models.Test;

[TestClass]
public class IEnumerableMediaExtensionsTests {

  /// <summary>
  /// Internal source of data
  /// </summary>
  internal TMSTableJsonMovie Database => _Database ??= new TMSTableJsonMovie("data", "movies");
  private TMSTableJsonMovie? _Database;

  public IEnumerableMediaExtensionsTests() {
    Assert.IsTrue(Database.Exists());
    Assert.IsTrue(Database.OpenOrCreate());
    Assert.IsTrue(Database.Load());
  }

  #region --- Any tag --------------------------------------------
  [TestMethod]
  public void FilterByAnyTag_OneTagOnly_ResultOk() {
    TFilter Filter = new TFilter() { Tags = "science-fiction", TagSelection = EFilterType.Any };
    TraceMessage($"{nameof(Filter)} : {Filter.GetType().Name}", Filter);
    IList<IMedia> Target = Database.GetFiltered(Filter).ToList();
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count > 10);
    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void FilterByAnyTag_OneTagOnlyButMissing_NothingToList() {
    TFilter Filter = new TFilter() { Tags = "xxxtotoxxx", TagSelection = EFilterType.Any };
    TraceMessage($"{nameof(Filter)} : {Filter.GetType().Name}", Filter);
    IEnumerable<IMedia> Target = Database.GetFiltered(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.IsEmpty());
  }

  [TestMethod]
  public void FilterByAnyTag_MultipleTags_ResultOk() {
    TFilter Filter = new TFilter() { Tags = "science-fiction aliens", TagSelection = EFilterType.Any };
    TraceMessage("Filter", Filter);
    IEnumerable<IMedia> Target = Database.GetFiltered(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);
    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }
  #endregion --- Any tag --------------------------------------------

  #region --- All tags --------------------------------------------
  [TestMethod]
  public void FilterByAllTag_OneTagOnly_ResultOk() {
    TFilter Filter = new TFilter() { Tags = "science-fiction", TagSelection = EFilterType.All };
    TraceMessage($"{nameof(Filter)} : {Filter.GetType().Name}", Filter);
    IEnumerable<IMedia> Target = Database.GetFiltered(Filter);
    Assert.IsNotNull(Target);
    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);

  }

  [TestMethod]
  public void FilterByAllTag_OneTagOnlyButMissing_ResultOk() {
    TFilter Filter = new TFilter() { Tags = "xxxtotoxxx", TagSelection = EFilterType.All };
    TraceMessage($"{nameof(Filter)} : {Filter.GetType().Name}", Filter);
    IEnumerable<IMedia> Target = Database.GetFiltered(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.IsEmpty());
    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void FilterByAllTag_MultipleTags_ResultOk() {
    TFilter Filter = new TFilter() { Tags = "science-fiction aliens", TagSelection = EFilterType.All };
    TraceMessage($"{nameof(Filter)} : {Filter.GetType().Name}", Filter);
    IEnumerable<IMedia> Target = Database.GetFiltered(Filter);
    Assert.IsNotNull(Target);
    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);
  }
  #endregion --- All tags --------------------------------------------
}