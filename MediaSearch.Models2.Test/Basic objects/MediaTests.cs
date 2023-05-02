namespace MediaSearch.Models2.Test;

[TestClass]
public class MediaTests {

  [TestMethod]
  public void Instanciate_EmptyMedia() {
    Message("Creation of an empty media");
    IMedia Target = new TMedia();
    Assert.IsNotNull(Target);

    Message("Sources are empty");
    Assert.IsTrue(Target.MediaSources.IsEmpty());

    Message("Pictures are empty");
    Assert.IsTrue(Target.MediaPictures.IsEmpty());

    Message("Infos are empty - Media is invalid");
    Assert.IsTrue(Target.IsInvalid);
    Assert.IsTrue(Target.MediaInfos.IsEmpty());


    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void Instanciate_Media() {
    Message("Creation of an empty media, type movie");
    IMedia Target = new TMedia() { MediaType = EMediaType.Movie };
    Assert.AreEqual(EMediaType.Movie, Target.MediaType);

    Message("Creation of a MediaInfo US");
    IMediaInfo TargetInfoUS = new TMediaInfo() {
      Language = ELanguage.English,
      Title = "Star wars - Episode 1",
      Description = "The first episode"
    };
    TargetInfoUS.Tags.AddRange(new string[] { "Science-fiction", "War", "Space" });
    Dump(TargetInfoUS);

    Message("Creation of a MediaInfo FR");
    IMediaInfo TargetInfoFR = new TMediaInfo() {
      Language = ELanguage.French,
      Title = "Star wars - Episode 1",
      Description = "Le premier épisode"
    };
    TargetInfoFR.Tags.AddRange(new string[] { "Science-fiction", "Guerre", "Espace" });
    Dump(TargetInfoFR);

    Message("Adding MediaInfos to Media");
    Target.MediaInfos.Add(TargetInfoUS.Language, TargetInfoUS);
    Target.MediaInfos.Add(TargetInfoFR.Language, TargetInfoFR);
    Dump(Target);

    Assert.AreEqual(2, Target.MediaInfos.Count);

    Ok();
  }
}