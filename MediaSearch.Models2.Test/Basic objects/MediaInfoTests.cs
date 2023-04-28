namespace MediaSearch.Models2.Test;

[TestClass]
public class MediaInfoTests {

  [TestMethod]
  public void Instanciate_EmptyMediaInfo() {
    Message("Creation of a MediaInfo");
    IMediaInfo Target = new TMediaInfo();
    Assert.IsNotNull(Target);
    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void Instanciate_MediaInfo() {
    Message("Creation of a MediaInfo");
    IMediaInfo Target = new TMediaInfo() {
      Language = ELanguage.English,
      Title = "Star wars - Episode 1",
      Description = "The first episode"
    };
    Assert.IsNotNull(Target);
    Target.Tags.AddRange(new string[] { "Science-fiction", "War", "Space" });

    Dump(Target);

    Ok();
  }
}