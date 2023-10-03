namespace MediaSearch.Models;
public class TMediaSourceStreamProperties {
  public List<IStreamProperties> StreamProperties { get; } = new();

  public IEnumerable<TStreamVideoProperties> VideoProperties => StreamProperties.OfType<TStreamVideoProperties>();
  public IEnumerable<TStreamAudioProperties> AudioProperties => StreamProperties.OfType<TStreamAudioProperties>();
  public IEnumerable<TStreamSubTitleProperties> SubTitleProperties => StreamProperties.OfType<TStreamSubTitleProperties>();
  public IEnumerable<TStreamGenericProperties> OtherProperties => StreamProperties.OfType<TStreamGenericProperties>();
  public IEnumerable<TStreamDataProperties> DataProperties => StreamProperties.OfType<TStreamDataProperties>();


}
