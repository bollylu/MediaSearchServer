namespace MediaSearch.Models.Support;
public interface IMediaSourceStreamsFinder {

  string Filename { get; }

  Task Init();
  Task<string> GetVersion();

  TMediaStreams MediaSourceStreams { get; }

}
