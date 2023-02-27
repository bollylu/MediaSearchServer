namespace MediaSearch.Models;
public interface ILanguageDictionary {
}

public interface ILanguageDictionary<T> : ILanguageDictionary, IDictionary<ELanguage, T> {
  string ToString(int indent);
}
