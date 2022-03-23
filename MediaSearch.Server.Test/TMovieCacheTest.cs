namespace MediaSearch.Server.Services.Test;

[TestClass]
public class TMovieCacheTest {

  [ClassInitialize]
  public static async Task MovieCacheInit(TestContext testContext) {
    //await MediaSearch.Models.GlobalSettings.Initialize().ConfigureAwait(false);
    Global.MovieCache = new XMovieCache() { DataSource = @"data\movies.json" };
    await Global.MovieCache.Parse(CancellationToken.None).ConfigureAwait(false);
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

    TMoviesPage? Target = Global.MovieCache.GetMoviesPage(TFilter.Empty);
    Assert.IsNotNull(Target);
    Assert.AreEqual(AMovieCache.DEFAULT_PAGE_SIZE, Target.Movies.Count());

    Target = Global.MovieCache.GetMoviesPage(DefaultFilter);
    Assert.IsNotNull(Target);
    Assert.AreEqual(AMovieCache.DEFAULT_PAGE_SIZE, Target.Movies.Count());

    Target = Global.MovieCache.GetMoviesPage(Filter50);
    Assert.IsNotNull(Target);
    Assert.AreEqual(50, Target.Movies.Count());
  }

  [TestMethod]
  public async Task SaveCacheToJson() {
    TMovieCache MovieCache = new TMovieCache() { RootStoragePath = @"\\multimedia.sharenet.priv\multimedia\films" };
    using (CancellationTokenSource Timeout = new CancellationTokenSource(100000)) {
      await MovieCache.Parse(Timeout.Token);
    }
    StringBuilder sb = new StringBuilder();
    sb.AppendLine("{\n \"movies\" : [\n");
    foreach (IJson MovieItem in MovieCache.GetAllMovies()) {
      sb.Append(MovieItem.ToJson());
      sb.AppendLine(",");
    }
    sb.Truncate(1);
    sb.AppendLine("\n]\n}");
    const string TARGET_FOLDER = @"i:\json";
    if (!Directory.Exists(TARGET_FOLDER)) {
      Directory.CreateDirectory(TARGET_FOLDER);
    }
    File.WriteAllText($@"{TARGET_FOLDER}\movies.json", sb.ToString(), Encoding.UTF8);

    Assert.IsTrue(File.Exists($@"{TARGET_FOLDER}\movies.json"));
  }
}
