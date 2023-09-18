using MediaSearch.Models.Support;

namespace MediaSearch.Models2.Test.ffprobe;
[TestClass]
public class ffprobeTest {

  [TestMethod]
  public async Task GetFFPROBE_Version() {
    Message("Reading version of ffprobe");
    IPropertiesFinder FFProbe = new TFFProbe("");
    Assert.IsNotNull(FFProbe);
    Dump(FFProbe);

    string Target = await FFProbe.GetVersion();

    Assert.IsNotNull(Target);
    Dump(Target);

    Assert.AreEqual("6.0", Target);
    Ok();
  }

  [TestMethod]
  public async Task GetFFPROBE_Streams() {
    Message("Reading streams");
    IPropertiesFinder FFProbe = new TFFProbe(@".\Data\films\La bamba (1987)\La bamba (1987).mkv");
    Assert.IsNotNull(FFProbe);
    Dump(FFProbe);
    await FFProbe.Init();

    Message("--- Audio");
    foreach (var kvp in FFProbe.GetAudioStreams()) {
      Dump(kvp.Key);
      Dump(kvp.Value);
    }

    Message("--- Video");
    foreach (var kvp in FFProbe.GetVideoStreams()) {
      Dump(kvp.Key);
      Dump(kvp.Value);
    }

    Message("--- SubTitles");
    foreach (var kvp in FFProbe.GetSubTitleStreams()) {
      Dump(kvp.Key);
      Dump(kvp.Value);
    }

    Ok();
  }
}
