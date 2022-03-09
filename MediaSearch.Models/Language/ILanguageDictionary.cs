namespace MediaSearch.Models.Language;
public interface ILanguageDictionary {
}

public interface ILanguageDictionary<T> : ILanguageDictionary, IDictionary<ELanguage, T> {

  
}
