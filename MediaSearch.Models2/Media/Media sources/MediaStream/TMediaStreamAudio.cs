namespace MediaSearch.Models;
public class TMediaStreamAudio : AMediaStream {

  public int Channels { get; set; } = 0;
  public int SampleRate { get; set; } = 0;
  public int BitRate { get; set; } = 0;
  public string ChannelsLayout { get; set; } = string.Empty;



  public TMediaStreamAudio(int index) : base(index, EStreamCodecType.audio) { }
}
