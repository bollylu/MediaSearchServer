namespace MediaSearch.Models;

public class TMediaMovie : AMedia {

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaMovie() : base() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaMovie>();
  }
  public TMediaMovie(ILogger logger) : base(logger) { }
  public TMediaMovie(TMediaMovie movie) : base(movie) {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaMovie>();
  } 
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

}
