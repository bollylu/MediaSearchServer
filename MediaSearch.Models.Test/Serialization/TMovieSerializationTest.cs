using BLTools.Text;

using MediaSearch.Server.Services;

namespace MediaSearch.Models.Test;

[TestClass]
public class TMovieSerializationTest {

  [ClassInitialize]
  public static async Task ClassInitialize(TestContext context) {
    await MediaSearch.Models.GlobalSettings.Initialize().ConfigureAwait(false);
  }

  internal IMovieService MovieService => _MovieService ??= new XMovieService(new XMovieCache() { DataSource = @"data\movies.json" }) { Logger = new TConsoleLogger() };
  private IMovieService? _MovieService;

  [TestMethod]
  public async Task SerializeTMovie() {

    IMovie Source = await MovieService.GetAllMovies().FirstAsync().ConfigureAwait(false);
    TraceMessage("Source", Source);

    string Target = Source.ToJson();

    Assert.IsNotNull(Target);
    TraceMessage("Target", Target);
  }

  [TestMethod]
  public async Task DeserializeTMovie() {
    IMovie Movie = await MovieService.GetAllMovies().FirstAsync().ConfigureAwait(false);

    string Source = Movie.ToJson();
    TraceMessage("Source", Source);

    IMovie? Target = IJson<TMovie>.FromJson(Source);

    Assert.IsNotNull(Target);
    Assert.AreEqual(Movie.Name, Target.Name);
    Assert.AreEqual(Movie.Description, Target.Description);
    Assert.AreEqual(Movie.FileName, Target.FileName);
    Assert.AreEqual(Movie.Size, Target.Size);
    Assert.AreEqual(Movie.Group, Target.Group);
    Assert.AreEqual(Movie.Tags.Count, Target.Tags.Count);
    Assert.AreEqual(Movie.OutputYear, Target.OutputYear);
    TraceMessage("Target", Target);
  }

}

