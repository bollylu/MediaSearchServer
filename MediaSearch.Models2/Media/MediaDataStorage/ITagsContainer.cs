namespace MediaSearch.Models;

public interface ITagsContainer {

  /// <summary>
  /// A list of tags (0 <= count <= n)
  /// </summary>
  List<string> Tags { get; }

}

