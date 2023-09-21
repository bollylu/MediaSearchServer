namespace MediaSearch.Models.Support.ffprobe;
public class TMediaProperties {
  public List<IStreamProperties> StreamProperties { get; } = new();

  public IEnumerable<TStreamVideoProperties> VideoProperties => StreamProperties.OfType<TStreamVideoProperties>();
  public IEnumerable<TStreamAudioProperties> AudioProperties => StreamProperties.OfType<TStreamAudioProperties>();
  public IEnumerable<TStreamSubripProperties> SubTitleProperties => StreamProperties.OfType<TStreamSubripProperties>();
  public IEnumerable<TStreamGenericProperties> OtherProperties => StreamProperties.OfType<TStreamGenericProperties>();

}
