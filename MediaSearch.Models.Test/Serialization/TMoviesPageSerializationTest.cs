using MediaSearch.Server.Services;

namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TMoviesPageSerializationTest {

  internal IMovieService MovieService => _MovieService ??= new XMovieService(new XMovieCache() { DataSource = @"data\movies.json" });
  private IMovieService? _MovieService;

  [TestMethod]
  public async Task Serialize() {

    List<IMovie> Movies = await MovieService.GetAllMovies().Take(2).ToListAsync<IMovie>().ConfigureAwait(false);

    IMoviesPage Source = new TMoviesPage() {
      Name = "Sélection",
      Page = 1,
      AvailablePages = 3,
      AvailableMovies = 3456,
      Source = "Andromeda"
    };
    Source.Movies.AddRange(Movies);

    Dump(Source, "Source");

    string Target = Source.ToJson();

    Assert.IsNotNull(Target);
    Dump(Target, "Target");
  }

  [TestMethod]
  public async Task Deserialize() {
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
    Dump(Source, "Source");

    IMoviesPage? Target = IJson<TMoviesPage>.FromJson(Source);
    Assert.IsNotNull(Target);
    Dump(Target, "Target");
  }

}

