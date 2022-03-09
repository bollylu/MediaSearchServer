namespace MediaSearch.Models.Language;
public record RLanguage  {

  public ELanguage Language { get; init; } = ELanguage.Unknown;
  public string LanguageCode { get; init; } = "";
  public string LanguageText { get; init; } = "";

  public RLanguage() { }
  public RLanguage(ELanguage language, string languageCode, string languageText) {
    Language = language;
    LanguageCode = languageCode;
    LanguageText = languageText;
  }
  
}
