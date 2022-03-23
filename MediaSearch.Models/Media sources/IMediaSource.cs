namespace MediaSearch.Models;
public interface IMediaSource : IName {

  public EMediaSourceType MediaSourceType { get; }

  public IMediaSearchDatabase? Database { get; }
}
