using System.Collections.Concurrent;

namespace MediaSearch.Models;
public interface IMediaMovieParser : ILoggable {

  Task<IMediaMovie?> ParseFile(string fullFileName);
  Task ParseFolderAsync(string fullFolderName);
  Task ParseFolderAsync(string fullFolderName, CancellationToken token);

  event EventHandler<string>? OnParseFileStarting;
  event EventHandler<string>? OnParseFolderStarting;
  event EventHandler<int>? OnParseFolderProgress;
  event EventHandler<string>? OnParseFileCompleted;
  event EventHandler<string>? OnParseFolderCompleted;

  void Init();

  ConcurrentQueue<IMediaMovie> Results { get; }
  bool ParsingComplete { get; }

  int LastParseCount { get; }
  int LastSuccessCount { get; }
  int LastErrorCount { get; }

}
