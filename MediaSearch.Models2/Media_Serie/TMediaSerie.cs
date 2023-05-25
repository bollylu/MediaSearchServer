namespace MediaSearch.Models;

public class TMediaSerie : AMedia {

  /// <summary>
  /// The type of serie
  /// </summary>
  public ESerieType SerieType { get; set; }

  public List<IMediaSerieSeason> Seasons { get; set; } = new List<IMediaSerieSeason>();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSerie() : base() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaSerie>();
  }
  public TMediaSerie(ILogger logger) : base(logger) { }
  public TMediaSerie(TMediaSerie movie) : base(movie) {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaSerie>();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder(base.ToString(indent));
    RetVal.AppendIndent($"- {nameof(SerieType)}={SerieType}", indent);
    RetVal.AppendIndent($"- {nameof(Seasons)}", indent);
    foreach (IMediaSerieSeason SeasonItem in Seasons) {
      RetVal.AppendIndent(SeasonItem.ToString(indent), indent + 2);
    }
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------
}
