using BLTools.Text;

using MediaSearch.Server.Services;
using MediaSearch.Test.Support;

namespace MediaSearch.Models.Test;

[TestClass]
public class TMoviesPageSerializationTest {

  [ClassInitialize]
  public static async Task ClassInitialize(TestContext context) {
    await MediaSearch.Models.GlobalSettings.Initialize().ConfigureAwait(false);
  }

  internal IMovieService MovieService => _MovieService ??= new XMovieService(new XMovieCache() { DataSource = @"data\movies.json" }) { Logger = new TConsoleLogger() };
  private IMovieService? _MovieService;

  [TestMethod]
  public async Task SerializeTMoviesPageWithConverter() {

    List<IMovie> Movies = await MovieService.GetAllMovies().Take(2).ToListAsync<IMovie>().ConfigureAwait(false);

    IMoviesPage Source = new TMoviesPage() {
      Name = "Sélection",
      Page = 1,
      AvailablePages = 3,
      AvailableMovies = 3456,
      Source = "Andromeda"
    };
    Source.Movies.AddRange(Movies);

    TraceMessage("Source", Source);

    string Target = Source.ToJson();

    Assert.IsNotNull(Target);
    TraceMessage("Target", Target);
  }

  [TestMethod]
  public async Task DeserializeTMoviesPageWithConverter() {
    List<IMovie> Movies = await MovieService.GetAllMovies().Take(2).ToListAsync<IMovie>().ConfigureAwait(false);

    IMoviesPage MoviesPage = new TMoviesPage() {
      Name = "Sélection",
      Page = 1,
      AvailablePages = 3,
      AvailableMovies = 3456,
      Source = "Andromeda"
    };
    MoviesPage.Movies.AddRange(Movies);

    string Source = MoviesPage.ToJson();
    TraceMessage("Source", Source);

    IMoviesPage? Target = IJson<TMoviesPage>.FromJson(Source);
    Assert.IsNotNull(Target);
    TraceMessage("Target", Target);
  }

}

