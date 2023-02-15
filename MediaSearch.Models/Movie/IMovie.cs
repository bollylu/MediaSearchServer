namespace MediaSearch.Models;

public interface IMovie : IMedia, IJson<IMovie> {

  /// <summary>
  /// The normalized extension of the movie (container type) based on file extension
  /// </summary>
  EMovieExtension Extension { get; }

  /// <summary>
  /// The group where the movie belongs
  /// </summary>
  string Group { get; set; }

  bool IsGroupMember { get; }

  /// <summary>
  /// The sub group where the movie belongs
  /// </summary>
  string SubGroup { get; set; }

  /// <summary>
  /// The movie size
  /// </summary>
  long Size { get; set; }

  /// <summary>
  /// The year when the movie was distributed on cinema/DTV/...
  /// </summary>
  int OutputYear { get; set; }

  const string DEFAULT_PICTURE_NAME = "folder.jpg";

  IMovieInfoContent MovieInfoContent { get; }



}
