namespace MediaSearch.Models;
public abstract class AMediaStream : IMediaStream {

  public const string PROPERTY_INDEX = nameof(Index);
  public const string PROPERTY_CODEC_TYPE = nameof(CodecType);

  public string Name => $"{CodecType}#{Index}";
  public int Index { get; init; } = 0;
  public EStreamCodecType CodecType { get; init; } = EStreamCodecType.Unknown;

  public string CodecName { get; set; } = string.Empty;
  public string CodecLongName { get; set; } = string.Empty;

  public string Language { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;

  public bool Default { get; set; } = false;
  public bool Forced { get; set; } = false;

  protected AMediaStream(int index, string codecType) {
    Index = index;
    CodecType = Enum.Parse<EStreamCodecType>(codecType);
  }

  protected AMediaStream(int index, EStreamCodecType codecType) {
    Index = index;
    CodecType = codecType;
  }
}
