namespace MediaSearch.Models.Support.ffprobe;
public class TStreamVideoProperties : AStreamProperties {

  public int Width { get; set; } = 0;
  public int Height { get; set; } = 0;
  public string ColorSpace { get; set; } = string.Empty;
  public string PixelFormat { get; set; } = string.Empty;

  public TStreamVideoProperties(int index) : base(index, EStreamCodecType.video) { }

}
