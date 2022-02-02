using BLTools.Text;

using MediaSearch.Server.Services;

namespace MediaSearch.Models.Test;

[TestClass]
public class TMoviesPageSerializationTest {

  internal IMovieService MovieService => _MovieService ??= new XMovieService(new XMovieCache() { DataSource = @"data\movies.json" }) { Logger = new TConsoleLogger() };
  private IMovieService? _MovieService;

  [TestMethod]
  public async Task SerializeTMoviesPageWithConverter() {

    List<IMovie> Source = await MovieService.GetAllMovies().ToListAsync<IMovie>().ConfigureAwait(false);

    TMoviesPage Movies = new TMoviesPage() {
      Name = "Sélection",
      Page = 1,
      AvailablePages = 3,
      Source = "Andromeda"
    };
    Movies.Movies.AddRange(Source);

    string JsonMovies = Movies.ToJson();

    Assert.IsNotNull(JsonMovies);
    Console.WriteLine(JsonMovies);
  }

  [TestMethod]
  public async Task DeserializeTMoviesPageWithConverter() {
    List<IMovie> Source = await MovieService.GetAllMovies().ToListAsync<IMovie>().ConfigureAwait(false);

    TMoviesPage Movies = new TMoviesPage() {
      Name = "Sélection",
      Page = 1,
      AvailablePages = 3,
      AvailableMovies = 3456,
      Source = "Andromeda"
    };
    Movies.Movies.AddRange(Source);

    string JsonMovies = Movies.ToJson();

    Assert.IsNotNull(JsonMovies);
    Console.WriteLine(JsonMovies);

    IMoviesPage? Target = TMoviesPage.FromJson(JsonMovies);
    Console.WriteLine(Target?.ToString().BoxFixedWidth(120));
  }

}

