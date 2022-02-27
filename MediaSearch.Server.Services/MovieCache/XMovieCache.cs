namespace MediaSearch.Server.Services;

/// <summary>
/// Fake movie cache with hardcoded values
/// </summary>
public class XMovieCache : AMovieCache {

  public string DataSource { get; init; } = "";

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public XMovieCache() {
    Name = "Demo";
    RootStoragePath = @"\\andromeda.sharenet.priv\Films";
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  //protected override IEnumerable<IFileInfo> _FetchFiles(CancellationToken token) {
  //  IList<IFileInfo> RetVal = new List<IFileInfo> {
  //      new XFileInfo(@"\\andromeda.sharenet.priv\Films\Science-fiction\[Aliens, créatures, ...]\Alien agent (2007)\Alien agent (2007).avi") { Length = 123456789, },
  //      new XFileInfo(@"\\andromeda.sharenet.priv\Films\Science-fiction\[Space, spaceships, ...]\2001 #\2010, l'année du premier contact (1984)\2010, l'année du premier contact (1984).mkv") { Length = 12456789 },
  //      new XFileInfo(@"\\andromeda.sharenet.priv\Films\Comédie\[Amitié]\#Pire soirée (2017)\#Pire soirée (2017).mkv") { Length = 12456789 },
  //      new XFileInfo(@"\\andromeda.sharenet.priv\Films\Science-fiction\[Aliens, créatures, ...]\Godzilla #\Godzilla (1954)\Godzilla (1954).mkv") { Length = 987654321 }
  //    };
  //  return RetVal;
  //}

  public override Task Parse(CancellationToken token) {
    if (!File.Exists(DataSource)) {
      return Task.FromException(new Exception($"Missing or invalid datasource {DataSource}"));
    }

    string DataSourceContent = File.ReadAllText(DataSource);
    JsonDocument JsonContent = JsonDocument.Parse(DataSourceContent);
    JsonElement JsonMovies = JsonContent.RootElement;
    foreach (JsonElement JsonMovieItem in JsonMovies.GetProperty("movies").EnumerateArray()) {
      IMovie? Movie = IJson<TMovie>.FromJson(JsonMovieItem.GetRawText());
      if (Movie is not null) {
        AddMovie(Movie);
      }
    }

    return Task.CompletedTask;
  }

  public static XMovieCache Instance(string dataSource) {
    XMovieCache RetVal = new XMovieCache() { DataSource = dataSource };
    Task.Run(() => RetVal.Parse(CancellationToken.None)).Wait();
    return RetVal;
  }
}
