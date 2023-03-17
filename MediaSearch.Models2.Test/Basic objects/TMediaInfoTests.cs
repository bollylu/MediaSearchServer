namespace MediaSearch.Models2.Test;

[TestClass]
public class TMediaInfoTests {

  [TestMethod]
  public void Instanciate_MediaInfo() {
    Message("Creation of a MediaInfo");
    IMediaInfo Target = new TMediaInfo();
    Assert.IsNotNull(Target);
    Dump(Target);

    Ok();
  }
}