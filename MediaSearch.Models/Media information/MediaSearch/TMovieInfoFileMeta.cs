namespace MediaSearch.Models;

public class TMovieInfoFileMeta : ALoggable, IMediaInfoFile {

  public const string DEFAULT_FILENAME = "media.meta";
  public const int TIMEOUT_IN_MS = 5000;

  public string StoragePath { get; set; } = "";
  public string StorageName { get; set; } = DEFAULT_FILENAME;

  public string Name => FullStorageName;
  public string FullStorageName => Path.Join(StoragePath.NormalizePath(), StorageName);

  public string RawContent { get; private set; } = "";

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMovieInfoFileMeta() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMovieInfoFileMeta>();
  }
  public TMovieInfoFileMeta(string storagePath, string storageName = DEFAULT_FILENAME) : this() {
    StoragePath = storagePath;
    StorageName = storageName;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public ValueTask<bool> ExistsAsync(CancellationToken token) {
    return ValueTask.FromResult(File.Exists(FullStorageName));
  }

  public async ValueTask<bool> ReadAsync(CancellationToken token) {
    try {
      RawContent = await File.ReadAllTextAsync(FullStorageName, token);
      Content = IJson<TMovieInfoContentMeta>.FromJson(RawContent);
      Logger.IfDebugMessageEx($"Read content of {FullStorageName}", RawContent);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox("Unable to read MediaFile", ex);
      return false;
    }
  }

  public async ValueTask<bool> WriteAsync(CancellationToken token) {
    try {
      Logger.IfDebugMessageEx($"Writing content to {FullStorageName}", RawContent);
      //RawContent = IJson<TMovieInfoContentMeta>. Content
      await File.WriteAllTextAsync(FullStorageName, RawContent, token);
      return true;
    } catch (Exception ex) {
      LogErrorBox("Unable to write MediaFile", ex);
      return false;
    }
  }

  public async ValueTask<bool> ExportAsync(string newFilename, CancellationToken token) {
    try {
      Logger.IfDebugMessageEx($"Writing content to {newFilename}", RawContent);
      await File.WriteAllTextAsync(newFilename, RawContent, token);
      return true;
    } catch (Exception ex) {
      LogErrorBox("Unable to export MediaFile", ex);
      return false;
    }
  }

  public TMovieInfoContentMeta? Content { get; set; }

  public ILanguageDictionary<string> GetTitles() {
    return Content?.Titles ?? TLanguageDictionary<string>.Empty;
  }

  public ILanguageDictionary<string> GetDescription() {
    return Content?.Descriptions ?? TLanguageDictionary<string>.Empty;
  }

  IMediaInfoContent? IMediaInfoFile.Content { get; set; }

  public bool Exists() {
    return File.Exists(FullStorageName);
  }

  public bool Read() {
    try {
      RawContent = File.ReadAllText(FullStorageName);
      Content = IJson<TMovieInfoContentMeta>.FromJson(RawContent);
      Logger.IfDebugMessageEx($"Read content of {FullStorageName}", RawContent);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox("Unable to read MediaFile", ex);
      return false;
    }
  }

  public bool Write() {
    try {
      Logger.IfDebugMessageEx($"Writing content to {FullStorageName}", RawContent);
      //RawContent = IJson<TMovieInfoContentMeta>. Content
      File.WriteAllText(FullStorageName, RawContent);
      return true;
    } catch (Exception ex) {
      LogErrorBox("Unable to write MediaFile", ex);
      return false;
    }
  }

}
