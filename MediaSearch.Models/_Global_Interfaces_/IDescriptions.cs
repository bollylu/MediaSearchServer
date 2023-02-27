namespace MediaSearch.Models;

public interface IDescriptions {

  /// <summary>
  /// A list of unique descriptions with ELanguage
  /// </summary>
  ILanguageTextInfos Descriptions { get; }

}

