namespace MediaSearch.Models;
public class TMediaSourceVirtual : AMediaSource, IMediaSourceVirtual {

  #region --- Public properties ------------------------------------------------------------------------------
  public string StorageRoot { get; set; } = string.Empty;
  public string StoragePath { get; set; } = string.Empty;
  public string FileName { get; set; } = string.Empty;
  public string FileExtension { get; set; } = string.Empty;

  public string FullFileName => Path.Combine(StorageRoot, StoragePath, $"{FileName}.{FileExtension}");
  public long Size { get; set; } = 0;
  #endregion --- Public properties ---------------------------------------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSourceVirtual() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaSourceVirtual>();
  }

  public TMediaSourceVirtual(IMediaSourceVirtual mediaSource) : base(mediaSource) {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaSourceVirtual>();
    StorageRoot = mediaSource.StorageRoot;
    StoragePath = mediaSource.StoragePath;
    FileName = mediaSource.FileName;
    FileExtension = mediaSource.FileExtension;
    Size = mediaSource.Size;
  }

  public TMediaSourceVirtual(string rootStorage) {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaSourceVirtual>();
    StorageRoot = rootStorage;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public static TMediaSourceVirtual Empty { get { return new TMediaSourceVirtual(); } }

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine(base.ToString(indent));
    RetVal.AppendIndent($"- {nameof(StorageRoot)} : {StorageRoot.WithQuotes()}", indent);
    RetVal.AppendIndent($"- {nameof(StoragePath)} : {StoragePath.WithQuotes()}", indent);
    RetVal.AppendIndent($"- {nameof(FileName)} : {FileName.WithQuotes()}", indent);
    RetVal.AppendIndent($"- {nameof(FileExtension)} : {FileExtension.WithQuotes()}", indent);
    RetVal.AppendIndent($"- {nameof(FullFileName)} : {FullFileName.WithQuotes()}", indent);
    RetVal.AppendIndent($"- {nameof(Size)} : {Size} bytes", indent);
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public IEnumerable<TStreamVideoProperties> GetVideoStreams() {
    return Properties.GetProperties().Select(p => p.Value).OfType<TStreamVideoProperties>();
  }

  public IEnumerable<TStreamAudioProperties> GetAudioStreams() {
    return Properties.GetProperties().Select(p => p.Value).OfType<TStreamAudioProperties>();
  }
  public IEnumerable<TStreamSubTitleProperties> GetSubTitleStreams() {
    return Properties.GetProperties().Select(p => p.Value).OfType<TStreamSubTitleProperties>();
  }

}

