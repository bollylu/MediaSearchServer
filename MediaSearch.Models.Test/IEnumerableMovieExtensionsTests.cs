using BLTools.Text;

using MediaSearch.Server.Services;

namespace MediaSearch.Models.Test;

[TestClass]
public class IEnumerableMovieExtensionsTests {

  [ClassInitialize]
  public static async Task ClassInitialize(TestContext context) {
    await MediaSearch.Models.GlobalSettings.Initialize().ConfigureAwait(false);
  }

  /// <summary>
  /// Internal source of data
  /// </summary>
  internal static IMovieCache MovieCache => _MovieCache ??= XMovieCache.Instance(@"data\movies.json");
  private static IMovieCache? _MovieCache;

  #region --- Any tag --------------------------------------------
  [TestMethod]
  public void FilterByAnyTag_OneTagOnly_ResultOk() {
    TFilter Filter = new TFilter() { Tags = "science-fiction", TagSelection = EFilterType.Any };
    Console.WriteLine(Filter.ToString().Box("Filter"));
    IEnumerable<IMovie> Target = MovieCache.GetAllMovies().FilterByTags(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);
    Support.PrintMovies(Target);
  }

  [TestMethod]
  public void FilterByAnyTag_OneTagOnlyButMissing_ResultOk() {
    TFilter Filter = new TFilter() { Tags = "xxxtotoxxx", TagSelection = EFilterType.Any };
    Console.WriteLine(Filter.ToString().Box("Filter"));
    IEnumerable<IMovie> Target = MovieCache.GetAllMovies().FilterByTags(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.IsEmpty());
  }

  [TestMethod]
  public void FilterByAnyTag_MultipleTags_ResultOk() {
    TFilter Filter = new TFilter() { Tags = "science-fiction aliens", TagSelection = EFilterType.Any };
    Console.WriteLine(Filter.ToString().Box("Filter"));
    IEnumerable<IMovie> Target = MovieCache.GetAllMovies().FilterByTags(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);
    Support.PrintMovies(Target);
  }
  #endregion --- Any tag --------------------------------------------

  #region --- All tags --------------------------------------------
  [TestMethod]
  public void FilterByAllTag_OneTagOnly_ResultOk() {
    TFilter Filter = new TFilter() { Tags = "science-fiction", TagSelection = EFilterType.All };
    Console.WriteLine(Filter.ToString().Box("Filter"));
    IEnumerable<IMovie> Target = MovieCache.GetAllMovies().FilterByTags(Filter);
    Assert.IsNotNull(Target);
    Support.PrintMovies(Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);

  }

  [TestMethod]
  public void FilterByAllTag_OneTagOnlyButMissing_ResultOk() {
    TFilter Filter = new TFilter() { Tags = "xxxtotoxxx", TagSelection = EFilterType.All };
    Console.WriteLine(Filter.ToString().Box("Filter"));
    IEnumerable<IMovie> Target = MovieCache.GetAllMovies().FilterByTags(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.IsEmpty());
  }

  [TestMethod]
  public void FilterByAllTag_MultipleTags_ResultOk() {
    TFilter Filter = new TFilter() { Tags = "science-fiction aliens", TagSelection = EFilterType.All };
    Console.WriteLine(Filter.ToString().Box("Filter"));
    IEnumerable<IMovie> Target = MovieCache.GetAllMovies().FilterByTags(Filter);
    Assert.IsNotNull(Target);
    Support.PrintMovies(Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);
  }
  #endregion --- All tags --------------------------------------------
}