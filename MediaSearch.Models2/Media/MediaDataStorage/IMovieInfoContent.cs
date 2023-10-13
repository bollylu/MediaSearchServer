namespace MediaSearch.Models;

public interface IMovieInfoContent : IMediaInfoContent {

  List<ELanguage> Soundtracks { get; }
  List<ELanguage> Subtitles { get; }

}
