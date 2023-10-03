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

    TMediaSerieSeason Target = new TMediaSerieSeason() {
      SerieType = ESerieType.Anime,
      Number = 1
    };
    Assert.IsNotNull(Target);
    Target.MediaInfos.Add(
      new TMediaInfo() {
        Language = ELanguage.French,
        Title = "La première saison",
        Description = "Ici tout commence"
      }
    );

    Message("Adding episodes");
    Assert.IsTrue(Target.AddEpisode(DataSourceMedia.MediaSerieEpisode_DragonBall1));
    Assert.IsTrue(Target.AddEpisode(DataSourceMedia.MediaSerieEpisode_DragonBall2));

    Dump(Target);
    Ok();
  }
}
