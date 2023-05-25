namespace MediaSearch.Models;
public class TMediaSerieSeason : IMediaSerieSeason {

  public ESerieType SerieType { get; set; } = ESerieType.Unknown;
  public int Number { get; set; }

  public List<TMediaSerieEpisode> Episodes { get; set; } = new();

  public TMediaInfos Infos { get; set; } = new TMediaInfos();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSerieSeason() { }
  public TMediaSerieSeason(ESerieType type) {
    SerieType = type;
  }
  public TMediaSerieSeason(TMediaSerieSeason season) {
    SerieType = season.SerieType;
    Episodes.AddRange(season.Episodes);
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendIndent($"- {nameof(SerieType)} = {SerieType}", indent);
    RetVal.AppendIndent($"- {nameof(Number)} = {Number}", indent);
    RetVal.AppendIndent($"- {nameof(Infos)}", indent);
    RetVal.AppendIndent($"- {Infos.ToString(indent)}", indent + 2);
    RetVal.AppendIndent($"- {nameof(Episodes)}", indent);
    foreach (TMediaSerieEpisode EpisodeItem in Episodes) {
      RetVal.AppendIndent($"# {EpisodeItem.ToString(indent)}", indent + 2);
    }
    return RetVal.ToString();
  }


  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------
}
