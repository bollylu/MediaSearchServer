
using MediaSearch.Database;

namespace MediaSearch.Server.Services.Test;

[TestClass]
public class TMovieServiceTest {

  [TestMethod]
  public void Instanciate_TMovieService_Empty() {
    IMovieService Target = new TMovieService();
    Dump(Target);
    Ok();
  }

  [TestMethod]
  public void Instanciate_TMovieService_WithDatabase() {
    IMSDatabase Database = new TMSDatabaseMemory();
    Database.Open();
    IMSTable<IMovie>? Table = Database.GetTable("movies") as IMSTable<IMovie>;
    Assert.IsNotNull(Table);
    IMovieService Target = new TMovieService(Table);
    Dump(Target);
    Ok();
  }

  [TestMethod]
  public void Instanciate_TMovieService_WithDatabase_WithContent() {
    IMSDatabase Database = new TMSDatabaseJson("data", "demo");
    Database.Open();
    IMSTable<IMovie>? Table = Database.GetTable("movies") as IMSTable<IMovie>;
    Assert.IsNotNull(Table);
    IMovieService Target = new TMovieService(Table) { Name = "test db", Description = "the db for the tests" };
    Dump(Target);
    Ok();
  }

  [TestMethod]
  public async Task TMovieService_GetFirstPage() {
    IMSDatabase Database = new TMSDatabaseJson("data", "demo");
    Database.Open();
    IMSTable<IMovie>? Table = Database.GetTable("movies") as IMSTable<IMovie>;
    Assert.IsNotNull(Table);
    IMovieService Source = new TMovieService(Table) { Name = "test db", Description = "the db for the tests" };

    TFilter DefaultFilter = new TFilter() {
      Page = 1,
      PageSize = TMoviesPage.DEFAULT_PAGE_SIZE
    };
    Dump(DefaultFilter);

    IMoviesPage? Target = await Source.GetMoviesPage(DefaultFilter).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    Dump(Target);

    Assert.AreEqual(TMoviesPage.DEFAULT_PAGE_SIZE, Target.Movies.Count);
    Ok();
  }

  [TestMethod]
  public async Task Service_GetLastPage() {
    IMSDatabase Database = new TMSDatabaseJson("data", "demo");
    Database.Open();
    IMSTable<IMovie>? Table = Database.GetTable("movies") as IMSTable<IMovie>;
    Assert.IsNotNull(Table);
    IMovieService Source = new TMovieService(Table) { Name = "test db", Description = "the db for the tests" };

    TFilter DefaultFilter = new TFilter() {
      Page = 1,
      PageSize = TMoviesPage.DEFAULT_PAGE_SIZE
    };
    Dump(DefaultFilter);

    IMoviesPage? Target = await Source.GetMoviesLastPage(DefaultFilter).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    Dump(Target);
    Ok();
  }

  [TestMethod]
  public async Task Service_GetFilteredFirstPage() {
    IMSDatabase Database = new TMSDatabaseJson("data", "demo");
    Database.Open();
    IMSTable<IMovie>? Table = Database.GetTable("movies") as IMSTable<IMovie>;
    Assert.IsNotNull(Table);
    IMovieService Source = new TMovieService(Table) { Name = "test db", Description = "the db for the tests" };

    TFilter DefaultFilter = new TFilter() {
      Keywords = "the",
      Page = 1,
      PageSize = TMoviesPage.DEFAULT_PAGE_SIZE
    };
    Dump(DefaultFilter);

    IMoviesPage? Target = await Source.GetMoviesPage(DefaultFilter).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    Dump(Target);

    Assert.AreEqual(TMoviesPage.DEFAULT_PAGE_SIZE, Target.Movies.Count);
    Ok();
  }

  [TestMethod]
  public async Task Service_GetFilteredLastPage() {
    IMSDatabase Database = new TMSDatabaseJson("data", "demo");
    Database.Open();
    IMSTable<IMovie>? Table = Database.GetTable("movies") as IMSTable<IMovie>;
    Assert.IsNotNull(Table);
    IMovieService Source = new TMovieService(Table) { Name = "test db", Description = "the db for the tests" };

    TFilter DefaultFilter = new TFilter() {
      Keywords = "the",
      Page = 1,
      PageSize = TMoviesPage.DEFAULT_PAGE_SIZE
    };
    Dump(DefaultFilter);

    IMoviesPage? Target = await Source.GetMoviesLastPage(DefaultFilter).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    Dump(Target);
    Ok();
  }

}
