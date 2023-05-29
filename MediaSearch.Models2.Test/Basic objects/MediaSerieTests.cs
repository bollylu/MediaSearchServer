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
    Message("Instanciate episodes");
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
    Dump(Ep1);
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
    Dump(Ep2);

    Message("Adding episodes to season");
    TMediaSerieSeason Season1 = new TMediaSerieSeason() {
      SerieType = ESerieType.Anime,
      Number = 1,
      Infos = new TMediaInfos(
          new TMediaInfo() {
            Language = ELanguage.French,
            Title = "La première saison",
            Description = "Ici tout commence"
          })
    };
    Assert.IsTrue(Season1.AddEpisode(Ep1));
    Assert.IsTrue(Season1.AddEpisode(Ep2));
    Dump(Season1);

    Message("Instanciate MediaInfo");
    TMediaInfo InfoFR = new TMediaInfo() {
      Language = ELanguage.French,
      Title = "Dragon Ball",
      Description = "Les aventures de SanGoKu et cie"
    };
    Dump(InfoFR);

    Message("Instanciate serie");
    TMediaSerie Target = new TMediaSerie() {
      SerieType = ESerieType.Anime,
      MediaInfos = new TMediaInfos(InfoFR)
    };

    Dump(Target);

    Message($"Adding {nameof(Season1)}");
    Assert.IsTrue(Target.AddSeason(Season1));
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
    Message("Instanciate episodes");
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

    Message("Adding episodes to season");
    TMediaSerieSeason Season1 = new TMediaSerieSeason() {
      SerieType = ESerieType.Anime,
      Number = 1,
      Infos = new TMediaInfos(
          new TMediaInfo() {
            Language = ELanguage.French,
            Title = "La première saison",
            Description = "Ici tout commence"
          })
    };
    Assert.IsTrue(Season1.AddEpisode(Ep1));
    Assert.IsTrue(Season1.AddEpisode(Ep2));

    Message("Instanciate MediaInfo");
    TMediaInfo InfoFR = new TMediaInfo() {
      Language = ELanguage.French,
      Title = "Dragon Ball",
      Description = "Les aventures de SanGoKu et cie"
    };

    Message("Instanciate serie");
    TMediaSerie Target = new TMediaSerie() {
      SerieType = ESerieType.Anime,
      MediaInfos = new TMediaInfos(InfoFR)
    };

    Dump(Target);

    Message($"Adding {nameof(Season1)}");
    Assert.IsTrue(Target.AddSeason(Season1));
    Dump(Target);

    Message($"Second attempt to add {nameof(Season1)}, will fail");
    Assert.IsFalse(Target.AddSeason(Season1));

    Ok();
  }

  [TestMethod]
  public void Instanciate_Serie_InvalidSeasonNumber_Error() {
    Message("Instanciate episodes");
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

    Message("Adding episodes to season");
    TMediaSerieSeason Season1 = new TMediaSerieSeason() {
      SerieType = ESerieType.Anime,
      Number = -1,
      Infos = new TMediaInfos(
          new TMediaInfo() {
            Language = ELanguage.French,
            Title = "La première saison",
            Description = "Ici tout commence"
          })
    };
    Assert.IsTrue(Season1.AddEpisode(Ep1));
    Assert.IsTrue(Season1.AddEpisode(Ep2));

    Message("Instanciate MediaInfo");
    TMediaInfo InfoFR = new TMediaInfo() {
      Language = ELanguage.French,
      Title = "Dragon Ball",
      Description = "Les aventures de SanGoKu et cie"
    };

    Message("Instanciate serie");
    TMediaSerie Target = new TMediaSerie() {
      SerieType = ESerieType.Anime,
      MediaInfos = new TMediaInfos(InfoFR)
    };

    Dump(Target);

    Message($"Attempt to add {nameof(Season1)}, will fail");
    Assert.IsFalse(Target.AddSeason(Season1));

    Ok();
  }

  [TestMethod]
  public void Instanciate_Serie_InvalidSeasonType_Error() {
    Message("Instanciate episodes");
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

    Message("Adding episodes to season");
    TMediaSerieSeason Season1 = new TMediaSerieSeason() {
      SerieType = ESerieType.Anime,
      Number = 1,
      Infos = new TMediaInfos(
          new TMediaInfo() {
            Language = ELanguage.French,
            Title = "La première saison",
            Description = "Ici tout commence"
          })
    };
    Assert.IsTrue(Season1.AddEpisode(Ep1));
    Assert.IsTrue(Season1.AddEpisode(Ep2));

    Message("Instanciate MediaInfo");
    TMediaInfo InfoFR = new TMediaInfo() {
      Language = ELanguage.French,
      Title = "Dragon Ball",
      Description = "Les aventures de SanGoKu et cie"
    };

    Message("Instanciate serie");
    TMediaSerie Target = new TMediaSerie() {
      SerieType = ESerieType.TvShow,
      MediaInfos = new TMediaInfos(InfoFR)
    };

    Dump(Target);

    Message($"Attempt to add {nameof(Season1)}, will fail because of unmatching serie type");
    Assert.IsFalse(Target.AddSeason(Season1));

    Ok();
  }

  [TestMethod]
  public void RetrieveSeasonFromSerie_InvalidNumber_Error() {
    Message("Instanciate episodes");
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

    Message("Adding episodes to season");
    TMediaSerieSeason Season1 = new TMediaSerieSeason() {
      SerieType = ESerieType.Anime,
      Number = 1,
      Infos = new TMediaInfos(
          new TMediaInfo() {
            Language = ELanguage.French,
            Title = "La première saison",
            Description = "Ici tout commence"
          })
    };
    Assert.IsTrue(Season1.AddEpisode(Ep1));
    Assert.IsTrue(Season1.AddEpisode(Ep2));

    Message("Instanciate MediaInfo");
    TMediaInfo InfoFR = new TMediaInfo() {
      Language = ELanguage.French,
      Title = "Dragon Ball",
      Description = "Les aventures de SanGoKu et cie"
    };

    Message("Instanciate serie");
    TMediaSerie Target = new TMediaSerie() {
      SerieType = ESerieType.Anime,
      MediaInfos = new TMediaInfos(InfoFR)
    };

    Message($"Adding {nameof(Season1)}");
    Assert.IsTrue(Target.AddSeason(Season1));

    Dump(Target);

    Message($"Attempt to retrieve season with invalid number, will fail");
    Assert.IsNull(Target.GetSeason(-3));

    Message($"Attempt to retrieve season with non-existent number, will fail");
    Assert.IsNull(Target.GetSeason(7));

    Ok();
  }
}
