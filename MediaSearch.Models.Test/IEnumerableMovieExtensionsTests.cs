using MediaSearch.Server.Services;

namespace MediaSearch.Models.Test;

[TestClass]
public class IEnumerableMovieExtensionsTests {

  #region --- Any tag --------------------------------------------
  [TestMethod]
  public void FilterByAnyTag_OneTagOnly_ResultOk() {
    IEnumerable<IMovie> Source = Global.MovieCache.GetAllMovies();
    IEnumerable<IMovie> Target = Source.FilterByAnyTag("science-fiction");
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.Any());
    Assert.AreEqual(3, Target.Count());
    Support.PrintMovies(Target);
  }

  [TestMethod]
  public void FilterByAnyTag_OneTagOnlyButMissing_ResultOk() {
    IEnumerable<IMovie> Source = Global.MovieCache.GetAllMovies();
    IEnumerable<IMovie> Target = Source.FilterByAnyTag("xxxtotoxxx");
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.IsEmpty());
  }

  [TestMethod]
  public void FilterByAnyTag_MultipleTags_ResultOk() {
    IEnumerable<IMovie> Source = Global.MovieCache.GetAllMovies();
    IEnumerable<IMovie> Target = Source.FilterByAnyTag("science-fiction aliens");
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.Any());
    Assert.AreEqual(3, Target.Count());
    Support.PrintMovies(Target);
  }
  #endregion --- Any tag --------------------------------------------

  #region --- All tags --------------------------------------------
  [TestMethod]
  public void FilterByAllTag_OneTagOnly_ResultOk() {
    IEnumerable<IMovie> Source = Global.MovieCache.GetAllMovies();
    IEnumerable<IMovie> Target = Source.FilterByAllTags("science-fiction");
    Assert.IsNotNull(Target);
    Support.PrintMovies(Target);
    Assert.IsTrue(Target.Any());
    Assert.AreEqual(3, Target.Count());
    
  }

  [TestMethod]
  public void FilterByAllTag_OneTagOnlyButMissing_ResultOk() {
    IEnumerable<IMovie> Source = Global.MovieCache.GetAllMovies();
    IEnumerable<IMovie> Target = Source.FilterByAllTags("xxxtotoxxx");
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.IsEmpty());
  }

  [TestMethod]
  public void FilterByAllTag_MultipleTags_ResultOk() {
    IEnumerable<IMovie> Source = Global.MovieCache.GetAllMovies();
    IEnumerable<IMovie> Target = Source.FilterByAllTags("science-fiction aliens");
    Assert.IsNotNull(Target);
    Support.PrintMovies(Target);
    Assert.IsTrue(Target.Any());
    Assert.AreEqual(2, Target.Count());
  } 
  #endregion --- All tags --------------------------------------------
}