namespace MediaSearch.Models2.Test;

[TestClass]
public class TMediaTests {

  [TestMethod]
  public void Instanciate_EmptyMedia() {
    Message("Creation of an empty media");
    IMedia Target = new TMedia();
    Assert.IsNotNull(Target);
    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void Instanciate_Media() {
    Message("Creation of an empty media");
    IMedia Target = new TMedia();

    Message("Creation of a MediaInfo");
    IMediaInfo TargetInfoUS = new TMediaInfo() {
      Language = ELanguage.English,
      Title = "Star wars - Episode 1",
      Description = "The first episode"
    };
    TargetInfoUS.Tags.AddRange(new string[] { "Science-fiction", "War", "Space" });

    Message("Creation of a MediaInfo");
    IMediaInfo TargetInfoFR = new TMediaInfo() {
      Language = ELanguage.English,
      Title = "Star wars - Episode 1",
      Description = "Le premier épisode"
    };
    TargetInfoFR.Tags.AddRange(new string[] { "Science-fiction", "Guerre", "Espace" });
    Dump(TargetInfoFR);

    Message("Adding MediaInfo to Media");
    Target.MediaInfos.Add(TargetInfoUS.Language, TargetInfoUS);
    Target.MediaInfos.Add(TargetInfoFR.Language, TargetInfoFR);
    Dump(Target);

    Assert.AreEqual(2, Target.MediaInfos.Count);

    Ok();
  }
}