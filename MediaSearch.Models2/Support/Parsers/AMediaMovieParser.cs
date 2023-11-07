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

  public abstract Task<IMediaMovie?> ParseFile(string source);

  public abstract Task ParseFolderAsync(string source);

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