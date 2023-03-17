namespace MediaSearch.Models;

public interface IMediaInfoTags {

  /// <summary>
  /// A list of tags (0 <= count <= n)
  /// </summary>
  List<string> Tags { get; }

}

