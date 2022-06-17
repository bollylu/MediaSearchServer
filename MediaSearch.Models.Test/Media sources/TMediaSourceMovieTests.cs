namespace MediaSearch.Models.Test;

[TestClass]
public class TMediaSourceMovieTests {

  [TestMethod]
  public void Instanciate_TMediaSource_Movie() {
    IMediaSource<IMovie> Target = new TMediaSourceMovie("\\\\server\\pathname");
    Assert.IsNotNull(Target);
    Assert.AreEqual(typeof(IMovie), Target.MediaType);
    Assert.AreEqual("\\\\server\\pathname", Target.RootStorage);

    Dump(Target);
  }

}