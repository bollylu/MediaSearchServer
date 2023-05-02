namespace MediaSearch.Models;

public interface IFilterKeywords {

  /// <summary>
  /// Selection through the keywords
  /// </summary>
  [JsonConverter(typeof(TMultiItemsSelectionJsonConverter))]
  IMultiItemsSelection Keywords { get; }

}
