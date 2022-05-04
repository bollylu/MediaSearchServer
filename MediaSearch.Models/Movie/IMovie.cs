namespace MediaSearch.Models;

public interface IMovie : IMedia, IJson<IMovie>, IDisposable, IAsyncDisposable {

  /// <summary>
  /// The normalized extension of the movie (container type) based on file extension
  /// </summary>
  EMovieExtension Extension { get; }

  List<ELanguage> Soundtracks { get; }
  List<ELanguage> Subtitles { get; }


  const string DEFAULT_PICTURE_NAME = "folder.jpg";

  void ReplaceBy(IMovie movie);

}
