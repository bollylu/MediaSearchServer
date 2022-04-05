namespace MediaSearch.Models;
public interface ILanguageTextInfo {
  /// <summary>
  /// The language of the value
  /// </summary>
  ELanguage Language { get; }

  /// <summary>
  /// The real content
  /// </summary>
  string Value { get; }

  /// <summary>
  /// When used in a list, one item should be marked as principal
  /// </summary>
  bool IsPrincipal { get; }

  string ToString(int indent);
}
