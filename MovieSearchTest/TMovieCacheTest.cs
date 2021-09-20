using System;
using System.Linq;
using System.Threading.Tasks;

using BLTools.Diagnostic.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MovieSearchServerServices.MovieService;

namespace MovieSearchTest {

  [TestClass]
  public class TMovieCacheTest {

    [ClassInitialize]
    public static async Task MovieCacheInit(TestContext testContext) {
      if (Global.MovieCache is null) {
        Global.MovieCache = new TMovieCache() { Storage = Global.STORAGE, Logger = new TConsoleLogger() };
        await Global.MovieCache.Load().ConfigureAwait(false);
      }
    }

    [TestMethod]
    public void CacheInitialized_CheckCache_CacheOk() {
      Assert.IsFalse(Global.MovieCache.IsEmpty());
    }

    [TestMethod]
    public void CacheInitialized_RetrieveMovies_MoviesOk() {
      Assert.AreEqual(Global.MovieCache.Count(), Global.MovieCache.GetAllMovies().Count());

      Assert.AreEqual(AMovieCache.DEFAULT_PAGE_SIZE, Global.MovieCache.GetMovies().Count());
      Assert.AreEqual(AMovieCache.DEFAULT_PAGE_SIZE, Global.MovieCache.GetMovies(2).Count());
      Assert.AreEqual(50, Global.MovieCache.GetMovies(2, 50).Count());
    }
  }

}
