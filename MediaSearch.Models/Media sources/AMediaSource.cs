using BLTools.Json;

namespace MediaSearch.Models;

public abstract class AMediaSource : ALoggable, IMediaSource {

  public EMediaType MediaSourceType { get; init; } = EMediaType.Unknown;

  public string StorageRoot { get; set; } = string.Empty;
  public string StoragePath { get; set; } = string.Empty;
  public string FileName { get; set; } = string.Empty;
  public string FileExtension { get; set; } = string.Empty;
  public long Size { get; set; } = 0;

  [JsonConverter(typeof(TDateOnlyJsonConverter))]
  public DateOnly DateAdded { get; set; } = DateOnly.FromDateTime(DateTime.Today);

  public DateOnly CreationDate { get; set; } = new DateOnly();
  public int CreationYear => CreationDate.Year;

  public string Description { get; set; } = string.Empty;
  public ELanguage Language { get; set; } = ELanguage.Unknown;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  protected AMediaSource() {
    Logger = GlobalSettings.LoggerPool.GetLogger<AMediaSource>();
  }

  protected AMediaSource(string rootStorage) : this() {
    StorageRoot = rootStorage;
  }

  protected AMediaSource(IMediaSource source) : this() {
    StorageRoot = source.StorageRoot;
    StoragePath = source.StoragePath;
    FileName = source.FileName;
    FileExtension = source.FileExtension;
    DateAdded = source.DateAdded;
    CreationDate = source.CreationDate;
    Size = source.Size;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public virtual string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendIndent($"- {nameof(MediaSourceType)} : {MediaSourceType}", indent);
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
