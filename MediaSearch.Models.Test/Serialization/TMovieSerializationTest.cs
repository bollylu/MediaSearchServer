using BLTools.Text;

using MediaSearch.Server.Services;

namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TMovieSerializationTest {

  internal IMSDatabase Database => _Database ??= new TMSDatabaseJson("data", "demo");
  private IMSDatabase? _Database;

  [TestMethod]
  public void Serialize() {
    IMovie Source = new TMovie(ELanguage.FrenchFrance, "Un bon film", 1966) { 
      Size=123456789,
      Group="MainGroup/SubGroup",
      DateAdded = DateOnly.Parse("2021-12-25"),
      FileExtension = ".mkv",
      FileName="A good movie.mkv",
      StoragePath="[Action]/MainGroup #/Subgroup #/A good movie",
      StorageRoot="//multimedia.sharenet.priv/multimedia"
    };
    Source.Titles.Add(ELanguage.English, "A good movie");
    Source.Descriptions.Add(ELanguage.English, "This is a good movie");
    Source.Soundtracks.Add(ELanguage.English);
    Source.Soundtracks.Add(ELanguage.FrenchFrance);
    Source.Subtitles.Add(ELanguage.French);
    Source.Tags.Add("Comédie");
    Source.Tags.Add("Action");

    TraceBox($"{nameof(Source)} : {Source.GetType().Name}", Source);

    string Target = Source.ToJson();

    Assert.IsNotNull(Target);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void Deserialize() {
    Assert.IsTrue(Database.Exists());
    Assert.IsTrue(Database.Open());

    IMSTable<IMovie>? MovieTable = Database.GetTable("movies") as IMSTable<IMovie>;
    Assert.IsNotNull(MovieTable);

    IMovie Movie = MovieTable.GetAll(1).Cast<IMovie>().First();
    Assert.IsNotNull(Movie);

    string Source = Movie.ToJson();
    TraceBox($"{nameof(Source)} : {Source.GetType().Name}", Source);

    IMovie? Target = IJson<TMovie>.FromJson(Source);

    Assert.IsNotNull(Target);
    Assert.AreEqual(Movie.Name, Target.Name);
    Assert.AreEqual(Movie.FileName, Target.FileName);
    Assert.AreEqual(Movie.Size, Target.Size);
    Assert.AreEqual(Movie.Group, Target.Group);
    Assert.AreEqual(Movie.Tags.Count, Target.Tags.Count);
    Assert.AreEqual(Movie.CreationYear, Target.CreationYear);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);

    Database.Close();
  }

}

