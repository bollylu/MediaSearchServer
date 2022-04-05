namespace MediaSearch.Models;

public interface ITitles {

  /// <summary>
  /// A list of alternate KeyValuePairs (language & name) (0 <= count <= n)
  /// </summary>
  ILanguageDictionary<string> Titles { get; }

}

