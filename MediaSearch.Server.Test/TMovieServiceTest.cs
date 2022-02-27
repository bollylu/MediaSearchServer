using BLTools.Text;

using static MediaSearch.Models.Support;

namespace MediaSearch.Server.Services.Test;

[TestClass]
public class TMovieServiceTest {

  #region --- Initialize --------------------------------------------
  [ClassInitialize]
  public static async Task MovieCacheInit(TestContext testContext) {
    //await MediaSearch.Models.GlobalSettings.Initialize().ConfigureAwait(false);
    Global.MovieService = new TMovieService(new XMovieCache() { DataSource = @"data\movies.json" });
    await Global.MovieService.Initialize().ConfigureAwait(false);
  }
  #endregion --- Initialize --------------------------------------------

  [TestMethod]
  public async Task TestCacheIsInitialized() {

    Assert.IsTrue(Global.MovieService.MoviesExtensions.Any());

    Assert.IsTrue(await Global.MovieService.GetAllMovies().AnyAsync().ConfigureAwait(false));

    int Count = await Global.MovieService.GetAllMovies().CountAsync().ConfigureAwait(false);

    TraceMessage("Movie count", Count);
    Assert.IsTrue(Count > 0);
  }

  [TestMethod]
  public async Task Service_GetFirstPage() {
    TFilter DefaultFilter = new TFilter() {
      Page = 1,
      PageSize = AMovieCache.DEFAULT_PAGE_SIZE
    };
    TraceMessage("Filter", DefaultFilter);
    IMoviesPage? Target = await Global.MovieService.GetMoviesPage(DefaultFilter).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    TraceMoviesName(Target.Movies);
    Assert.AreEqual(IMovieService.DEFAULT_PAGE_SIZE, Target.Movies.Count);
  }

  [TestMethod]
  public async Task Service_GetLastPage() {
    int MovieCount = await Global.MovieService.GetAllMovies().CountAsync().ConfigureAwait(false);
    int PageCount = (MovieCount / IMovieService.DEFAULT_PAGE_SIZE) + (MovieCount % IMovieService.DEFAULT_PAGE_SIZE) > 0 ? 1 : 0;

    StringBuilder Message = new();
    Message.AppendLine($"Movies count = {MovieCount}");
    Message.AppendLine($"Last page = {PageCount}");
    TraceMessage("Result", Message);

    IMoviesPage? Target = await Global.MovieService.GetMoviesLastPage(TFilter.Empty).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    TraceMoviesName(Target.Movies);
    Assert.IsTrue(IMovieService.DEFAULT_PAGE_SIZE >= Target.Movies.Count);
  }

  [TestMethod]
  public async Task Service_GetFilteredFirstPage() {
    TFilter Filter = new TFilter() { Keywords = "The", Page = 1, PageSize = IMovieService.DEFAULT_PAGE_SIZE };
    TraceMessage("Filter", Filter);
    IMoviesPage? Target = await Global.MovieService.GetMoviesPage(Filter).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    TraceMoviesName(Target.Movies);
    Assert.IsTrue(IMovieService.DEFAULT_PAGE_SIZE >= Target.Movies.Count);
  }

  [TestMethod]
  public async Task Service_GetFilteredLastPage() {
    TFilter Filter = new TFilter() { Keywords = "The", PageSize = IMovieService.DEFAULT_PAGE_SIZE };
    TraceMessage("Filter", Filter);

    int MovieCount = (await Global.MovieService.GetMoviesPage(Filter).ConfigureAwait(false) ?? TMoviesPage.Empty).Movies.Count;
    int PageCount = (MovieCount / IMovieService.DEFAULT_PAGE_SIZE) + (MovieCount % IMovieService.DEFAULT_PAGE_SIZE) > 0 ? 1 : 0;

    
    StringBuilder Message = new();
    Message.AppendLine($"Movies count = {MovieCount}");
    Message.AppendLine($"Last page = {PageCount}");
    TraceMessage("Result", Message);

    IMoviesPage? Target = await Global.MovieService.GetMoviesLastPage(Filter).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    TraceMoviesName(Target.Movies);
    Assert.IsTrue(IMovieService.DEFAULT_PAGE_SIZE >= Target.Movies.Count);
  }

}
