namespace MediaSearch.Models;

public interface IMovie : IMedia, IJson<IMovie> {

  /// <summary>
  /// The normalized extension of the movie (container type) based on file extension
  /// </summary>
  EMovieExtension Extension { get; }

  

  /// <summary>
  /// The sub group where the movie belongs
  /// </summary>
  string SubGroup { get; set; }

  

  const string DEFAULT_PICTURE_NAME = "folder.jpg";

  IMovieInfoContent MovieInfoContent { get; }

  void Duplicate(IMovie movie);

}
