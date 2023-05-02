using BLTools.Text;

namespace MediaSearch.Models.Test;

[TestClass]
public class IEnumerableMovieExtensionsTests {

  //[ClassInitialize]
  //public static async Task ClassInitialize(TestContext context) {
  //  await MediaSearch.Models.GlobalSettings.Initialize().ConfigureAwait(false);
  //}

  /// <summary>
  /// Internal source of data
  /// </summary>
  internal static List<IMedia> MediaSource => _MediaSource ??= XMovieCache.Instance(@"data\movies.json");
  private static List<IMedia>? _MediaSource;

  #region --- Any tag --------------------------------------------
  [TestMethod]
  public void FilterByAnyTag_OneTagOnly_ResultOk() {
    Message("Instanciate filter");
    TFilter Filter = new TFilter() { Tags = new TMultiItemsSelection(EFilterType.Any, "science-fiction") };
    Dump(Filter);

    Message("Filter data");
    IEnumerable<IMedia> Target = MediaSource.GetAllMovies().FilterByTags(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);
    Dump(Target);

    Ok();

  }

  [TestMethod]
  public void FilterByAnyTag_OneTagOnlyButMissing_ResultOk() {
    Message("Instanciate filter");
    TFilter Filter = new TFilter() { Tags = new TMultiItemsSelection(EFilterType.Any, "xxxtotoxxx") };
    Dump(Filter);

    Message("Filter data");
    IEnumerable<IMedia> Target = MediaSource.GetAllMovies().FilterByTags(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.IsEmpty());

    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void FilterByAnyTag_MultipleTags_ResultOk() {
    Message("Instanciate filter");
    TFilter Filter = new TFilter() { Tags = new TMultiItemsSelection(EFilterType.Any, "science-fiction", "aliens") };
    Dump(Filter);

    Message("Filter data");
    IEnumerable<IMedia> Target = MediaSource.GetAllMovies().FilterByTags(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);
    Dump(Target);

    Ok();
  }
  #endregion --- Any tag --------------------------------------------

  #region --- All tags --------------------------------------------
  [TestMethod]
  public void FilterByAllTag_OneTagOnly_ResultOk() {
    Message("Instanciate filter");
    TFilter Filter = new TFilter() { Tags = new TMultiItemsSelection(EFilterType.All, "science-fiction") };
    Dump(Filter);

    Message("Filter data");
    IEnumerable<IMedia> Target = MediaSource.GetAllMovies().FilterByTags(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);
    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void FilterByAllTag_OneTagOnlyButMissing_ResultOk() {
    Message("Instanciate filter");
    TFilter Filter = new TFilter() { Tags = new TMultiItemsSelection(EFilterType.All, "xxxtotoxxx") };
    Dump(Filter);

    Message("Filter data");
    IEnumerable<IMedia> Target = MediaSource.GetAllMovies().FilterByTags(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.IsEmpty());
    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void FilterByAllTag_MultipleTags_ResultOk() {
    Message("Instanciate filter");
    TFilter Filter = new TFilter() { Tags = new TMultiItemsSelection(EFilterType.All, "science-fiction", "aliens") };
    Dump(Filter);

    Message("Filter data");
    IEnumerable<IMedia> Target = MediaSource.GetAllMovies().FilterByTags(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);
    Dump(Target);

    Ok();
  }
  #endregion --- All tags --------------------------------------------
}