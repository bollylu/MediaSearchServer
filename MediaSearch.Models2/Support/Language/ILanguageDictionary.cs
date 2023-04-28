namespace MediaSearch.Models;
public interface ILanguageDictionary {
}

public interface ILanguageDictionary<T> : ILanguageDictionary, IDictionary<ELanguage, T>, IToStringIndent {

  bool Exists(ELanguage language);

}
