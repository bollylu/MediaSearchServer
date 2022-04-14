namespace MediaSearch.Models;

public interface ITitles {

  /// <summary>
  /// A list of unique titles with ELanguage
  /// </summary>
  ILanguageTextInfos Titles { get; }

}

