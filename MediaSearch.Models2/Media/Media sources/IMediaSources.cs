namespace MediaSearch.Models;
public interface IMediaSources {

  IMediaSource? GetDefault();
  void SetDefault(IMediaSource mediaSource);

  IMediaSource? Get(ELanguage language);
  IEnumerable<IMediaSource> GetAll();
  bool Add(params IMediaSource[] mediaSource);
  bool AddRange(IEnumerable<IMediaSource> mediaSources);
  bool Remove(IMediaSource mediaSource);
  void Clear();

  bool Any();
  bool Any(Predicate<IMediaSource> predicate);
  bool IsEmpty();

  string Dump();
}
