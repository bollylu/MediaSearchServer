using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BLTools.Diagnostic.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MovieSearchModels;
using static MovieSearchModels.TSupport;

using MovieSearchServerServices.MovieService;

namespace MovieSearchTest {
  [TestClass]
  public class TMovieServiceTest {

    [ClassInitialize]
    public static async Task MovieCacheInit(TestContext testContext) {
      if (Global.MovieService is null) {
        if (Global.MovieCache is null) {
          Global.MovieService = new TMovieService() { Storage = Global.STORAGE, Logger = new TConsoleLogger() };
          await Global.MovieService.Initialize().ConfigureAwait(false);
        }
        else {
          Global.MovieService = new TMovieService(Global.MovieCache) { Storage = Global.STORAGE, Logger = new TConsoleLogger() };
        }
      }
    }

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
      List<TMovie> Target = await Global.MovieService.GetMovies(1, IMovieService.DEFAULT_PAGE_SIZE).ToListAsync().ConfigureAwait(false);
      PrintMoviesName(Target);
      Assert.AreEqual(IMovieService.DEFAULT_PAGE_SIZE, Target.Count);
    }

    [TestMethod]
    public async Task Service_GetLastPage() {
      int MovieCount = await Global.MovieService.MoviesCount().ConfigureAwait(false);
      int PageCount = await Global.MovieService.PagesCount(IMovieService.DEFAULT_PAGE_SIZE).ConfigureAwait(false);

      Console.WriteLine($"Movie count = {MovieCount}");
      Console.WriteLine($"Page count = {PageCount}");

      List<TMovie> Target = await Global.MovieService.GetMovies(PageCount, IMovieService.DEFAULT_PAGE_SIZE).ToListAsync().ConfigureAwait(false);
      PrintMoviesName(Target);
      Assert.AreNotEqual(IMovieService.DEFAULT_PAGE_SIZE, Target.Count);
    }

    [TestMethod]
    public async Task Service_GetFilteredFirstPage() {
      List<TMovie> Target = await Global.MovieService.GetMovies("The", 1, IMovieService.DEFAULT_PAGE_SIZE).ToListAsync().ConfigureAwait(false);
      PrintMoviesName(Target);
      Assert.AreEqual(IMovieService.DEFAULT_PAGE_SIZE, Target.Count);
    }

    [TestMethod]
    public async Task Service_GetFilteredLastPage() {
      int MovieCount = await Global.MovieService.MoviesCount("the").ConfigureAwait(false);
      int PageCount = await Global.MovieService.PagesCount("the", IMovieService.DEFAULT_PAGE_SIZE).ConfigureAwait(false);

      Console.WriteLine($"Movies count = {MovieCount}");
      Console.WriteLine($"Last page = {PageCount}");

      List<TMovie> Target = await Global.MovieService.GetMovies("The", PageCount, IMovieService.DEFAULT_PAGE_SIZE).ToListAsync().ConfigureAwait(false);
      PrintMoviesName(Target);
      Assert.AreNotEqual(IMovieService.DEFAULT_PAGE_SIZE, Target.Count);
    }


    

  }
}
