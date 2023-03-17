namespace MediaSearch.Models;
public interface ILanguageTextInfos {

  void Add(ILanguageTextInfo languageTextInfo);
  void Add(string value);
  void Add(ELanguage language, string value);
  void Clear();
  bool Any();
  bool IsEmpty();
  int Count();

  ILanguageTextInfo? Get(ELanguage language);
  ILanguageTextInfo? GetPrincipal();
  IEnumerable<ILanguageTextInfo> GetAll();

  void SetPrincipal(ELanguage language);
  void SetPrincipal(ILanguageTextInfo languageTextInfo);

  bool HasMoreThanOnePrincipal();

  string ToString(int indent);
}
