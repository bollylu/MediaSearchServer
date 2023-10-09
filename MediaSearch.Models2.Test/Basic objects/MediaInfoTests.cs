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
      Language = ELanguage.French,
      Title = "Star wars - Episode 1",
      Description = "Le premier épisode",
      CreationDate = DateOnly.FromDateTime(DateTime.Now),
      Tags = new List<string> { "Science-fiction", "Guerre", "Espace" }
    };
    Assert.IsNotNull(Target);
    Target.Groups.Add("Star wars");
    Dump(Target);

    Ok();
  }
}