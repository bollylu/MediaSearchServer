using System.Runtime.InteropServices;
using System.Transactions;

using Microsoft.Extensions.Primitives;

namespace MediaSearch.Models2.Test;

[TestClass]
public class MediaMovieParserTests {
  [TestMethod]
  public void Instanciate_MediaMovieParser() {
    Message("Instanciate media movie parser");
    TMediaMovieParser Target = new TMediaMovieParser("");
    Assert.IsNotNull(Target);
    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void Instanciate_MediaMovieParser_WithRootPath() {
    Message("Instanciate media movie parser with root path");
    TMediaMovieParser Target = new TMediaMovieParser(@"\\server\share\folder\");
    Assert.IsNotNull(Target);
    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void Parse_Linux() {
    const string ROOT = "/mnt/films";

    Message("Instanciate media movie parser");
    TMediaMovieParser Parser = new TMediaMovieParser(ROOT, false);
    Assert.IsNotNull(Parser);
    Dump(Parser);

    const string SOURCE = "/mnt/films/new movie (2022)/new movie (2022).mkv";
    Message($"Parse {SOURCE.WithQuotes()}");

    IMediaMovie? Target = Parser.Parse(new XFileInfo(OSPlatform.Linux, SOURCE, ROOT) { Length = 1234567 });
    Assert.IsNotNull(Target);

    Dump(Target);

    Assert.AreEqual("new movie", Target.Name);
    Assert.IsNotNull(Target.MediaInfos);
    Assert.AreEqual(2022, Target.MediaInfos.Get(ELanguage.French)?.CreationYear ?? 0);

    Ok();
  }

  [TestMethod]
  public void Parse_Windows() {
    Message("Instanciate media movie parser");
    TMediaMovieParser Parser = new TMediaMovieParser(@".\data\films\");
    Assert.IsNotNull(Parser);
    Dump(Parser);

    const string SOURCE = @".\data\films\La bamba (1987)\La bamba (1987).mkv";
    Message($"Parse {SOURCE.WithQuotes()}");

    IMediaMovie? Target = Parser.Parse(new TFileInfo(SOURCE));
    Assert.IsNotNull(Target);

    Dump(Target);

    Assert.AreEqual("La bamba", Target.Name);
    Assert.IsNotNull(Target.MediaInfos);
    Assert.AreEqual(1987, Target.MediaInfos.Get(ELanguage.French)?.CreationYear ?? 0);

    Ok();
  }
}
