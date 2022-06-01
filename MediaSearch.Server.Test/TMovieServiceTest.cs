using BLTools.Text;

using MediaSearch.Database;

using static MediaSearch.Models.Support;

namespace MediaSearch.Server.Services.Test;

[TestClass]
public class TMovieServiceTest {

  [TestMethod]
  public void Instanciate_TMovieService_Empty() {
    IMovieService Target = new TMovieService();
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void Instanciate_TMovieService_WithDatabase() {
    IMSDatabase Database = new TMSDatabaseMemory();
    Database.Open();
    IMSTable<IMovie>? Table = Database.GetTable("movies") as IMSTable<IMovie>;
    Assert.IsNotNull(Table);
    IMovieService Target = new TMovieService(Table);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void Instanciate_TMovieService_WithDatabase_WithContent() {
    IMSDatabase Database = new TMSDatabaseJson("data", "demo");
    Database.Open();
    IMSTable<IMovie>? Table = Database.GetTable("movies") as IMSTable<IMovie>;
    Assert.IsNotNull(Table);
    IMovieService Target = new TMovieService(Table) { Name = "test db", Description = "the db for the tests" };
    TraceBox("Target", Target);
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
    TraceBox($"{nameof(DefaultFilter)} : {DefaultFilter.GetType().Name}", DefaultFilter);

    IMoviesPage? Target = await Source.GetMoviesPage(DefaultFilter).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
    Assert.AreEqual(TMoviesPage.DEFAULT_PAGE_SIZE, Target.Movies.Count);
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
    TraceBox($"{nameof(DefaultFilter)} : {DefaultFilter.GetType().Name}", DefaultFilter);

    IMoviesPage? Target = await Source.GetMoviesLastPage(DefaultFilter).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
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
    TraceBox($"{nameof(DefaultFilter)} : {DefaultFilter.GetType().Name}", DefaultFilter);

    IMoviesPage? Target = await Source.GetMoviesPage(DefaultFilter).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
    Assert.AreEqual(TMoviesPage.DEFAULT_PAGE_SIZE, Target.Movies.Count);
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
    TraceBox($"{nameof(DefaultFilter)} : {DefaultFilter.GetType().Name}", DefaultFilter);

    IMoviesPage? Target = await Source.GetMoviesLastPage(DefaultFilter).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

}
