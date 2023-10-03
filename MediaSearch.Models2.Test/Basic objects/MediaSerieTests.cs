namespace MediaSearch.Models2.Test;
[TestClass]
public class MediaSerieTests {

  [TestMethod]
  public void Instanciate_Serie() {
    Message("Instanciate serie");
    TMediaSerie Target = new TMediaSerie();
    Assert.IsNotNull(Target);
    Dump(Target);
    Ok();
  }

  [TestMethod]
  public void Instanciate_Serie_WithValues() {

    Message("Instanciate serie");
    TMediaSerie Target = new TMediaSerie() {
      SerieType = ESerieType.Anime,
    };
    Target.MediaInfos.AddRange(DataSourceMedia.MediaInfos_DragonBall.GetAll());
    Dump(Target);

    Message($"Adding {nameof(DataSourceMedia.MediaSerieSeason_DragonBall1)}");
    Assert.IsTrue(Target.AddSeason(DataSourceMedia.MediaSerieSeason_DragonBall1));
    Dump(Target);

    Assert.AreEqual(1, Target.GetSeasons().Count());

    Message("Retrieving season 1");
    IMediaSerieSeason? TargetSeason1 = Target.GetSeason(1);
    Assert.IsNotNull(TargetSeason1);
    Assert.AreEqual(2, TargetSeason1.GetEpisodes().Count());
    Dump(TargetSeason1);

    Ok();
  }

  [TestMethod]
  public void Instanciate_Serie_DuplicateSeason_Error() {
    Message("Instanciate serie");
    TMediaSerie Target = new TMediaSerie() {
      SerieType = ESerieType.Anime
    };
    Assert.IsNotNull(Target);
    Dump(Target);

    Message($"Adding {nameof(DataSourceMedia.MediaSerieSeason_DragonBall1)}");
    Assert.IsTrue(Target.AddSeason(DataSourceMedia.MediaSerieSeason_DragonBall1));
    Dump(Target);

    Message($"Second attempt to add {nameof(DataSourceMedia.MediaSerieSeason_DragonBall1)}, will fail");
    Assert.IsFalse(Target.AddSeason(DataSourceMedia.MediaSerieSeason_DragonBall1));

    Ok();
  }

  [TestMethod]
  public void Instanciate_Serie_InvalidSeasonNumber_Error() {
    Message("Instanciate serie");
    TMediaSerie Target = new TMediaSerie() {
      SerieType = ESerieType.Anime
    };

    Dump(Target);

    TMediaSerieSeason InvalidSeason = new TMediaSerieSeason(DataSourceMedia.MediaSerieSeason_DragonBall1);
    InvalidSeason.Number = -1;
    Message($"Attempt to add {nameof(InvalidSeason)}, will fail");
    Assert.IsFalse(Target.AddSeason(InvalidSeason));

    Ok();
  }

  [TestMethod]
  public void Instanciate_Serie_InvalidSeasonType_Error() {
    Message("Instanciate serie");
    TMediaSerie Target = new TMediaSerie() {
      SerieType = ESerieType.TvShow
    };

    Dump(Target);

    Message($"Attempt to add {nameof(DataSourceMedia.MediaSerieSeason_DragonBall1)}, will fail because of unmatching serie type");
    Assert.IsFalse(Target.AddSeason(DataSourceMedia.MediaSerieSeason_DragonBall1));

    Ok();
  }

  [TestMethod]
  public void RetrieveSeasonFromSerie_InvalidNumber_Error() {

    Message($"Attempt to retrieve season with invalid number, will fail");
    Assert.IsNull(DataSourceMedia.MediaSerie_DragonBall.GetSeason(-3));

    Message($"Attempt to retrieve season with non-existent number, will fail");
    Assert.IsNull(DataSourceMedia.MediaSerie_DragonBall.GetSeason(7));

    Ok();
  }
}
