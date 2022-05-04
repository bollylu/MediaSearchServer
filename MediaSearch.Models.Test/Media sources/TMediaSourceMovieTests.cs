namespace MediaSearch.Models.Test;

[TestClass]
public class TMediaSourceMovieTests {

  [TestMethod]
  public void Instanciate_TMediaSource_Movie() {
    IMediaSource Target = new TMediaSource(EMediaSourceType.Movie, "\\\\server\\pathname");
    Assert.IsNotNull(Target);
    Assert.AreEqual(EMediaSourceType.Movie, Target.MediaType);
    Assert.AreEqual("\\\\server\\pathname", Target.RootStorage);

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

}