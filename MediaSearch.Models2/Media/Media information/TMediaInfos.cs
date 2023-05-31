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

  #region --- Constructor(s) ---------------------------------------------------------------------------------
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
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public IMediaInfo? Get(ELanguage language) {
    if (this.ContainsKey(language)) {
      return this[language];
    }

    return null;
  }

  public IEnumerable<IMediaInfo> GetAll() {
    foreach (KeyValuePair<ELanguage, IMediaInfo> MediaInfoItem in this) {
      yield return MediaInfoItem.Value;
    }
  }
}
