using MediaSearch.Models.Support;

namespace MediaSearch.Models2.Test.ffprobe;
[TestClass]
public class ffprobeTest {

  private const string DATA_SOURCE_FOLDER = @"\\andromeda.sharenet.priv\Films\Comédie";

  [TestMethod]
  public async Task GetFFPROBE_Version() {
    Message("Reading version of ffprobe");
    IMediaSourceStreamsFinder FFProbe = new TFFProbe("");
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
    Message($"Reading streams in {DATA_SOURCE_FOLDER.WithQuotes()}");

    List<IMediaSourceStreamsFinder> Probes = new();
    foreach (string MovieItem in Directory.EnumerateDirectories(DATA_SOURCE_FOLDER)) {
      string? MovieFile = Directory.EnumerateFiles(MovieItem).FirstOrDefault(m => !m.EndsWith(".jpg"));
      if (MovieFile is null) {
        continue;
      }
      Probes.Add(new TFFProbe(MovieFile));
    }

    Message("Processing files...");

    await Parallel.ForEachAsync(Probes, new ParallelOptions { MaxDegreeOfParallelism = 15 }, async (finder, token) => {
      Message($"Init {finder.Filename.WithQuotes()}");
      await finder.Init();
    });

    Message("Data is gathered.");

    foreach (IMediaSourceStreamsFinder FinderItem in Probes) {
      Dump(FinderItem, 2);
    }

    Ok();
  }
}
