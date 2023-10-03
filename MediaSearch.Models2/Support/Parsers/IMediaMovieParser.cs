namespace MediaSearch.Models;
public interface IMediaMovieParser {

  Task<IMediaMovie?> ParseFile(string fullFilename);

}
