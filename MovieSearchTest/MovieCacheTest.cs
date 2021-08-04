using System;
using System.Linq;

using BLTools.Diagnostic.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MovieSearchServerServices.MovieService;

namespace MovieSearchTest {

  [TestClass]
  public class MovieCacheTest {

    public static IMovieCache MovieCache;

    [ClassInitialize]
    public static void MovieCacheInit(TestContext testContext) {
      MovieCache = new TMovieCache() { RootPath = @"\\andromeda.sharenet.priv\films", Logger = new TConsoleLogger() };
      Console.WriteLine("Initialize cache");
      MovieCache.Load();
      Console.WriteLine($"{MovieCache.GetAllMovies().Count()} movies in cache");
    }

    [TestMethod]
    public void CacheInitialized_CheckCache_CacheOk() {
      Assert.IsFalse(MovieCache.IsEmpty());
    }

    [TestMethod]
    public void CacheInitialized_RetrieveMovies_MoviesOk() {
      Assert.AreEqual(MovieCache.Count(), MovieCache.GetAllMovies().Count());

      Assert.AreEqual(AMovieCache.DEFAULT_PAGE_SIZE, MovieCache.GetMovies().Count());
      Assert.AreEqual(AMovieCache.DEFAULT_PAGE_SIZE, MovieCache.GetMovies(2).Count());
      Assert.AreEqual(50, MovieCache.GetMovies(2, 50).Count());
    }
  }

}
