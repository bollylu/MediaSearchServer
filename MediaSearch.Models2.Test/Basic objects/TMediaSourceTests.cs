namespace MediaSearch.Models2.Test;

[TestClass]
public class TMediaSourceTests {

  [TestMethod]
  public void Instanciate_MediaSource() {
    Message("Creation of a MediaSource");
    IMediaSource Target = new TMediaSource();
    Assert.IsNotNull(Target);
    Dump(Target);

    Ok();
  }
}