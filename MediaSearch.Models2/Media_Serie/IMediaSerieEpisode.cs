namespace MediaSearch.Models;
public interface IMediaSerieEpisode {

  /// <summary>
  /// The numer of the episode, counting from first episode
  /// </summary>
  public int AbsoluteNumber { get; set; }

  /// <summary>
  /// The season where the episode belongs
  /// </summary>
  public int Season { get; set; }

  /// <summary>
  /// The number inside the season
  /// </summary>
  public int Number { get; set; }

  /// <summary>
  /// The type of serie
  /// </summary>
  public ESerieType SerieType { get; set; }

  IMediaInfos MediaInfos { get; set; }
  IMediaSources MediaSources { get; set; }
  IMediaPictures MediaPictures { get; set; }
}
