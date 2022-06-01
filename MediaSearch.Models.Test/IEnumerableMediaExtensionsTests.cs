using BLTools.Text;

using MediaSearch.Server.Services;

namespace MediaSearch.Models.Test;

[TestClass]
public class IEnumerableMediaExtensionsTests {

  /// <summary>
  /// Internal source of data
  /// </summary>
  internal IMSDatabase Database => _Database ??= new TMSDatabaseJson("data", "demo");
  private IMSDatabase? _Database;

  internal IMSTable<IMovie> MovieTable;

  public IEnumerableMediaExtensionsTests() {
    Assert.IsTrue(Database.Exists());
    IMSTable? Table = Database.GetTable("movies");
    if (Table is null) {
      throw new ApplicationException("Unable to obtain the table");
    }
    MovieTable = (IMSTable<IMovie>)Table;

    //Assert.IsTrue(Database.OpenOrCreate());
    //Assert.IsTrue(Database.Load());
  }

  #region --- Any tag --------------------------------------------
  [TestMethod]
  public void FilterByAnyTag_OneTagOnly_ResultOk() {
    TFilter Filter = new TFilter() { Tags = "science-fiction", TagSelection = EFilterType.Any };
    TraceBox($"{nameof(Filter)} : {Filter.GetType().Name}", Filter);
    IEnumerable<IMedia> Target = MovieTable.GetFiltered(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void FilterByAnyTag_OneTagOnlyButMissing_NothingToList() {
    TFilter Filter = new TFilter() { Tags = "xxxtotoxxx", TagSelection = EFilterType.Any };
    TraceBox($"{nameof(Filter)} : {Filter.GetType().Name}", Filter);
    IEnumerable<IMedia> Target = MovieTable.GetFiltered(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.IsEmpty());
  }

  [TestMethod]
  public void FilterByAnyTag_MultipleTags_ResultOk() {
    TFilter Filter = new TFilter() { Tags = "science-fiction aliens", TagSelection = EFilterType.Any };
    TraceBox("Filter", Filter);
    IEnumerable<IMedia> Target = MovieTable.GetFiltered(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }
  #endregion --- Any tag --------------------------------------------

  #region --- All tags --------------------------------------------
  [TestMethod]
  public void FilterByAllTag_OneTagOnly_ResultOk() {
    TFilter Filter = new TFilter() { Tags = "science-fiction", TagSelection = EFilterType.All };
    TraceBox($"{nameof(Filter)} : {Filter.GetType().Name}", Filter);
    IEnumerable<IMedia> Target = MovieTable.GetFiltered(Filter);
    Assert.IsNotNull(Target);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);

  }

  [TestMethod]
  public void FilterByAllTag_OneTagOnlyButMissing_ResultOk() {
    TFilter Filter = new TFilter() { Tags = "xxxtotoxxx", TagSelection = EFilterType.All };
    TraceBox($"{nameof(Filter)} : {Filter.GetType().Name}", Filter);
    IEnumerable<IMedia> Target = MovieTable.GetFiltered(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.IsEmpty());
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void FilterByAllTag_MultipleTags_ResultOk() {
    TFilter Filter = new TFilter() { Tags = "science-fiction aliens", TagSelection = EFilterType.All };
    TraceBox($"{nameof(Filter)} : {Filter.GetType().Name}", Filter);
    IEnumerable<IMedia> Target = MovieTable.GetFiltered(Filter);
    Assert.IsNotNull(Target);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);
  }
  #endregion --- All tags --------------------------------------------
}