namespace MediaSearch.Models;

public interface IFilterKeywords {

  /// <summary>
  /// Selection through the keywords
  /// </summary>
  IMultiItemsSelection Keywords { get; }

}
