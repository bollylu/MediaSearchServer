namespace MediaSearch.Models;
public class TMediaSerieSeason : IMediaSerieSeason {

  public ESerieType SerieType { get; set; } = ESerieType.Unknown;
  public int Number { get; set; }

  private readonly List<TMediaSerieEpisode> Items = new();

  public TMediaInfos Infos { get; set; } = new TMediaInfos();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSerieSeason() { }
  public TMediaSerieSeason(ESerieType type) {
    SerieType = type;
  }
  public TMediaSerieSeason(TMediaSerieSeason season) {
    SerieType = season.SerieType;
    Items.AddRange(season.Items);
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendIndent($"- {nameof(SerieType)} = {SerieType}", indent);
    RetVal.AppendIndent($"- {nameof(Number)} = {Number}", indent);

    if (Infos.Any()) {
      RetVal.AppendIndent($"- {nameof(Infos)}", indent);
      foreach (var MediaInfoItem in Infos) {
        RetVal.AppendIndent($"- {MediaInfoItem.Value}", indent + 2);
      }
    } else {
      RetVal.AppendIndent($"- {nameof(Infos)} is empty", indent);
    }

    if (Items.Any()) {
      RetVal.AppendIndent($"- {nameof(Items)} ({Items.Count})", indent);
      foreach (TMediaSerieEpisode EpisodeItem in Items) {
        RetVal.AppendIndent($"### {EpisodeItem.Season}x{EpisodeItem.Number:00} {EpisodeItem.Name} ###", indent + 2);
        RetVal.AppendIndent($"{EpisodeItem.ToString(indent)}", indent + 2);
      }
    } else {
      RetVal.AppendIndent($"- No episodes available", indent);
    }
    return RetVal.ToString();
  }


  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public bool AddEpisode(TMediaSerieEpisode episode) {
    Items.Add(episode);
    return true;
  }

  public bool RemoveEpisode(TMediaSerieEpisode episode) {
    Items.Remove(episode);
    return true;
  }

  public IEnumerable<TMediaSerieEpisode> GetEpisodes() {
    foreach (TMediaSerieEpisode EpisodeItem in Items) {
      yield return EpisodeItem;
    }
  }

  public void Clear() {
    Items.Clear();
  }
}
