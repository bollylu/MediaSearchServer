using MediaSearch.Models;

namespace MediaSearch.Models2;

public interface IFilterKeywords {

  /// <summary>
  /// Selection through the keywords
  /// </summary>
  IMultiItemsSelection Keywords { get; }

}
