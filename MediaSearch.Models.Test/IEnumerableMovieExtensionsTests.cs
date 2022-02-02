using BLTools.Text;

using MediaSearch.Server.Services;

namespace MediaSearch.Models.Test;

[TestClass]
public class IEnumerableMovieExtensionsTests {

  #region --- Any tag --------------------------------------------
  [TestMethod]
  public void FilterByAnyTag_OneTagOnly_ResultOk() {
    IEnumerable<IMovie> Source = Global.MovieCache.GetAllMovies();
    TFilter Filter = new TFilter() { Tags = "science-fiction", TagSelection = EFilterType.Any };
    Console.WriteLine(Filter.ToString().Box("Filter"));
    IEnumerable<IMovie> Target = Source.FilterByTags(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);
    Support.PrintMovies(Target);
  }

  [TestMethod]
  public void FilterByAnyTag_OneTagOnlyButMissing_ResultOk() {
    IEnumerable<IMovie> Source = Global.MovieCache.GetAllMovies();
    TFilter Filter = new TFilter() { Tags = "xxxtotoxxx", TagSelection = EFilterType.Any };
    Console.WriteLine(Filter.ToString().Box("Filter"));
    IEnumerable<IMovie> Target = Source.FilterByTags(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.IsEmpty());
  }

  [TestMethod]
  public void FilterByAnyTag_MultipleTags_ResultOk() {
    IEnumerable<IMovie> Source = Global.MovieCache.GetAllMovies();
    TFilter Filter = new TFilter() { Tags = "science-fiction aliens", TagSelection = EFilterType.Any };
    Console.WriteLine(Filter.ToString().Box("Filter"));
    IEnumerable<IMovie> Target = Source.FilterByTags(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);
    Support.PrintMovies(Target);
  }
  #endregion --- Any tag --------------------------------------------

  #region --- All tags --------------------------------------------
  [TestMethod]
  public void FilterByAllTag_OneTagOnly_ResultOk() {
    IEnumerable<IMovie> Source = Global.MovieCache.GetAllMovies();
    TFilter Filter = new TFilter() { Tags = "science-fiction", TagSelection = EFilterType.All };
    Console.WriteLine(Filter.ToString().Box("Filter"));
    IEnumerable<IMovie> Target = Source.FilterByTags(Filter);
    Assert.IsNotNull(Target);
    Support.PrintMovies(Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);

  }

  [TestMethod]
  public void FilterByAllTag_OneTagOnlyButMissing_ResultOk() {
    IEnumerable<IMovie> Source = Global.MovieCache.GetAllMovies();
    TFilter Filter = new TFilter() { Tags = "xxxtotoxxx", TagSelection = EFilterType.All };
    Console.WriteLine(Filter.ToString().Box("Filter"));
    IEnumerable<IMovie> Target = Source.FilterByTags(Filter);
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.IsEmpty());
  }

  [TestMethod]
  public void FilterByAllTag_MultipleTags_ResultOk() {
    IEnumerable<IMovie> Source = Global.MovieCache.GetAllMovies();
    TFilter Filter = new TFilter() { Tags = "science-fiction aliens", TagSelection = EFilterType.All };
    Console.WriteLine(Filter.ToString().Box("Filter"));
    IEnumerable<IMovie> Target = Source.FilterByTags(Filter);
    Assert.IsNotNull(Target);
    Support.PrintMovies(Target);
    Assert.IsTrue(Target.Any());
    Assert.IsTrue(Target.Count() > 10);
  }
  #endregion --- All tags --------------------------------------------
}