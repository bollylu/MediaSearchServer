namespace MediaSearch.Models;

/// <summary>
/// A media used to store a serie (TV show, anime, ...)
/// </summary>
public interface IMediaSerie : IMedia {

  /// <summary>
  /// The type of serie
  /// </summary>
  ESerieType SerieType { get; set; }

  /// <summary>
  /// Add a non-existing season to the serie
  /// </summary>
  /// <param name="season">The season to add</param>
  /// <returns><see langword="true"/> if successful, <see langword="false"/> otherwise</returns>
  bool AddSeason(IMediaSerieSeason season);

  /// <summary>
  /// Remove a season from the serie
  /// </summary>
  /// <param name="season">The season to remove</param>
  /// <returns><see langword="true"/> if successful, <see langword="false"/> otherwise</returns>
  bool RemoveSeason(IMediaSerieSeason season);

  /// <summary>
  /// Remove a season from the serie
  /// </summary>
  /// <param name="seasonNumber">The number of the season to remove</param>
  /// <returns><see langword="true"/> if successful, <see langword="false"/> otherwise</returns>
  bool RemoveSeason(int seasonNumber);

  /// <summary>
  /// Get all the seasons from the serie
  /// </summary>
  /// <returns>An enumeration of the seasons</returns>
  IEnumerable<IMediaSerieSeason> GetSeasons();

  /// <summary>
  /// Get on season from the serie
  /// </summary>
  /// <param name="index">The number of the season to retrieve</param>
  /// <returns>The season requested or <see langword="null"/> if any error</returns>
  IMediaSerieSeason? GetSeason(int index);

  /// <summary>
  /// Clear all the seasons from the serie (and the episodes from each seasons)
  /// </summary>
  void Clear();
}
