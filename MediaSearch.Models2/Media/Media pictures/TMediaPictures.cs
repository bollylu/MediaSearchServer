using MediaSearch.Models;

namespace MediaSearch.Models2;

public class TMediaPictures : TLanguageDictionary<IPicture>, IMediaPictures {

  public IPicture? Default {
    get {
      if (IsEmpty()) {
        return null;
      }
      return this.First().Value;
    }
    set {
      this.Add(value?.Language ?? ELanguage.Unknown, value ?? TPicture.Default);
    }
  }

  public TMediaPictures() { }
  public TMediaPictures(IPicture picture) {
    Add(picture.Language, picture);
  }
  public TMediaPictures(IMediaPictures mediaPictures) {
    foreach (KeyValuePair<ELanguage, IPicture> Item in mediaPictures) {
      Add(Item.Key, Item.Value);
    }
  }
}
