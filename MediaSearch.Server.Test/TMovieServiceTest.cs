using MediaSearch.Storage;

namespace MediaSearch.Server.Services.Test;

[TestClass]
public class TMovieServiceTest {

  #region --- Initialize --------------------------------------------
  [ClassInitialize]
  public static async Task MovieCacheInit(TestContext testContext) {
    //await MediaSearch.Models.GlobalSettings.Initialize().ConfigureAwait(false);
    Global.MovieService = new TMovieService(new TStorageMemoryMovies(), new TMediaSourceMovie(@"data\movies.json"));
    await Global.MovieService.Initialize().ConfigureAwait(false);
  }
  #endregion --- Initialize --------------------------------------------

  [TestMethod]
  public async Task TestCacheIsInitialized() {

    Assert.IsTrue(Global.MovieService.MoviesExtensions.Any());

    Assert.IsTrue(await Global.MovieService.GetAll().AnyAsync().ConfigureAwait(false));

    int Count = await Global.MovieService.GetAll().CountAsync().ConfigureAwait(false);

    Dump(Count, "Movie count");
    Assert.IsTrue(Count > 0);
  }

  [TestMethod]
  public async Task Service_GetFirstPage() {
    TFilter DefaultFilter = new TFilter() {
      Page = 1,
      PageSize = AMediaCache.DEFAULT_PAGE_SIZE
    };
    Dump(DefaultFilter, "Filter");
    IMoviesPage? Target = await Global.MovieService.GetPage(DefaultFilter).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    TraceMoviesName(Target.Movies);
    Assert.AreEqual(IMediaService.DEFAULT_PAGE_SIZE, Target.Movies.Count);
  }

  [TestMethod]
  public async Task Service_GetLastPage() {
    int MovieCount = await Global.MovieService.GetAll().CountAsync().ConfigureAwait(false);
    int PageCount = (MovieCount / IMediaService.DEFAULT_PAGE_SIZE) + (MovieCount % IMediaService.DEFAULT_PAGE_SIZE) > 0 ? 1 : 0;

    StringBuilder Message = new();
    Message.AppendLine($"Movies count = {MovieCount}");
    Message.AppendLine($"Last page = {PageCount}");
    Dump(Message, "Result");

    IMoviesPage? Target = await Global.MovieService.GetLastPage(TFilter.Empty).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    TraceMoviesName(Target.Movies);
    Assert.IsTrue(IMediaService.DEFAULT_PAGE_SIZE >= Target.Movies.Count);
  }

  [TestMethod]
  public async Task Service_GetFilteredFirstPage() {
    TFilter Filter = new TFilter() { Keywords = "The", Page = 1, PageSize = IMediaService.DEFAULT_PAGE_SIZE };
    Dump(Filter, "Filter");
    IMoviesPage? Target = await Global.MovieService.GetPage(Filter).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    TraceMoviesName(Target.Movies);
    Assert.IsTrue(IMediaService.DEFAULT_PAGE_SIZE >= Target.Movies.Count);
  }

  [TestMethod]
  public async Task Service_GetFilteredLastPage() {
    TFilter Filter = new TFilter() { Keywords = "The", PageSize = IMediaService.DEFAULT_PAGE_SIZE };
    Dump(Filter, "Filter");

    int MovieCount = (await Global.MovieService.GetPage(Filter).ConfigureAwait(false) ?? TMoviesPage.Empty).Movies.Count;
    int PageCount = (MovieCount / IMediaService.DEFAULT_PAGE_SIZE) + (MovieCount % IMediaService.DEFAULT_PAGE_SIZE) > 0 ? 1 : 0;


    StringBuilder Message = new();
    Message.AppendLine($"Movies count = {MovieCount}");
    Message.AppendLine($"Last page = {PageCount}");
    Dump(Message, "Result");

    IMoviesPage? Target = await Global.MovieService.GetLastPage(Filter).ConfigureAwait(false);
    Assert.IsNotNull(Target);
    TraceMoviesName(Target.Movies);
    Assert.IsTrue(IMediaService.DEFAULT_PAGE_SIZE >= Target.Movies.Count);
  }

}
