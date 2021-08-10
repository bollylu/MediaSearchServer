using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BLTools.Diagnostic.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MovieSearch.Models;

using MovieSearchServerServices.MovieService;

namespace MovieSearchTest {
  [TestClass]
  public class TMovieServiceTest {

    static IMovieService MovieService;

    [ClassInitialize]
    public static async Task MovieCacheInit(TestContext testContext) {
      MovieService = new TMovieService(@"\\andromeda.sharenet.priv\films") { Logger = new TConsoleLogger() };
      await MovieService.Initialize().ConfigureAwait(false);
    }

    [TestMethod]
    public async Task TestCacheIsInitialized() {

      Assert.IsTrue(MovieService.MoviesExtensions.Any());

      Assert.IsTrue(await MovieService.GetAllMovies().AnyAsync().ConfigureAwait(false));

      int Count = await MovieService.GetAllMovies().CountAsync().ConfigureAwait(false);

      Console.WriteLine($"Count : {Count}");
      Assert.IsTrue(Count > 0);
    }

    [TestMethod]
    public async Task Service_GetFirstPage() {
      List<IMovie> Target = await MovieService.GetMovies(1, IMovieService.DEFAULT_PAGE_SIZE).ToListAsync().ConfigureAwait(false);
      _Print(Target);
      Assert.AreEqual(IMovieService.DEFAULT_PAGE_SIZE, Target.Count);
    }

    [TestMethod]
    public async Task Service_GetLastPage() {
      Console.WriteLine($"Movies count = {MovieService.MoviesCount()}");
      Console.WriteLine($"Pages count = {MovieService.PagesCount(IMovieService.DEFAULT_PAGE_SIZE)}");

      List<IMovie> Target = await MovieService.GetMovies(MovieService.PagesCount(IMovieService.DEFAULT_PAGE_SIZE), IMovieService.DEFAULT_PAGE_SIZE).ToListAsync().ConfigureAwait(false);
      _Print(Target);
      Assert.AreNotEqual(IMovieService.DEFAULT_PAGE_SIZE, Target.Count);
    }

    [TestMethod]
    public async Task Service_GetFilteredFirstPage() {
      List<IMovie> Target = await MovieService.GetMovies("The", 1, IMovieService.DEFAULT_PAGE_SIZE).ToListAsync().ConfigureAwait(false);
      _Print(Target);
      Assert.AreEqual(IMovieService.DEFAULT_PAGE_SIZE, Target.Count);
    }

    [TestMethod]
    public async Task Service_GetFilteredLastPage() {
      Console.WriteLine($"Movies count = {MovieService.MoviesCount("The")}");
      int LastPage = MovieService.PagesCount("The", IMovieService.DEFAULT_PAGE_SIZE);
      Console.WriteLine($"Pages count = {LastPage}");

      List<IMovie> Target = await MovieService.GetMovies("The", LastPage, IMovieService.DEFAULT_PAGE_SIZE).ToListAsync().ConfigureAwait(false);
      _Print(Target);
      Assert.AreNotEqual(IMovieService.DEFAULT_PAGE_SIZE, Target.Count);
    }


    private static void _Print(IEnumerable<IMovie> movies) {
      foreach (IMovie MovieItem in movies) {
        Console.WriteLine(MovieItem.LocalName);
      }
    }

  }
}
