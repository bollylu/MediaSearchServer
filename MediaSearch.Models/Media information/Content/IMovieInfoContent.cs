namespace MediaSearch.Models;

public interface IMovieInfoContent {

  List<ELanguage> Soundtracks { get; }
  List<ELanguage> Subtitles { get; }

}
