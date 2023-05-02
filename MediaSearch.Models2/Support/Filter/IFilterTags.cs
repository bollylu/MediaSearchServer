namespace MediaSearch.Models;

public interface IFilterTags {

  /// <summary>
  /// Selection through the tags
  /// </summary>
  [JsonConverter(typeof(TMultiItemsSelectionJsonConverter))]
  IMultiItemsSelection Tags { get; }


}
