namespace MediaSearch.Models;
public interface IMediaInfos {

  IEnumerable<IMediaInfo> GetAll();
  IMediaInfo? GetDefault();
  IMediaInfo? Get(ELanguage language);
  bool Add(params IMediaInfo[] mediaInfo);
  bool AddRange(IEnumerable<IMediaInfo> mediaInfo);
  bool Remove(IMediaInfo mediaInfo);
  void Clear();

  bool Any();
  bool Any(Func<IMediaInfo, bool> predicate);
  bool IsEmpty();

  void SetTitle(ELanguage language, string title);

}
