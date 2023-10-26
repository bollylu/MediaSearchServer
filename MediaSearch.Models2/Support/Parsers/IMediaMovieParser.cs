namespace MediaSearch.Models;
public interface IMediaMovieParser : ILoggable {

  Task<IMediaMovie?> ParseFile(string fullFileName);
  Task<IMediaMovie?> ParseFolder(string fullFolderName);

}
