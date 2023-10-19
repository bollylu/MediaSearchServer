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
    RetVal.AppendLine($"- {nameof(StorageRoot)} : {StorageRoot.WithQuotes()}");
    RetVal.AppendLine($"- {nameof(StoragePath)} : {StoragePath.WithQuotes()}");
    RetVal.AppendLine($"- {nameof(FileName)} : {FileName.WithQuotes()}");
    RetVal.AppendLine($"- {nameof(FileExtension)} : {FileExtension.WithQuotes()}");
    RetVal.AppendLine($"- {nameof(FullFileName)} : {FullFileName.WithQuotes()}");
    RetVal.AppendLine($"- {nameof(Size)} : {Size}");
    RetVal.AppendLine($"- {nameof(MediaStreams)} : {MediaStreams.GetAll().Count()} stream(s)");
    if (MediaStreams.Any()) {
      RetVal.AppendLine($"  - Video : {MediaStreams.MediaStreamsVideo.Count()}");
      RetVal.AppendLine($"  - Audio : {MediaStreams.MediaStreamsAudio.Count()} ({MediaStreams.MediaStreamsAudio.Select(s => s.Language).CombineToString()})");
      RetVal.AppendLine($"  - SubTitle : {MediaStreams.MediaStreamsSubTitle.Count()} ({MediaStreams.MediaStreamsSubTitle.Select(s => s.Language).CombineToString()})");
      RetVal.AppendLine($"  - Data : {MediaStreams.MediaStreamsData.Count()}");
      RetVal.AppendLine($"  - Unknown : {MediaStreams.MediaStreamsUnknown.Count()}");
    }
    RetVal.AppendLine($"- {nameof(Languages)} : {Languages.Count()}");
    if (Languages.Any()) {
      RetVal.AppendLine($"  - Values : {Languages.Select(l => l.ToString()).CombineToString()}");
      RetVal.AppendLine($"  - Principal : {Languages.GetPrincipal()}");
    }
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

}

