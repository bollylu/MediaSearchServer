namespace MediaSearch.Models;

/// <summary>
/// Whatever makes a grouping in season
/// </summary>
public interface IMediaSerieSeason : IMedia {

  /// <summary>
  /// The type of serie
  /// </summary>
  ESerieType SerieType { get; set; }

  /// <summary>
  /// The number of the season
  /// </summary>
  int Number { get; set; }

  /// <summary>
  /// Add a non-existing episode to the season
  /// </summary>
  /// <param name="episode">The episode to add, with valid and unique number</param>
  /// <returns><see langword="true"/> if successful, <see langword="false"/> otherwise</returns>
  bool AddEpisode(TMediaSerieEpisode episode);

  /// <summary>
  /// Remove an episode from the season
  /// </summary>
  /// <param name="episode">The episode to remove</param>
  /// <returns><see langword="true"/> if successful, <see langword="false"/> otherwise</returns>
  bool RemoveEpisode(TMediaSerieEpisode episode);

  /// <summary>
  /// Remove an episode from the season
  /// </summary>
  /// <param name="episodeNumber">The number of the episode to remove</param>
  /// <returns><see langword="true"/> if successful, <see langword="false"/> otherwise</returns>
  bool RemoveEpisode(int episodeNumber);

  /// <summary>
  /// Get all the episodes from the season
  /// </summary>
  /// <returns>An enumeration of the episodes</returns>
  IEnumerable<TMediaSerieEpisode> GetEpisodes();

  /// <summary>
  /// Get one episode from the season
  /// </summary>
  /// <param name="episodeNumber">The number of the episode to retrieve</param>
  /// <returns>The episode requested or <see langword="null"/> if any error</returns>
  IMediaSerieEpisode? GetEpisode(int episodeNumber);

  /// <summary>
  /// Clear all the episodes from the season
  /// </summary>
  void Clear();
}
