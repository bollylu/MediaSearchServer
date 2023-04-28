namespace MediaSearch.Models;
public class TMediaSources : TLanguageDictionary<IMediaSource>, IMediaSources {

  public IMediaSource? Default {
    get {
      if (IsEmpty()) {
        return null;
      }
      return this.First().Value;
    }
    set {
      this.Add(value?.Language ?? ELanguage.Unknown, value ?? TMediaSource.Empty);
    }
  }

  public TMediaSources() { }
  public TMediaSources(IMediaSource mediaSource) {
    Add(mediaSource.Language, mediaSource);
  }
  public TMediaSources(IMediaSources mediaSources) {
    foreach (KeyValuePair<ELanguage, IMediaSource> Item in mediaSources) {
      Add(Item.Key, Item.Value);
    }
  }

}
