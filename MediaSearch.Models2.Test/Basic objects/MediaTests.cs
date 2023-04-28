namespace MediaSearch.Models2.Test;

[TestClass]
public class MediaTests {

  [TestMethod]
  public void Instanciate_EmptyMedia() {
    Message("Creation of an empty media");
    IMedia Target = new TMedia();
    Assert.IsNotNull(Target);

    Message("Sources is empty");
    Assert.IsTrue(Target.MediaSources.IsEmpty());

    Message("MediaInfos is empty");
    Assert.IsTrue(Target.MediaInfos.IsEmpty());

    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void Instanciate_Media() {
    Message("Creation of an empty media");
    IMedia Target = new TMedia();

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
      Description = "Le premier �pisode"
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