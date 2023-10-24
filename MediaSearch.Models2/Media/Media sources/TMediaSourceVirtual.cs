using BLTools.Diagnostic;

namespace MediaSearch.Models;
public class TMediaSourceVirtual : AMediaSource, IMediaSourceVirtual {

  #region --- Public properties ------------------------------------------------------------------------------
  public string StorageRoot { get; set; } = string.Empty;
  public string StoragePath { get; set; } = string.Empty;
  public string FileName { get; set; } = string.Empty;
  public string FileExtension { get; set; } = string.Empty;

  public string FullFileName => Path.Combine(StorageRoot, StoragePath, $"{FileName}.{FileExtension}");
  public long Size { get; set; } = 0;

  public IMediaStreams MediaStreams { get; } = new TMediaStreams();

  #endregion --- Public properties ---------------------------------------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSourceVirtual() : base() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaSourceVirtual>();
  }

  public TMediaSourceVirtual(IMediaSourceVirtual mediaSource) : base(mediaSource) {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaSourceVirtual>();
    StorageRoot = mediaSource.StorageRoot;
    StoragePath = mediaSource.StoragePath;
    FileName = mediaSource.FileName;
    FileExtension = mediaSource.FileExtension;
    Size = mediaSource.Size;
    MediaStreams.AddRange(mediaSource.MediaStreams.GetAll());
  }

  public TMediaSourceVirtual(string rootStorage) : this() {
    StorageRoot = rootStorage;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public static TMediaSourceVirtual Empty => new TMediaSourceVirtual();

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

  public override string Dump() {
    StringBuilder RetVal = new StringBuilder(base.Dump());
    RetVal.AppendIndent($"- {nameof(StorageRoot)} : {StorageRoot.WithQuotes()}", 2);
    RetVal.AppendIndent($"- {nameof(StoragePath)} : {StoragePath.WithQuotes()}", 2);
    RetVal.AppendIndent($"- {nameof(FileName)} : {FileName.WithQuotes()}", 2);
    RetVal.AppendIndent($"- {nameof(FileExtension)} : {FileExtension.WithQuotes()}", 2);
    RetVal.AppendIndent($"- {nameof(FullFileName)} : {FullFileName.WithQuotes()}", 2);
    RetVal.AppendIndent($"- {nameof(Size)} : {Size}", 2);
    RetVal.AppendIndent($"- {nameof(MediaStreams)} : {MediaStreams.GetAll().Count()} stream(s)", 2);
    if (MediaStreams.Any()) {
      RetVal.AppendIndent($"- Video : {MediaStreams.MediaStreamsVideo.Count()}", 4);
      RetVal.AppendIndent($"- Audio : {MediaStreams.MediaStreamsAudio.Count()} ({MediaStreams.MediaStreamsAudio.Select(s => s.Language).CombineToString()})", 4);
      RetVal.AppendIndent($"- SubTitle : {MediaStreams.MediaStreamsSubTitle.Count()} ({MediaStreams.MediaStreamsSubTitle.Select(s => s.Language).CombineToString()})", 4);
      RetVal.AppendIndent($"- Data : {MediaStreams.MediaStreamsData.Count()}", 4);
      RetVal.AppendIndent($"- Unknown : {MediaStreams.MediaStreamsUnknown.Count()}", 4);
    }
    RetVal.AppendIndent($"- {nameof(Languages)} : {Languages.Count}", 2);
    if (Languages.Any()) {
      RetVal.AppendIndent($"- Values : {Languages.Select(l => l.ToString()).CombineToString()}", 4);
      RetVal.AppendIndent($"- Principal : {Languages.GetPrincipal()}", 4);
    }
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

}

