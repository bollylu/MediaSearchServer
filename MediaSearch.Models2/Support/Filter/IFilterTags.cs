using MediaSearch.Models;

namespace MediaSearch.Models2;

public interface IFilterTags {

  /// <summary>
  /// Selection through the tags
  /// </summary>
  IMultiItemsSelection Tags { get; }


}
