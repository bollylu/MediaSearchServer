using BLTools.Text;

using static MediaSearch.Models.Support;

namespace MediaSearch.Server.Services.Test;

[TestClass]
public class TMovieServiceTest {

  [TestMethod]
  public void Instanciate_TMovieService_Empty() {
    IMovieService Target = new TMovieService();
    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void Instanciate_TMovieService_WithDatabase() {
    TMediaSearchMovieDatabaseMemory Database = new TMediaSearchMovieDatabaseMemory();
    IMovieService Target = new TMovieService(Database);
    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public async Task Instanciate_TMovieService_WithDatabase_WithContent() {
    TMSTableJsonMovie Database = new TMSTableJsonMovie("data", "movies.json");
    Database.OpenOrCreate();
    await Database.LoadAsync(CancellationToken.None);

    IMovieService Target = new TMovieService(Database) { Name = "test db", Description = "the db for the tests" };
    TraceMessage("Target", Target);

    //TFilter DefaultFilter = new TFilter() {
    //  Page = 1,
    //  PageSize = TMoviesPage.DEFAULT_PAGE_SIZE
    //};
    //TraceMessage("Filter", DefaultFilter);





    //IMoviesPage? Target = await Global.MovieService.GetMoviesPage(DefaultFilter).ConfigureAwait(false);
    //Assert.IsNotNull(Target);
    //TraceMoviesName(Target.Movies);
    //Assert.AreEqual(TMoviesPage.DEFAULT_PAGE_SIZE, Target.Movies.Count);
  }

  [TestMethod]
  public async Task TMovieService_GetFirstPage() {
    TMSTableJsonMovie Database = new TMSTableJsonMovie("data", "movies.json");
    Database.OpenOrCreate();
    await Database.LoadAsync(CancellationToken.None);

    IMovieService Source = new TMovieService(Database) { Name = "test db", Description = "the db for the tests" };
    TraceMessage($"{nameof(Source)} : {Source.GetType().Name}", Source);

    TFilter DefaultFilter = new TFilter() {
      Page = 1,
      PageSize = TMoviesPage.DEFAULT_PAGE_SIZE
    };
    TraceMessage($"{nameof(DefaultFilter)} : {DefaultFilter.GetType().Name}", DefaultFilter);

    IMoviesPage? Target = await Source.GetMoviesPage(DefaultFilter).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
    Assert.AreEqual(TMoviesPage.DEFAULT_PAGE_SIZE, Target.Movies.Count);
  }

  [TestMethod]
  public async Task Service_GetLastPage() {
    TMSTableJsonMovie Database = new TMSTableJsonMovie("data", "movies.json");
    Database.OpenOrCreate();
    await Database.LoadAsync(CancellationToken.None);

    IMovieService Source = new TMovieService(Database) { Name = "test db", Description = "the db for the tests" };
    TraceMessage($"{nameof(Source)} : {Source.GetType().Name}", Source);

    TFilter DefaultFilter = new TFilter() {
      Page = 1,
      PageSize = TMoviesPage.DEFAULT_PAGE_SIZE
    };
    TraceMessage($"{nameof(DefaultFilter)} : {DefaultFilter.GetType().Name}", DefaultFilter);

    IMoviesPage? Target = await Source.GetMoviesLastPage(DefaultFilter).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public async Task Service_GetFilteredFirstPage() {
    TMSTableJsonMovie Database = new TMSTableJsonMovie("data", "movies.json");
    Database.OpenOrCreate();
    await Database.LoadAsync(CancellationToken.None);

    IMovieService Source = new TMovieService(Database) { Name = "test db", Description = "the db for the tests" };
    TraceMessage($"{nameof(Source)} : {Source.GetType().Name}", Source);

    TFilter DefaultFilter = new TFilter() {
      Keywords = "the",
      Page = 1,
      PageSize = TMoviesPage.DEFAULT_PAGE_SIZE
    };
    TraceMessage($"{nameof(DefaultFilter)} : {DefaultFilter.GetType().Name}", DefaultFilter);

    IMoviesPage? Target = await Source.GetMoviesPage(DefaultFilter).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
    Assert.AreEqual(TMoviesPage.DEFAULT_PAGE_SIZE, Target.Movies.Count);
  }

  [TestMethod]
  public async Task Service_GetFilteredLastPage() {
    TMSTableJsonMovie Database = new TMSTableJsonMovie("data", "movies.json");
    Database.OpenOrCreate();
    await Database.LoadAsync(CancellationToken.None);

    IMovieService Source = new TMovieService(Database) { Name = "test db", Description = "the db for the tests" };
    TraceMessage($"{nameof(Source)} : {Source.GetType().Name}", Source);

    TFilter DefaultFilter = new TFilter() {
      Keywords = "the",
      Page = 1,
      PageSize = TMoviesPage.DEFAULT_PAGE_SIZE
    };
    TraceMessage($"{nameof(DefaultFilter)} : {DefaultFilter.GetType().Name}", DefaultFilter);

    IMoviesPage? Target = await Source.GetMoviesLastPage(DefaultFilter).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

}
