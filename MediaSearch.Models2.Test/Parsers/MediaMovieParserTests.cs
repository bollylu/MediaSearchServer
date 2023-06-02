using System.Transactions;

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
    TMediaMovieParser Target = new TMediaMovieParser(@"\\server\share\folder");
    Assert.IsNotNull(Target);
    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void Parse_FileOnly() {
    Message("Instanciate media movie parser");
    TMediaMovieParser Parser = new TMediaMovieParser("");
    Assert.IsNotNull(Parser);
    Dump(Parser);

    const string SOURCE = "/mnt/films/new movie (2022).mkv";
    Message($"Parse {SOURCE.WithQuotes()}");

    IMediaMovie? Target = Parser.Parse(new XFileInfo(SOURCE) { Length = 1234567 });
    Assert.IsNotNull(Target);

    Dump(Target);

    Ok();
  }
}
