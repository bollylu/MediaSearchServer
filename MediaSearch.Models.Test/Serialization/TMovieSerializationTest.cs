using BLTools.Text;

using MediaSearch.Server.Services;

namespace MediaSearch.Models.Test;

[TestClass]
public class TMovieSerializationTest {

  private IMovieService _MovieService = new TMovieService();

  [TestInitialize]
  public async Task BuildData() {
    IMovieCache Cache = new XMovieCache() { Logger = new TConsoleLogger(), DataSource = @"data\movies.json" };
    await Cache.Parse(CancellationToken.None);
    _MovieService = new TMovieService(Cache);
  }

  [TestMethod]
  public async Task SerializeTMovie() {
    TMovie Source = await _MovieService.GetAllMovies().FirstAsync();

    string JsonMovie = Source.ToJson();

    Assert.IsNotNull(JsonMovie);
    Console.WriteLine(JsonMovie.ToString().BoxFixedWidth(120));
  }

  [TestMethod]
  public async Task DeserializeTMovie() {
    TMovie Source = await _MovieService.GetAllMovies().FirstAsync();

    string JsonMovie = Source.ToJson();
    Console.WriteLine(JsonMovie.BoxFixedWidth(120));

    IMovie? Target = TMovie.FromJson(JsonMovie);

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

}

