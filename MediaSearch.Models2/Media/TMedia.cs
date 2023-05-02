using MediaSearch.Models;

namespace MediaSearch.Models2;

public class TMedia : AMedia {

  public TMedia() { }

  public TMedia(string name) {
    Name = name;
  }

  public TMedia(string name, IMediaInfos mediaInfos, IMediaSources mediaSources) {
    Name = name;
    MediaInfos = new TMediaInfos(mediaInfos);
    MediaSources = new TMediaSources(mediaSources);
  }
}
