namespace MediaSearch.Models;

public interface IMovie : IMedia, IJson<IMovie> {

  /// <summary>
  /// The year when the movie was distributed on cinema/DTV/...
  /// </summary>
  int OutputYear { get; set; }

  const string DEFAULT_PICTURE_NAME = "folder.jpg";

  IMovieInfoContent MovieInfoContent { get; }



}
