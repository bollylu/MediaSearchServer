namespace MediaSearch.Models;

public interface IFilterTags {

  /// <summary>
  /// List of tags in a string, separated by space
  /// </summary>
  string Tags { get; }

  /// <summary>
  /// How to combine multiple tags
  /// </summary>
  EFilterType TagSelection { get; }

}
