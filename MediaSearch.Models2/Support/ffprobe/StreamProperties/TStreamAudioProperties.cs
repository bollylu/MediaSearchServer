namespace MediaSearch.Models.Support.ffprobe;
public class TStreamAudioProperties : AStreamProperties {

  public int Channels { get; set; } = 0;
  public int SampleRate { get; set; } = 0;
  public int BitRate { get; set; } = 0;
  public string ChannelsLayout { get; set; } = string.Empty;



  public TStreamAudioProperties(int index) : base(index, EStreamCodecType.audio) { }
}
