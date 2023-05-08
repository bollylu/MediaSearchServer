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

    Message("Obtaining a media from data source");
    IMedia Target = DataSourceMedia.Medias.First();
    Assert.IsNotNull(Target);

    Dump(Target);

    Assert.AreEqual(2, Target.MediaInfos.Count);

    Ok();
  }
}