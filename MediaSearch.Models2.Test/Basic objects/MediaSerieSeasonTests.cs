namespace MediaSearch.Models2.Test;
[TestClass]
public class MediaSerieSeasonTests {
  [TestMethod]
  public void Instanciate_Season() {
    Message("Instanciate season");
    TMediaSerieSeason Target = new TMediaSerieSeason();
    Assert.IsNotNull(Target);
    Dump(Target);
    Ok();
  }

  [TestMethod]
  public void Instanciate_Season_WithValues() {
    Message("Instanciate season");

    TMediaSerieEpisode Ep1 = new TMediaSerieEpisode() {
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
    TMediaSerieEpisode Ep2 = new TMediaSerieEpisode() {
      AbsoluteNumber = 2,
      Number = 2,
      Season = 1,
      SerieType = ESerieType.Anime,
      MediaInfos = new TMediaInfos(
        new TMediaInfo() {
          Title = "Le combat",
          Language = ELanguage.French,
          Description = "Le second épisode de Dragon Ball"
        })
    };

    TMediaSerieSeason Target = new TMediaSerieSeason() {
      SerieType = ESerieType.Anime,
      Number = 1,
      MediaInfos = new TMediaInfos(
          new TMediaInfo() {
            Language = ELanguage.French,
            Title = "La première saison",
            Description = "Ici tout commence"
          })
    };
    Assert.IsNotNull(Target);

    Message("Adding episodes");
    Assert.IsTrue(Target.AddEpisode(Ep1));
    Assert.IsTrue(Target.AddEpisode(Ep2));

    Dump(Target);
    Ok();
  }
}
