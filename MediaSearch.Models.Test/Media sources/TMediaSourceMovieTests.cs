namespace MediaSearch.Models.Test;

[TestClass]
public class TMediaSourceMovieTests {

  [TestMethod]
  public void Instanciate_TMediaSource_Movie() {
    IMediaSource<IMovie> Target = new TMediaSource<IMovie>("\\\\server\\pathname");
    Assert.IsNotNull(Target);
    Assert.AreEqual(EMediaSourceType.Movie, Target.MediaType);
    Assert.AreEqual("\\\\server\\pathname", Target.RootStorage);

    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

}