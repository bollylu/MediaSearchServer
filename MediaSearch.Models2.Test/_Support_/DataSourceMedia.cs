using System.Collections.Generic;

namespace MediaSearch.Models2.Test;
internal static class DataSourceMedia {
  static internal List<IMedia> Medias { get; } = new List<IMedia>();

  static DataSourceMedia() {
    Medias.Add(new TMediaMovie("Le jour le plus long") {
      MediaInfos = new TMediaInfos(
        new TMediaInfo() {
          Language = ELanguage.French,
          Description = "Le jour du débarquement",
          Tags = new List<string> { "Guerre" },
          Title = "Le jour le plus long"
        },
        new TMediaInfo() {
          Language = ELanguage.English,
          Description = "The longuest day",
          Tags = new List<string> { "War" },
          Title = "The longuest day"
        }
      ),
      MediaSources = new TMediaSources(
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
      ),
      CreationDate = new DateOnly(1966, 11, 25)
    });

    Medias.Add(new TMediaMovie("Star wars 1") {
      MediaInfos = new TMediaInfos(
        new TMediaInfo() {
          Language = ELanguage.French,
          Title = "Star wars - Episode 1",
          Description = "Le premier épisode",
          CreationDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-6)),
          Group = "Star wars",
          Tags = new List<string> { "Science-fiction", "Guerre", "Espace" }
        }
      )
    });

    Medias.Add(new TMediaMovie("Star wars 2") {
      MediaType = EMediaType.Movie,
      MediaInfos = new TMediaInfos(
        new TMediaInfo() {
          Language = ELanguage.French,
          Title = "Star wars - Episode 2",
          Description = "Le second épisode",
          CreationDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-5)),
          Group = "Star wars",
          Tags = new List<string> { "Science-fiction", "Guerre", "Espace" }
        }
      )
    });

    Medias.Add(new TMediaMovie("Star wars 3") {
      MediaInfos = new TMediaInfos(
       new TMediaInfo() {
         Language = ELanguage.French,
         Title = "Star wars - Episode 3",
         Description = "Le troisième épisode",
         CreationDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-4)),
         Group = "Star wars",
         Tags = new List<string> { "Science-fiction", "Guerre", "Espace" }
       }
     )
    });

    Medias.Add(new TMediaMovie("Star wars 4 - Invalid entry"));
  }

}
