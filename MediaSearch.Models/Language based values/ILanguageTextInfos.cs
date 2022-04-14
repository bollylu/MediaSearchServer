namespace MediaSearch.Models;
public interface ILanguageTextInfos {

  void Add(ILanguageTextInfo languageTextInfo);
  void Add(string value, bool isPrincipal = false);
  void Add(ELanguage language, string value, bool isPrincipal = false);
  void Clear();
  bool Any();
  bool IsEmpty();
  int Count();

  ILanguageTextInfo? Get(ELanguage language);
  ILanguageTextInfo? GetPrincipal();
  IEnumerable<ILanguageTextInfo> GetAll();

  string ToString(int indent);
}
