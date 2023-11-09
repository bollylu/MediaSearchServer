namespace MediaSearch.Server.Services;

/// <summary>
/// Fake movie cache with hardcoded values
/// </summary>
public class XMovieCache : AMediaCache {

  public string DataSource { get; init; } = "";

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public XMovieCache() {
    Name = "Demo";
    RootStoragePath = @"\\andromeda.sharenet.priv\Films";
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public static XMovieCache Instance(string dataSource) {
    XMovieCache RetVal = new XMovieCache() { DataSource = dataSource };
    return RetVal;
  }
}
