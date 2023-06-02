namespace MediaSearch.Models;
public interface IMediaInfos : ILanguageDictionary<IMediaInfo> {

  /// <summary>
  /// The default media info
  /// </summary>
  IMediaInfo? Default { get; set; }

  /// <summary>
  /// Get the media info referenced by language
  /// </summary>
  /// <param name="language">The language associated with the media info</param>
  /// <returns>The requested media info or <see langword="null"/> if missing or in error</returns>
  IMediaInfo? Get(ELanguage language);

  /// <summary>
  /// Get all media infos
  /// </summary>
  /// <returns></returns>
  IEnumerable<IMediaInfo> GetAll();

}
