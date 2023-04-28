using System.Runtime.CompilerServices;

namespace MediaSearch.Models;
public class TMediaInfos : TLanguageDictionary<IMediaInfo>, IMediaInfos {

  public IMediaInfo? Default {
    get {
      if (IsEmpty()) {
        return null;
      }
      return this.First().Value;
    }
    set {
      this.Add(value?.Language ?? ELanguage.Unknown, value ?? TMediaInfo.Empty);
    }
  }

  public TMediaInfos() { }
  public TMediaInfos(params IMediaInfo[] mediaInfos) {
    foreach (IMediaInfo Item in mediaInfos) {
      Add(Item.Language, Item);
    }
  }
  public TMediaInfos(IMediaInfos mediaInfos) {
    foreach (KeyValuePair<ELanguage, IMediaInfo> Item in mediaInfos) {
      Add(Item.Key, Item.Value);
    }
  }
}
