namespace MediaSearch.Server.Services.Test;

[TestClass]
public class TMediaSearchDatabaseTest {

  [ClassInitialize]
  public static async Task DatabaseInit(TestContext testContext) {
    Global.Database = new TMediaSearchDatabaseJson() { StoragePath="data", StorageFilename = "movies.json" };
    Global.Database.Open();
    Global.MovieService = new TMovieService(Global.Database);
    await Global.MovieService.ParseAsync(CancellationToken.None).ConfigureAwait(false);
  }

  [TestMethod]
  public void Database_Initialized_NotEmpty() {
    Assert.IsFalse(Global.Database.IsEmpty());
  }

  [TestMethod]
  public void Database_Initialized_WithMoviesFiltered() {
    Assert.AreEqual(Global.Database.Count(), Global.Database.GetAll().Count());

    TFilter DefaultFilter = new TFilter() {
      Page = 2,
      PageSize = TMoviesPage.DEFAULT_PAGE_SIZE
    };

    TFilter Filter50 = new TFilter() {
      PageSize = 50
    };

    IEnumerable<IMedia> Target = Global.Database.GetFiltered(TFilter.Empty);
    Assert.IsNotNull(Target);
    Assert.AreEqual(Global.Database.Count(), Target.Count());

    Target = Global.Database.GetFiltered(DefaultFilter);
    Assert.IsNotNull(Target);
    Assert.AreEqual(TMoviesPage.DEFAULT_PAGE_SIZE, Target.Count());

    Target = Global.Database.GetFiltered(Filter50);
    Assert.IsNotNull(Target);
    Assert.AreEqual(50, Target.Count());
  }
  
}
