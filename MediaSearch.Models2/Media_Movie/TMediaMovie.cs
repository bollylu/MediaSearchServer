namespace MediaSearch.Models;

public class TMediaMovie : AMedia, IMediaMovie {

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaMovie() : base() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaMovie>();
    MediaType = EMediaType.Movie;
  }
  public TMediaMovie(ILogger logger) : base(logger) {
    MediaType = EMediaType.Movie;
  }
  public TMediaMovie(IMediaMovie movie) : base(movie) {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaMovie>();
    MediaType = EMediaType.Movie;
  }

  public TMediaMovie(string title) : base(title) {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaMovie>();
    MediaType = EMediaType.Movie;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

}
