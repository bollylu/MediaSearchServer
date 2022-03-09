namespace MediaSearch.Models;

public class TMediaInfoFileMeta : IMediaInfoFile {

  public const string DEFAULT_FILENAME = "media.meta";

  public IMediaSearchLogger<TMediaInfoFileMeta> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMediaInfoFileMeta>();

  public string StoragePath { get; set; } = "";
  public string StorageName { get; set; } = DEFAULT_FILENAME;

  public IMediaInfoContent Content { get; } = new TMediaInfoContentMeta();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaInfoFileMeta() { }
  public TMediaInfoFileMeta(string storagePath, string storageName) {
    StoragePath = storagePath;
    StorageName = storageName;
  } 
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public bool Exists() {
    string FullStorageName = Path.Combine(StoragePath, StorageName);
    return File.Exists(FullStorageName);
  }

  public bool Read() {
    throw new NotImplementedException();
  }

  public bool Write() {
    throw new NotImplementedException();
  }

  public bool Export(string newFilename) {
    throw new NotImplementedException();
  }

}
