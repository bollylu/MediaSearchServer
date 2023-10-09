namespace MediaSearch.Models;
public interface IMediaInfos {

  IEnumerable<IMediaInfo> GetAll();
  IMediaInfo? GetDefault();
  bool SetDefault(IMediaInfo mediaInfo);
  IMediaInfo? Get(ELanguage language);
  bool Add(params IMediaInfo[] mediaInfo);
  bool AddRange(IEnumerable<IMediaInfo> mediaInfo);
  bool Remove(IMediaInfo mediaInfo);
  void Clear();

  bool Any();
  bool Any(Func<IMediaInfo, bool> predicate);
  bool IsEmpty();



}
