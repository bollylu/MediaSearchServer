namespace MediaSearch.Models;
public class TMediaStreamUnknown : AMediaStream {

  public string CodecTypeUnknown { get; set; } = string.Empty;

  public TMediaStreamUnknown(int index) : base(index, EStreamCodecType.Unknown) { }

}
