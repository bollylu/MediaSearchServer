namespace MediaSearch.Models2.Test;

[TestClass]
public class MediaSourceTests {

  [TestMethod]
  public void Instanciate_MediaSourceVirtual() {
    Message("Creation of a MediaSource virtual");
    IMediaSource Target = new TMediaSourceVirtual();
    Assert.IsNotNull(Target);
    Assert.IsInstanceOfType(Target, typeof(IMediaSource));
    Assert.IsInstanceOfType(Target, typeof(IMediaSourceVirtual));
    Assert.IsInstanceOfType(Target, typeof(TMediaSourceVirtual));
    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void Instanciate_MediaSourcePhysical() {
    Message("Creation of a MediaSource physcal");
    IMediaSource Target = new TMediaSourcePhysical();
    Assert.IsNotNull(Target);
    Assert.IsInstanceOfType(Target, typeof(IMediaSource));
    Assert.IsInstanceOfType(Target, typeof(IMediaSourcePhysical));
    Assert.IsInstanceOfType(Target, typeof(TMediaSourcePhysical));
    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void MediaSourceVirtual_WithData() {
    Message("Creation of a MediaSource virtual");
    IMediaSource Target = new TMediaSourceVirtual() {
      Languages = new TListWithPrincipal<ELanguage>() { ELanguage.French },
      Description = "Version VFF",
      FileName = "Le jour le plus long (1966)",
      FileExtension = "mkv",
      StorageRoot = @"\\Andromeda\Films",
      StoragePath = @"Guerre\WW2",
      Size = 12345678,
      CreationDate = new DateOnly(1966, 1, 1)
    };
    Assert.IsNotNull(Target);

    Dump(Target);

    Ok();
  }
}