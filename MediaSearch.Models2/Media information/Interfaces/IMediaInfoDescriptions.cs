namespace MediaSearch.Models;

public interface IMediaInfoDescriptions {

  /// <summary>
  /// A list of unique descriptions with ELanguage
  /// </summary>
  ILanguageTextInfos Descriptions { get; }

}

