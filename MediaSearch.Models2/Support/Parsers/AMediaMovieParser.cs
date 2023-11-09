using System.Collections.Concurrent;

using MediaSearch.Models.Support;

namespace MediaSearch.Models;
public abstract class AMediaMovieParser : ALoggable, IMediaMovieParser {

  protected string _RootPath = "";

  static protected string[] AllowedExtensions = new string[] { "mkv", "mp4", "avi", "iso" };

  public ConcurrentQueue<IMediaMovie> Results { get; private set; } = new ConcurrentQueue<IMediaMovie>();
  public bool ParsingComplete { get; protected set; }
  public int LastParseCount { get; protected set; }
  public int LastSuccessCount { get; protected set; }
  public int LastErrorCount { get; protected set; }

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  protected AMediaMovieParser() {
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{nameof(_RootPath)} = {_RootPath.WithQuotes()}");
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public void Init() {
    LastErrorCount = 0;
    LastParseCount = 0;
    LastSuccessCount = 0;
    ParsingComplete = false;
    Results.Clear();
  }

  public async Task<IMediaMovie?> ParseFile(string source) {
    return await ParseFile(source, CancellationToken.None);
  }
  public abstract Task<IMediaMovie?> ParseFile(string source, CancellationToken token);

  public async Task ParseFolderAsync(string source) {
    await ParseFolderAsync(source, CancellationToken.None);
  }
  public virtual async Task ParseFolderAsync(string source, CancellationToken token) {
    #region === Validate parameters ===
    if (source.IsEmpty()) {
      LogError("Unable to parse : source is missing");
      return;
    }

    LogDebugBox(nameof(source), source.ToString());

    if (!Directory.Exists(source)) {
      LogError($"Unable to parse : {source.WithQuotes()} is missing or access is denied");
      return;
    }
    #endregion === Validate parameters ===

    NotifyParseFolderStarting(source);

    IEnumerable<string> Filenames = Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories)
                                             .Where(f => f.AfterLast('.').ToLowerInvariant().IsIn(AllowedExtensions));
    int Counter = 0;
    await Parallel.ForEachAsync(Filenames, token, async (f, s) => {
      IMediaMovie? NewValue = await ParseFile(f, s);
      NotifyParseFolderProgress(Counter++);
      if (NewValue is not null) {
        Results.Enqueue(NewValue);
      }
    });

    NotifyParseFolderCompleted(source);
    ParsingComplete = true;
  }

  #region --- Events --------------------------------------------
  public event EventHandler<string>? OnParseFileStarting;
  public event EventHandler<string>? OnParseFolderStarting;
  public event EventHandler<int>? OnParseFolderProgress;
  public event EventHandler<string>? OnParseFileCompleted;
  public event EventHandler<string>? OnParseFolderCompleted;

  protected void NotifyParseFileStarting(string filename) {
    OnParseFileStarting?.Invoke(this, filename);
  }

  protected void NotifyParseFileCompleted(string filename) {
    OnParseFileCompleted?.Invoke(this, filename);
  }

  protected void NotifyParseFolderStarting(string foldername) {
    OnParseFolderStarting?.Invoke(this, foldername);
  }

  protected void NotifyParseFolderProgress(int fileCount) {
    OnParseFolderProgress?.Invoke(this, fileCount);
  }
  protected void NotifyParseFolderCompleted(string foldername) {
    OnParseFolderCompleted?.Invoke(this, foldername);
  }
  #endregion --- Events -----------------------------------------
}