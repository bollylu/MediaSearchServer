using System.Collections.Generic;

namespace MediaSearch.Models2.Test;
internal static class DataSourceMedia {
  static internal List<IMedia> Medias { get; } = new List<IMedia>();

  static DataSourceMedia() {
    Medias.Add(new TMedia(
      "Le jour le plus long",
      new TMediaInfos(
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
      new TMediaSources()
    ) {
      CreationDate = new DateOnly(1966, 11, 25),
    });
  }

}
