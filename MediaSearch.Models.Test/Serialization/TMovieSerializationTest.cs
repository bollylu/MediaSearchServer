namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TMovieSerializationTest {

  //internal IMSDatabase Database => _Database ??= new TMSDatabaseJson("data", "demo");
  //private IMSDatabase? _Database;

  [TestMethod]
  public void Serialize() {
    IMovie Source = new TMovie(ELanguage.FrenchFrance, "Un bon film", 1966) {
      Size = 123456789,
      Group = "MainGroup/SubGroup",
      DateAdded = DateOnly.Parse("2021-12-25"),
      FileExtension = ".mkv",
      FileName = "A good movie.mkv",
      StoragePath = "[Action]/MainGroup #/Subgroup #/A good movie",
      StorageRoot = "//multimedia.sharenet.priv/multimedia"
    };
    Source.Titles.Add(ELanguage.English, "A good movie");
    Source.Descriptions.Add(ELanguage.English, "This is a good movie");
    Source.Soundtracks.Add(ELanguage.English);
    Source.Soundtracks.Add(ELanguage.FrenchFrance);
    Source.Subtitles.Add(ELanguage.French);
    Source.Tags.Add("Comédie");
    Source.Tags.Add("Action");

    Dump(Source);

    string Target = IJson.ToJson(Source);

    Assert.IsNotNull(Target);
    Dump(Target);
  }

  [TestMethod]
  public void Deserialize() {
    IMovie Source = new TMovie(ELanguage.FrenchFrance, "Un bon film", 1966) {
      Size = 123456789,
      Group = "MainGroup/SubGroup",
      DateAdded = DateOnly.Parse("2021-12-25"),
      FileExtension = ".mkv",
      FileName = "A good movie.mkv",
      StoragePath = "[Action]/MainGroup #/Subgroup #/A good movie",
      StorageRoot = "//multimedia.sharenet.priv/multimedia"
    };
    Source.Titles.Add(ELanguage.English, "A good movie");
    Source.Descriptions.Add(ELanguage.English, "This is a good movie");
    Source.Soundtracks.Add(ELanguage.English);
    Source.Soundtracks.Add(ELanguage.FrenchFrance);
    Source.Subtitles.Add(ELanguage.French);
    Source.Tags.Add("Comédie");
    Source.Tags.Add("Action");

    Dump(Source);

    string JsonSource = IJson.ToJson(Source);
    Dump(JsonSource);

    IMovie? Target = IJson.FromJson<TMovie>(JsonSource);

    Assert.IsNotNull(Target);
    Dump(Target);

    Assert.AreEqual(Source.Name, Target.Name);
    Assert.AreEqual(Source.FileName, Target.FileName);
    Assert.AreEqual(Source.Size, Target.Size);
    Assert.AreEqual(Source.Group, Target.Group);
    Assert.AreEqual(Source.Tags.Count, Target.Tags.Count);
    Assert.AreEqual(Source.CreationYear, Target.CreationYear);

  }

}

