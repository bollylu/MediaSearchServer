namespace MediaSearch.Models;

public interface IMovieInfoContent : IMediaInfoContent, IJson<IMovieInfoContent> {

  List<ELanguage> Soundtracks { get; }
  List<ELanguage> Subtitles { get; }

}
