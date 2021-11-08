using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BLTools.Diagnostic.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MovieSearchModels;

using MovieSearchServerServices.MovieService;

namespace MovieSearchTest {

  [TestClass]
  public class TMovieCacheTest {

    [ClassInitialize]
    public static async Task MovieCacheInit(TestContext testContext) {
      if (Global.MovieCache is null) {
        Global.MovieCache = new TMovieCache() { Storage = Global.STORAGE, Logger = new TConsoleLogger() };

        using (CancellationTokenSource Timeout = new CancellationTokenSource((int)TimeSpan.FromMinutes(5).TotalMilliseconds)) {
          IEnumerable<IFileInfo> Source = Global.MovieCache.FetchFiles(Timeout.Token);
          await Global.MovieCache.Parse(Source, Timeout.Token).ConfigureAwait(false);
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

      Assert.AreEqual(AMovieCache.DEFAULT_PAGE_SIZE, Global.MovieCache.GetMovies().Count());
      Assert.AreEqual(AMovieCache.DEFAULT_PAGE_SIZE, Global.MovieCache.GetMovies(2).Count());
      Assert.AreEqual(50, Global.MovieCache.GetMovies(2, 50).Count());
    }
  }

}
