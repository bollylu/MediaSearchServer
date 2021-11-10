namespace MovieSearchTest;

[TestClass]
public class TMovieSerializationTest {

  private IMovieService _MovieService;

  [TestInitialize]
  public void BuildData() {
    IMovieCache Cache = new XMovieCache() { Storage = @"\\Andromeda.sharenet.priv\films" };
    IEnumerable<IFileInfo> Files = Cache.FetchFiles();
    Cache.Parse(Files, CancellationToken.None);
    _MovieService = new TMovieService(Cache);
  }

  [TestMethod]
  public async Task SerializeTMovie() {
    IMovie Source = await _MovieService.GetAllMovies().FirstAsync();

    string JsonMovie = Source.ToJson();

    Assert.IsNotNull(JsonMovie);
    Console.WriteLine(JsonMovie);
  }

  [TestMethod]
  public async Task DeserializeTMovie() {
    IMovie Source = await _MovieService.GetAllMovies().FirstAsync();

    string JsonMovie = Source.ToJson();

    IMovie Target = TMovie.FromJson(JsonMovie);

    Assert.IsNotNull(Target);
    Assert.AreEqual(Source.Name, Target.Name);
    Assert.AreEqual(Source.Description, Target.Description);
    Assert.AreEqual(Source.FileName, Target.FileName);
    Assert.AreEqual(Source.Size, Target.Size);
    Assert.AreEqual(Source.Group, Target.Group);
    Assert.AreEqual(Source.Tags.Count, Target.Tags.Count);
    Assert.AreEqual(Source.OutputYear, Target.OutputYear);
    Console.WriteLine(Target.ToString());
  }

  [TestMethod]
  public async Task SerializeTMovies() {

    List<IMovie> Source = await _MovieService.GetAllMovies().ToListAsync<IMovie>();

    IMoviesPage Movies = new TMoviesPage() {
      Name = "Sélection",
      Page = 1,
      AvailablePages = 3,
      Source = "Andromeda"
    };
    Movies.Movies.AddRange(Source);

    string JsonMovies = Movies.ToJson(new JsonWriterOptions() { Indented = true });

    Assert.IsNotNull(JsonMovies);
    Console.WriteLine(JsonMovies);
  }

  [TestMethod]
  public async Task DeserializeTMovies() {
    List<IMovie> Source = await _MovieService.GetAllMovies().ToListAsync<IMovie>();

    IMoviesPage Movies = new TMoviesPage() {
      Name = "Sélection",
      Page = 1,
      AvailablePages = 3,
      Source = "Andromeda"
    };
    Movies.Movies.AddRange(Source);

    string JsonMovies = Movies.ToJson(new JsonWriterOptions() { Indented = true });
    Assert.IsNotNull(JsonMovies);
    Console.WriteLine(JsonMovies);

    IMoviesPage Target = TMoviesPage.FromJson(JsonMovies);
    Console.WriteLine(Target.ToString());
  }

}

