using BLTools.Text;

using static MediaSearch.Models.Support;

namespace MediaSearch.Server.Services.Test;

[TestClass]
public class TMovieServiceTest {

  #region --- Initialize --------------------------------------------
  [ClassInitialize]
  public static async Task MovieCacheInit(TestContext testContext) {
    await MediaSearch.Models.GlobalSettings.Initialize().ConfigureAwait(false);
    Global.MovieService = new TMovieService(new XMovieCache() { DataSource = @"data\movies.json" }) { Logger = new TConsoleLogger() };
    await Global.MovieService.Initialize().ConfigureAwait(false);
  }
  #endregion --- Initialize --------------------------------------------

  [TestMethod]
  public async Task TestCacheIsInitialized() {

    Assert.IsTrue(Global.MovieService.MoviesExtensions.Any());

    Assert.IsTrue(await Global.MovieService.GetAllMovies().AnyAsync().ConfigureAwait(false));

    int Count = await Global.MovieService.GetAllMovies().CountAsync().ConfigureAwait(false);

    Console.WriteLine($"Count : {Count}");
    Assert.IsTrue(Count > 0);
  }

  [TestMethod]
  public async Task Service_GetFirstPage() {
    TFilter DefaultFilter = new TFilter() {
      Page = 1,
      PageSize = AMovieCache.DEFAULT_PAGE_SIZE
    };
    Console.WriteLine(DefaultFilter.ToString().BoxFixedWidth("Filter", GlobalSettings.DEBUG_BOX_WIDTH));
    IMoviesPage? Target = await Global.MovieService.GetMoviesPage(DefaultFilter).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    PrintMoviesName(Target.Movies);
    Assert.AreEqual(IMovieService.DEFAULT_PAGE_SIZE, Target.Movies.Count);
  }

  [TestMethod]
  public async Task Service_GetLastPage() {
    int MovieCount = await Global.MovieService.GetAllMovies().CountAsync().ConfigureAwait(false);
    int PageCount = (MovieCount / IMovieService.DEFAULT_PAGE_SIZE) + (MovieCount % IMovieService.DEFAULT_PAGE_SIZE) > 0 ? 1 : 0;

    Console.WriteLine($"Movie count = {MovieCount}");
    Console.WriteLine($"Page count = {PageCount}");

    IMoviesPage? Target = await Global.MovieService.GetMoviesLastPage(TFilter.Empty).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    PrintMoviesName(Target.Movies);
    Assert.IsTrue(IMovieService.DEFAULT_PAGE_SIZE >= Target.Movies.Count);
  }

  [TestMethod]
  public async Task Service_GetFilteredFirstPage() {
    TFilter Filter = new TFilter() { Keywords = "The", Page = 1, PageSize = IMovieService.DEFAULT_PAGE_SIZE };
    Console.WriteLine(Filter.ToString().Box("Filter"));
    IMoviesPage? Target = await Global.MovieService.GetMoviesPage(Filter).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    PrintMoviesName(Target.Movies);
    Assert.IsTrue(IMovieService.DEFAULT_PAGE_SIZE >= Target.Movies.Count);
  }

  [TestMethod]
  public async Task Service_GetFilteredLastPage() {
    int MovieCount = await Global.MovieService.GetAllMovies().CountAsync().ConfigureAwait(false);
    int PageCount = (MovieCount / IMovieService.DEFAULT_PAGE_SIZE) + (MovieCount % IMovieService.DEFAULT_PAGE_SIZE) > 0 ? 1 : 0;

    TFilter Filter = new TFilter() { Keywords = "The", PageSize = IMovieService.DEFAULT_PAGE_SIZE };
    Console.WriteLine(Filter.ToString().Box("Filter"));
    Console.WriteLine($"Movies count = {MovieCount}");
    Console.WriteLine($"Last page = {PageCount}");

    IMoviesPage? Target = await Global.MovieService.GetMoviesLastPage(Filter).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    PrintMoviesName(Target.Movies);
    Assert.IsTrue(IMovieService.DEFAULT_PAGE_SIZE >= Target.Movies.Count);
  }

}
