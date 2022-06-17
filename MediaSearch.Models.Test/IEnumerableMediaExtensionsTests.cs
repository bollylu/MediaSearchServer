namespace MediaSearch.Models.Test;

[TestClass]
public class IEnumerableMediaExtensionsTests {

  /// <summary>
  /// Internal source of data
  /// </summary>
  internal IMSDatabase Database => _Database ??= new TMSDatabaseJson("data", "demo");
  private IMSDatabase? _Database;

  internal IMSTableMovie MovieTable;

  public IEnumerableMediaExtensionsTests() {
    Dump(Database);
    Assert.IsTrue(Database.Exists());

    Message("Opening database");
    Assert.IsTrue(Database.Open());

    Message("Get table Movies");
    IMSTable? Table = Database.Schema.Get("movies");
    if (Table is null) {
      Failed("Unable to obtain the Movies table");
      throw new ApplicationException("Unable to obtain the table");
    }

    MovieTable = (IMSTableMovie)Table;

  }

  #region --- Any tag --------------------------------------------
  [TestMethod]
  public void FilterByAnyTag_OneTagOnly_ResultOk() {
    TFilter Filter = new TFilter() { Tags = "science-fiction", TagSelection = EFilterType.Any };
    Dump(Filter);
    IEnumerable<IMedia> Target = MovieTable.GetFiltered(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);
    Dump(Target);
  }

  [TestMethod]
  public void FilterByAnyTag_OneTagOnlyButMissing_NothingToList() {
    TFilter Filter = new TFilter() { Tags = "xxxtotoxxx", TagSelection = EFilterType.Any };
    Dump(Filter);
    IEnumerable<IMedia> Target = MovieTable.GetFiltered(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.IsEmpty());
  }

  [TestMethod]
  public void FilterByAnyTag_MultipleTags_ResultOk() {
    TFilter Filter = new TFilter() { Tags = "science-fiction aliens", TagSelection = EFilterType.Any };
    Dump(Filter);
    IEnumerable<IMedia> Target = MovieTable.GetFiltered(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);
    Dump(Target);
  }
  #endregion --- Any tag --------------------------------------------

  #region --- All tags --------------------------------------------
  [TestMethod]
  public void FilterByAllTag_OneTagOnly_ResultOk() {
    TFilter Filter = new TFilter() { Tags = "science-fiction", TagSelection = EFilterType.All };
    Dump(Filter);
    IEnumerable<IMedia> Target = MovieTable.GetFiltered(Filter);
    Assert.IsNotNull(Target);
    Dump(Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);

  }

  [TestMethod]
  public void FilterByAllTag_OneTagOnlyButMissing_ResultOk() {
    TFilter Filter = new TFilter() { Tags = "xxxtotoxxx", TagSelection = EFilterType.All };
    Dump(Filter);
    IEnumerable<IMedia> Target = MovieTable.GetFiltered(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.IsEmpty());
    Dump(Target);
  }

  [TestMethod]
  public void FilterByAllTag_MultipleTags_ResultOk() {
    TFilter Filter = new TFilter() { Tags = "science-fiction aliens", TagSelection = EFilterType.All };
    Dump(Filter);
    IEnumerable<IMedia> Target = MovieTable.GetFiltered(Filter);
    Assert.IsNotNull(Target);
    Dump(Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);
  }
  #endregion --- All tags --------------------------------------------
}