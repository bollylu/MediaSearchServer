namespace MediaSearch.Server.Services;

  /// <summary>
  /// Fake movie cache with hardcoded values
  /// </summary>
  public class XMovieCache : AMovieCache {

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public XMovieCache() {
      Name = "Demo";
      Storage = @"\\andromeda.sharenet.priv\Films";
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public override IEnumerable<IFileInfo> FetchFiles(CancellationToken token) {
      IList<IFileInfo> RetVal = new List<IFileInfo> {
        new XFileInfo(@"\\andromeda.sharenet.priv\Films\Science-fiction\[Aliens, créatures, ...]\Alien agent (2007)\Alien agent (2007).avi") { Length = 123456789 },
        new XFileInfo(@"\\andromeda.sharenet.priv\Films\Science-fiction\[Aliens, créatures, ...]\Godzilla #\Godzilla (1954)\Godzilla (1954).mkv") { Length = 987654321 }
      };
      return RetVal;
    }

  }
