using MediaSearch.Server.Services;

namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TMovieSerializationTest {

  internal IMovieService MovieService => _MovieService ??= new XMovieService(new XMovieCache() { DataSource = @"data\movies.json" });
  private IMovieService? _MovieService;

  [TestMethod]
  public async Task Serialize() {

    IMovie Source = await MovieService.GetAllMovies().FirstAsync().ConfigureAwait(false);
    Dump(Source, "Source");

    string Target = Source.ToJson();

    Assert.IsNotNull(Target);
    Dump(Target, "Target");
  }

  [TestMethod]
  public async Task Deserialize() {
    IMovie Movie = await MovieService.GetAllMovies().FirstAsync().ConfigureAwait(false);

    string Source = Movie.ToJson();
    Dump(Source, "Source");

    IMovie? Target = IJson<TMovie>.FromJson(Source);

    Assert.IsNotNull(Target);
    Assert.AreEqual(Movie.Name, Target.Name);
    //Assert.AreEqual(Movie.Description, Target.Description);
    //Assert.AreEqual(Movie.FileName, Target.FileName);
    //Assert.AreEqual(Movie.Size, Target.Size);
    Assert.AreEqual(Movie.Group, Target.Group);
    Assert.AreEqual(Movie.Tags.Count, Target.Tags.Count);
    Assert.AreEqual(Movie.OutputYear, Target.OutputYear);
    TraceMessage("Target", Target);
  }

}

