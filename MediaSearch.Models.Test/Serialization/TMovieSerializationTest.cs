using BLTools.Text;

using MediaSearch.Server.Services;

namespace MediaSearch.Models.Test;

[TestClass]
public class TMovieSerializationTest {

  internal IMovieService MovieService => _MovieService ??= new XMovieService(new XMovieCache() { DataSource = @"data\movies.json" }) { Logger = new TConsoleLogger() };
  private IMovieService? _MovieService;

  [TestMethod]
  public async Task SerializeTMovie() {
    TMovie Source = await MovieService.GetAllMovies().FirstAsync();

    string JsonMovie = Source.ToJson();

    Assert.IsNotNull(JsonMovie);
    Console.WriteLine(JsonMovie.ToString().BoxFixedWidth(120));
  }

  [TestMethod]
  public async Task DeserializeTMovie() {
    TMovie Source = await MovieService.GetAllMovies().FirstAsync();

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

