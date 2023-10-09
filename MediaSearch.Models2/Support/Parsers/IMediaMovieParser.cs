namespace MediaSearch.Models;
public interface IMediaMovieParser : ILoggable {

  Task<IMediaMovie?> ParseFile(string fullFilename);

}
