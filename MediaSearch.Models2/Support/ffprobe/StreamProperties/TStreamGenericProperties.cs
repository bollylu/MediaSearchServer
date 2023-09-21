namespace MediaSearch.Models.Support.ffprobe;
public class TStreamGenericProperties : AStreamProperties {

  public string CodecTypeUnknown { get; set; } = string.Empty;

  public TStreamGenericProperties(int index) : base(index, EStreamCodecType.Unknown) { }

}
