namespace MovieSearchTest;

[TestClass]
public class TMovieCacheTest {

  [ClassInitialize]
  public static async Task MovieCacheInit(TestContext testContext) {
    if (Global.MovieCache is null) {
      Global.MovieCache = new TMovieCache() { RootStoragePath = Global.STORAGE, Logger = new TConsoleLogger() };

      using (CancellationTokenSource Timeout = new CancellationTokenSource((int)TimeSpan.FromMinutes(5).TotalMilliseconds)) {
        await Global.MovieCache.Parse(Timeout.Token).ConfigureAwait(false);
      }

    }
  }

  [TestMethod]
  public void CacheInitialized_CheckCache_CacheOk() {
    Assert.IsFalse(Global.MovieCache.IsEmpty());
  }

  [TestMethod]
  public void CacheInitialized_RetrieveMovies_MoviesOk() {
    Assert.AreEqual(Global.MovieCache.Count(), Global.MovieCache.GetAllMovies().Count());

    TFilter DefaultFilter = new TFilter() {
      Page = 2,
      PageSize = AMovieCache.DEFAULT_PAGE_SIZE
    };

    TFilter Filter50 = new TFilter() {
      PageSize = 50
    };

    Assert.AreEqual(AMovieCache.DEFAULT_PAGE_SIZE, Global.MovieCache.GetMoviesPage(TFilter.Empty).Movies.Count());
    Assert.AreEqual(AMovieCache.DEFAULT_PAGE_SIZE, Global.MovieCache.GetMoviesPage(DefaultFilter).Movies.Count());
    Assert.AreEqual(50, Global.MovieCache.GetMoviesPage(Filter50).Movies.Count());
  }
}
