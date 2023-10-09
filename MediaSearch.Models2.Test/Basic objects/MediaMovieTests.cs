namespace MediaSearch.Models2.Test;

[TestClass]
public class MediaMovieTests {
  [TestMethod]
  public void Instanciate_MediaMovie() {
    Message("Instanciate media movie");
    IMediaMovie Target = new TMediaMovie();
    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void Instanciate_MediaMovieWithValues() {
    Message("Instanciate media movie");
    TMediaInfos MediaInfos = new();
    MediaInfos.Add(
      new TMediaInfo() {
        Language = ELanguage.English,
        Description = "The longuest day",
        Tags = new List<string> { "War" },
        Title = "The longuest day",
        CreationDate = new DateOnly(1966, 11, 25)
      });
    TMediaSources MediaSources = new();
    MediaSources.Add(
      new TMediaSourceVirtual() {
        Languages = new TListWithPrincipal<ELanguage>() { ELanguage.French },
        Description = "Version VFF",
        FileName = "Le jour le plus long (1966)",
        FileExtension = "mkv",
        StorageRoot = @"\\Andromeda\Films",
        StoragePath = @"Guerre\WW2",
        Size = 4_986_321
      },
      new TMediaSourcePhysical() {
        Languages = new TListWithPrincipal<ELanguage>() { ELanguage.French, ELanguage.English },
        StorageType = EStorageType.DVD,
        StoragePlace = "Room 3, war movies",
        Description = "CD zone 2, VFF + US",
        CreationDate = new DateOnly(1998, 7, 23)
      }
    );
    IMediaMovie Target = new TMediaMovie("Le jour le plus long");
    Target.MediaSources.AddRange(MediaSources.GetAll());
    Target.MediaInfos.AddRange(MediaInfos.GetAll());

    Assert.IsNotNull(Target);
    Dump(Target);

    Ok();
  }
}
