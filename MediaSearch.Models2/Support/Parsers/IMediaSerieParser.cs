namespace MediaSearch.Models;
public interface IMediaSerieParser : ILoggable {
  Task<IMediaSerie?> ParseSerieFolder(string folder);
  Task<IMediaSerie?> ParseSeasonFolder(IMediaSerie serie, string folder);
  Task<IMediaSerie?> ParseEpisodeFile(IMediaSerieSeason season, string folder);
}
