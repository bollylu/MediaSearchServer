using BLTools.Text;

using MediaSearch.Server.Services;

namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TMovieSerializationTest {

  internal IMovieService MovieService => _MovieService ??= new XMovieService(new XMovieCache() { DataSource = @"data\movies.json" });
  private IMovieService? _MovieService;

  [TestMethod]
  public void Serialize() {
    IMovie Source = new TMovie() { 
      Name="A good movie",
      Description="This is a good movie",
      Size=123456789,
      Group="MainGroup/SubGroup",
      CreationYear = 1966,
      DateAdded = DateOnly.Parse("2021-12-25"),
      FileExtension = ".mkv",
      FileName="A good movie.mkv",
      StoragePath="[Action]/MainGroup #/Subgroup #/A good movie",
      StorageRoot="//multimedia.sharenet.priv/multimedia"
    };
    Source.Titles.Add(ELanguage.FrenchFrance, "Un bon film");
    Source.Titles.Add(ELanguage.English, "A good movie");
    Source.Soundtracks.Add(ELanguage.English);
    Source.Soundtracks.Add(ELanguage.FrenchFrance);
    Source.Subtitles.Add(ELanguage.French);
    Source.Tags.Add("Comédie");
    Source.Tags.Add("Action");

    TraceMessage("Source", Source);

    string Target = Source.ToJson();

    Assert.IsNotNull(Target);
    TraceMessage("Target", Target);
  }

  [TestMethod]
  public async Task Deserialize() {
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
    Assert.AreEqual(Movie.CreationYear, Target.CreationYear);
    TraceMessage("Target", Target);
  }

}

