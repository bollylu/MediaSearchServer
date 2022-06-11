namespace MediaSearch.Models;

public class TMovieInfoFileMeta : IMediaInfoFile, ILoggable {

  public const string DEFAULT_FILENAME = "media.msmeta";
  public const int TIMEOUT_IN_MS = 5000;

  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TMovieInfoFileMeta>();

  #region --- Storage info --------------------------------------------
  public string StoragePath { get; set; } = "";
  public string StorageName { get; set; } = DEFAULT_FILENAME;
  public string FullStorageName => Path.Join(StoragePath.NormalizePath(), StorageName);
  #endregion --- Storage info --------------------------------------------

  //#region --- Header --------------------------------------------
  //public IMediaInfoHeader Header {
  //  get {
  //    return _Header ??= new TMediaInfoHeader();
  //  }
  //  set {
  //    _Header = value;
  //  }
  //}
  //private IMediaInfoHeader? _Header; 
  //#endregion --- Header --------------------------------------------

  #region --- Content --------------------------------------------
  public IMedia Content => MetaContent;

  public TMovie MetaContent { get; set; } = new TMovie();

  public string RawContent { get; private set; } = "";
  #endregion --- Content --------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMovieInfoFileMeta() { }
  public TMovieInfoFileMeta(string storagePath, string storageName = DEFAULT_FILENAME) {
    StoragePath = storagePath;
    StorageName = storageName;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    //RetVal.AppendIndent($"- {nameof(Header)}", indent)
    //      .AppendIndent(Header, indent+2)
    RetVal.AppendIndent($"- {nameof(StoragePath)} = {StoragePath.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(StorageName)} = {StorageName.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(FullStorageName)} = {FullStorageName.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(Content)} ({Content.GetType().Name})", indent)
          .AppendIndent(Content, indent + 2);
    return RetVal.ToString();
  }


  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  #region --- IMediaFileInfo --------------------------------------------
  public bool Exists() {
    if (string.IsNullOrWhiteSpace(FullStorageName)) {
      return false;
    }
    return File.Exists(FullStorageName);
  }

  public bool Remove() {
    if (string.IsNullOrWhiteSpace(FullStorageName)) {
      return false;
    }
    try {
      File.Delete(FullStorageName);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to remove {FullStorageName}", ex);
      return false;
    }
  }

  public bool Read() {
    try {
      RawContent = File.ReadAllText(FullStorageName);
      TMovieInfoFileMeta? Converted = IJson.FromJson<TMovieInfoFileMeta>(RawContent);
      if (Converted is null) {
        Logger.LogErrorBox("Unable to deserialize Json", RawContent);
        return false;
      }
      //Header = Converted.Header;
      MetaContent = Converted.MetaContent;
      Logger.IfDebugMessageExBox($"Read content of {FullStorageName}", Content);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox("Unable to read MediaFile", ex);
      return false;
    }
  }

  public async Task<bool> ReadAsync(CancellationToken token) {
    try {
      RawContent = await File.ReadAllTextAsync(FullStorageName, token).ConfigureAwait(false);
      TMovieInfoFileMeta? Converted = IJson.FromJson<TMovieInfoFileMeta>(RawContent);
      if (Converted is null) {
        Logger.LogErrorBox("Unable to deserialize Json", RawContent);
        return false;
      }
      //Header = Converted.Header;
      MetaContent = Converted.MetaContent;
      Logger.IfDebugMessageExBox($"Read content of {FullStorageName}", Content);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox("Unable to read MediaFile", ex);
      return false;
    }
  }

  public bool Write() {
    try {
      RawContent = IJson.ToJson(this);
      Logger.IfDebugMessageExBox($"Writing content to {FullStorageName}", RawContent);
      File.WriteAllText(FullStorageName, RawContent);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox("Unable to write MediaFile", ex);
      return false;
    }
  }

  public async Task<bool> WriteAsync(CancellationToken token) {
    try {
      RawContent = IJson.ToJson(this);
      Logger.IfDebugMessageExBox($"Writing content to {FullStorageName}", RawContent);
      await File.WriteAllTextAsync(FullStorageName, RawContent, token).ConfigureAwait(false);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox("Unable to write MediaFile", ex);
      return false;
    }
  }

  public bool Export(string newFilename) {
    try {
      RawContent = IJson.ToJson(this);
      Logger.IfDebugMessageExBox($"Writing content to {newFilename}", RawContent);
      File.WriteAllText(newFilename, RawContent);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox("Unable to export MediaFile", ex);
      return false;
    }
  }

  public async Task<bool> ExportAsync(string newFilename, CancellationToken token) {
    try {
      RawContent = IJson.ToJson(this);
      Logger.IfDebugMessageExBox($"Writing content to {newFilename}", RawContent);
      await File.WriteAllTextAsync(newFilename, RawContent, token).ConfigureAwait(false);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox("Unable to export MediaFile", ex);
      return false;
    }
  }
  #endregion --- IMediaFileInfo --------------------------------------------

}
