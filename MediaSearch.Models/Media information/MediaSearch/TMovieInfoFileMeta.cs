namespace MediaSearch.Models;

public class TMovieInfoFileMeta : IMediaInfoFile {

  public const string DEFAULT_FILENAME = "media.meta";
  public const int TIMEOUT_IN_MS = 5000;

  public IMediaSearchLogger<TMovieInfoFileMeta> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMovieInfoFileMeta>();

  public string StoragePath { get; set; } = "";
  public string StorageName { get; set; } = DEFAULT_FILENAME;

  public string FullStorageName => Path.Join(StoragePath.NormalizePath(), StorageName);

  public string RawContent { get; private set; } = "";

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMovieInfoFileMeta() { }
  public TMovieInfoFileMeta(string storagePath, string storageName = DEFAULT_FILENAME) {
    StoragePath = storagePath;
    StorageName = storageName;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public Task<bool> Exists() {
    return Task.FromResult(File.Exists(FullStorageName));
  }

  public async Task<bool> Read() {
    try {
      using (CancellationTokenSource cts = new CancellationTokenSource(TIMEOUT_IN_MS)) {
        RawContent = await File.ReadAllTextAsync(FullStorageName, cts.Token);
      }
      Content = IJson<TMovieInfoContentMeta>.FromJson(RawContent);
      Logger.IfDebugMessageEx($"Read content of {FullStorageName}", RawContent);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox("Unable to read MediaFile", ex);
      return false;
    }
  }

  public async Task<bool> Write() {
    try {
      Logger.IfDebugMessageEx($"Writing content to {FullStorageName}", RawContent);
      //RawContent = IJson<TMovieInfoContentMeta>. Content
      using (CancellationTokenSource cts = new CancellationTokenSource(TIMEOUT_IN_MS)) {
        await File.WriteAllTextAsync(FullStorageName, RawContent, cts.Token);
      }
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox("Unable to write MediaFile", ex);
      return false;
    }
  }

  public async Task<bool> Export(string newFilename) {
    try {
      Logger.IfDebugMessageEx($"Writing content to {newFilename}", RawContent);
      using (CancellationTokenSource cts = new CancellationTokenSource(TIMEOUT_IN_MS)) {
        await File.WriteAllTextAsync(newFilename, RawContent, cts.Token);
      }
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox("Unable to export MediaFile", ex);
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
}
