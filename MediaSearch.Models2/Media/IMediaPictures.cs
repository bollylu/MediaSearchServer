namespace MediaSearch.Models;
public interface IMediaPictures {

  IMediaPicture? GetDefault();
  IMediaPicture? Get(ELanguage language);
  IEnumerable<IMediaPicture> GetAll();
  bool Add(IMediaPicture mediaPicture);
  bool AddRange(IEnumerable<IMediaPicture> mediaPicture);
  bool Remove(IMediaPicture mediaPicture);
  void Clear();
  void SetDefault(IMediaPicture mediaPicture);

  bool Any();
  bool Any(Func<IMediaPicture, bool> predicate);
  bool IsEmpty();
}
