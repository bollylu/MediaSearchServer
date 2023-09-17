using MediaSearch.Models;

namespace MediaSearch.Models2;

public class TMedia : AMedia {

  public TMedia() { }

  public TMedia(TMediaInfos mediaInfos, IEnumerable<IMediaSource> mediaSources) {
    MediaInfos = new TMediaInfos(mediaInfos);
    MediaSources = new TMediaSources(mediaSources);
  }
}
