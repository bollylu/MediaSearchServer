namespace MediaSearch.Models;

/// <summary>
/// Whatever makes a grouping in season
/// </summary>
public interface IMediaSerieSeason : IToStringIndent {

  /// <summary>
  /// The type of serie
  /// </summary>
  public ESerieType SerieType { get; set; }

  /// <summary>
  /// The number of the season
  /// </summary>
  public int Number { get; set; }

  ///// <summary>
  ///// List of the episodes for this season
  ///// </summary>
  //List<TMediaSerieEpisode> Episodes { get; set; }

  TMediaInfos Infos { get; set; }

  bool AddEpisode(TMediaSerieEpisode episode);
  bool RemoveEpisode(TMediaSerieEpisode episode);
  IEnumerable<TMediaSerieEpisode> GetEpisodes();

  void Clear();
}
