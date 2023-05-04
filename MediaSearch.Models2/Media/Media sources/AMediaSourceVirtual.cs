namespace MediaSearch.Models;
public class AMediaSourceVirtual : AMediaSource, IMediaSourceVirtual {

  public string StorageRoot { get; set; } = string.Empty;
  public string StoragePath { get; set; } = string.Empty;
  public string FileName { get; set; } = string.Empty;
  public string FileExtension { get; set; } = string.Empty;
  public long Size { get; set; } = 0;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  protected AMediaSourceVirtual() : base() {
    Logger = GlobalSettings.LoggerPool.GetLogger<AMediaSourceVirtual>();
  }

  protected AMediaSourceVirtual(string rootStorage) : this() {
    StorageRoot = rootStorage;
  }

  protected AMediaSourceVirtual(IMediaSourceVirtual mediaSource) : base(mediaSource) {
    StorageRoot = mediaSource.StorageRoot;
    StoragePath = mediaSource.StoragePath;
    FileName = mediaSource.FileName;
    FileExtension = mediaSource.FileExtension;
    Size = mediaSource.Size;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder(base.ToString(indent));
    RetVal.AppendIndent($"- {nameof(StorageRoot)} : {StorageRoot.WithQuotes()}", indent);
    RetVal.AppendIndent($"- {nameof(FileName)} : {FileName.WithQuotes()}", indent);
    RetVal.AppendIndent($"- {nameof(FileExtension)} : {FileExtension.WithQuotes()}", indent);
    RetVal.AppendIndent($"- {nameof(Size)} : {Size} bytes", indent);
    RetVal.AppendIndent($"- {nameof(DateAdded)} : {DateAdded}", indent);
    RetVal.AppendIndent($"- {nameof(CreationDate)} : {CreationDate}", indent);
    RetVal.AppendIndent($"- {nameof(CreationYear)} : {CreationYear}", indent);
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------
}
