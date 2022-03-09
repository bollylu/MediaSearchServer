namespace MediaSearch.Models.Language;
public static class Languages {

  private static readonly List<RLanguage> _Items = new();

  static Languages() {
    _Items.Add(new RLanguage(ELanguage.Unknown, "??", "(unknown)"));
    _Items.Add(new RLanguage(ELanguage.English, "EN", "English"));
    _Items.Add(new RLanguage(ELanguage.EnglishUs, "EN-US", "English (USA)"));
    _Items.Add(new RLanguage(ELanguage.EnglishUk, "EN-UK", "English (UK)"));
    _Items.Add(new RLanguage(ELanguage.French, "FR", "French"));
    _Items.Add(new RLanguage(ELanguage.FrenchFrance, "FR-FR", "French (France)"));
    _Items.Add(new RLanguage(ELanguage.FrenchBelgium, "FR-BE", "French (Belgique)"));
    _Items.Add(new RLanguage(ELanguage.FrenchQuebec, "FR-CA", "French (Québec)"));
    _Items.Add(new RLanguage(ELanguage.Japanese, "JA", "Japanese"));
    _Items.Add(new RLanguage(ELanguage.German, "GE", "German"));
    _Items.Add(new RLanguage(ELanguage.Dutch, "DU", "Dutch"));
    _Items.Add(new RLanguage(ELanguage.Korean, "KR", "Korean"));
    _Items.Add(new RLanguage(ELanguage.Chinese, "CN", "Chinese"));
    _Items.Add(new RLanguage(ELanguage.ChineseCantonese, "CN-CA", "Chinese (Cantonese)"));
    _Items.Add(new RLanguage(ELanguage.ChineseMandarin, "CN-MA", "Chinese (Mandarin)"));
  }

  public static RLanguage GetLanguage(ELanguage language) {
    return _Items.FirstOrDefault(x => x.Language == language) ?? _Items.First(x => x.Language == ELanguage.Unknown);
  }

  public static RLanguage GetLanguageFromCode(string languageCode) {
    return _Items.FirstOrDefault(x => x.LanguageCode.Equals(languageCode, StringComparison.InvariantCultureIgnoreCase)) ?? _Items.First(x => x.Language == ELanguage.Unknown);
  }

  public static string GetLanguageCode(ELanguage language) {
    return _Items.FirstOrDefault(x => x.Language == language)?.LanguageCode ?? "??" ;
  }

  public static IEnumerable<RLanguage> GetAll() {
    foreach (RLanguage LanguageItem in _Items) {
      yield return LanguageItem;
    }
  }
}
