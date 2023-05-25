namespace MediaSearch.Models2.Test;
[TestClass]
public class MediaSerieEpisodeTests {
  [TestMethod]
  public void Instanciate_Episode() {
    Message("Instanciate episode");
    TMediaSerieEpisode Target = new TMediaSerieEpisode();
    Assert.IsNotNull(Target);
    Dump(Target);
    Ok();
  }

  [TestMethod]
  public void Instanciate_Episode_WithValues() {
    Message("Instanciate episode");
    TMediaSerieEpisode Target = new TMediaSerieEpisode() {
      AbsoluteNumber = 1,
      Number = 1,
      Season = 1,
      SerieType = ESerieType.Anime,
      MediaInfos = new TMediaInfos(
        new TMediaInfo() {
          Title = "La rencontre",
          Language = ELanguage.French,
          Description = "Le premier épisode de Dragon Ball"
        })
    };
    Assert.IsNotNull(Target);
    Dump(Target);
    Ok();
  }
}
