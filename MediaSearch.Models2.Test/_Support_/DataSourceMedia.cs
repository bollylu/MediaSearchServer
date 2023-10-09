using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MediaSearch.Models2.Test;
internal static class DataSourceMedia {
  static internal List<IMedia> Medias { get; } = new List<IMedia>();

  static internal TMediaInfos MediaInfos_LeJourLePlusLong = new();
  static internal TMediaInfos MediaInfos_StarWars1 = new();
  static internal TMediaInfos MediaInfos_StarWars2 = new();
  static internal TMediaInfos MediaInfos_StarWars3 = new();
  static internal TMediaInfos MediaInfos_StarWars4 = new();

  static internal TMediaSources MediaSources_LeJourLePlusLong = new();

  static internal TMediaMovie MediaMovie_LeJourLePlusLong = new("Le jour le plus long");
  static internal TMediaMovie MediaMovie_StarWars1 = new("star wars 1");
  static internal TMediaMovie MediaMovie_StarWars2 = new("star wars 2");
  static internal TMediaMovie MediaMovie_StarWars3 = new("star wars 3");
  static internal TMediaMovie MediaMovie_StarWars4 = new("Star wars 4 - Invalid entry");

  static internal TMediaInfos MediaInfos_DragonBall_1x01 = new();
  static internal TMediaInfos MediaInfos_DragonBall_1x02 = new();
  static internal TMediaInfos MediaInfos_DragonBall_Season1 = new();
  static internal TMediaInfos MediaInfos_DragonBall = new();

  static internal TMediaSerieEpisode MediaSerieEpisode_DragonBall1 = new() {
    AbsoluteNumber = 1,
    Number = 1,
    Season = 1,
    SerieType = ESerieType.Anime
  };
  static internal TMediaSerieEpisode MediaSerieEpisode_DragonBall2 = new() {
    AbsoluteNumber = 2,
    Number = 2,
    Season = 1,
    SerieType = ESerieType.Anime
  };

  static internal TMediaSerieSeason MediaSerieSeason_DragonBall1 = new TMediaSerieSeason() {
    SerieType = ESerieType.Anime,
    Number = 1
  };

  static internal TMediaSerie MediaSerie_DragonBall = new();

  static DataSourceMedia() {

    #region --- MediaInfos --------------------------------------------
    MediaInfos_LeJourLePlusLong.Add(
      new TMediaInfo() {
        Language = ELanguage.French,
        Description = "Le jour du débarquement",
        Tags = new List<string> { "Guerre" },
        Title = "Le jour le plus long",
        CreationDate = new DateOnly(1966, 11, 25)
      },
      new TMediaInfo() {
        Language = ELanguage.English,
        Description = "The longuest day",
        Tags = new List<string> { "War" },
        Title = "The longuest day",
        CreationDate = new DateOnly(1966, 11, 25)
      }
);

    MediaInfos_StarWars1.Add(
      new TMediaInfo() {
        Language = ELanguage.French,
        Title = "Star wars - Episode 1",
        Description = "Le premier épisode",
        CreationDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-6)),
        Tags = new List<string> { "Science-fiction", "Guerre", "Espace" },
        Groups = new List<string>() { "Star wars" }
      }
    );

    MediaInfos_StarWars2.Add(
      new TMediaInfo() {
        Language = ELanguage.French,
        Title = "Star wars - Episode 2",
        Description = "Le second épisode",
        CreationDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-5)),
        Groups = new List<string>() { "Star wars" },
        Tags = new List<string> { "Science-fiction", "Guerre", "Espace" }
      }
    );

    MediaInfos_StarWars3.Add(
      new TMediaInfo() {
        Language = ELanguage.French,
        Title = "Star wars - Episode 3",
        Description = "Le troisième épisode",
        CreationDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-4)),
        Groups = new List<string>() { "Star wars" },
        Tags = new List<string> { "Science-fiction", "Guerre", "Espace" }
      }
    );

    MediaInfos_DragonBall_1x01.Add(
      new TMediaInfo() {
        Title = "La rencontre",
        Language = ELanguage.French,
        Description = "Le premier épisode de Dragon Ball"
      }
    );

    MediaInfos_DragonBall_1x02.Add(
      new TMediaInfo() {
        Title = "Le combat",
        Language = ELanguage.French,
        Description = "Le second épisode de Dragon Ball"
      }
    );

    MediaInfos_DragonBall_Season1.Add(
     new TMediaInfo() {
       Language = ELanguage.French,
       Title = "La première saison",
       Description = "Ici tout commence"
     }
   );

    MediaInfos_DragonBall.Add(
     new TMediaInfo() {
       Language = ELanguage.French,
       Title = "Dragon Ball",
       Description = "Les aventures de SanGoKu et cie"
     }
   );
    #endregion --- MediaInfos --------------------------------------------

    #region --- MediaSources --------------------------------------------
    MediaSources_LeJourLePlusLong.Add(
      new TMediaSourceVirtual() {
        Languages = new TListWithPrincipal<ELanguage>() { ELanguage.French },
        Description = "Version VFF",
        FileName = "Le jour le plus long (1966)",
        FileExtension = "mkv",
        StorageRoot = @"\\Andromeda\Films",
        StoragePath = @"Guerre\WW2"
      },
      new TMediaSourcePhysical() {
        Languages = new TListWithPrincipal<ELanguage>() { ELanguage.French, ELanguage.English },
        StorageType = EStorageType.DVD,
        StoragePlace = "Room 3, war movies",
        Description = "CD zone 2, VFF + US"
      }
    );
    #endregion --- MediaSources --------------------------------------------

    #region --- MediaMovies --------------------------------------------

    MediaMovie_LeJourLePlusLong.MediaInfos.AddRange(MediaInfos_LeJourLePlusLong.GetAll());
    MediaMovie_LeJourLePlusLong.MediaSources.AddRange(MediaSources_LeJourLePlusLong.GetAll());

    MediaMovie_StarWars1.MediaInfos.AddRange(MediaInfos_StarWars1.GetAll());
    MediaMovie_StarWars2.MediaInfos.AddRange(MediaInfos_StarWars2.GetAll());
    MediaMovie_StarWars3.MediaInfos.AddRange(MediaInfos_StarWars3.GetAll());
    #endregion --- MediaMovies --------------------------------------------

    #region --- MediaSerieEpisodes --------------------------------------------
    MediaSerieEpisode_DragonBall1.MediaInfos.AddRange(MediaInfos_DragonBall_1x01.GetAll());
    MediaSerieEpisode_DragonBall2.MediaInfos.AddRange(MediaInfos_DragonBall_1x02.GetAll());
    #endregion --- MediaSerieEpisodes -----------------------------------------

    #region --- MediaSerieSeasons --------------------------------------------
    MediaSerieSeason_DragonBall1.MediaInfos.AddRange(MediaInfos_DragonBall_Season1.GetAll());
    MediaSerieSeason_DragonBall1.AddEpisode(MediaSerieEpisode_DragonBall1);
    MediaSerieSeason_DragonBall1.AddEpisode(MediaSerieEpisode_DragonBall2);
    #endregion --- MediaSerieSeasons -----------------------------------------

    #region --- MediaSeries --------------------------------------------
    MediaSerie_DragonBall.MediaInfos.AddRange(MediaInfos_DragonBall.GetAll());
    MediaSerie_DragonBall.AddSeason(MediaSerieSeason_DragonBall1);
    #endregion --- MediaSeries --------------------------------------------

    #region --- Medias --------------------------------------------
    Medias.Add(MediaMovie_LeJourLePlusLong);
    Medias.Add(MediaMovie_StarWars1);
    Medias.Add(MediaMovie_StarWars2);
    Medias.Add(MediaMovie_StarWars3);
    Medias.Add(MediaMovie_StarWars4);
    Medias.Add(MediaSerie_DragonBall);
    #endregion --- Medias --------------------------------------------


  }

}
