using MediaSearch.Models;

namespace MediaSearch.Models2;

public interface IFilter : IJson, IFilterDaysBack, IFilterKeywords, IFilterTags, IFilterOutputDate, IFilterGroup {
  /// <summary>
  /// The language used in this filter
  /// </summary>
  ELanguage Language { get; }

  /// <summary>
  /// The page number
  /// </summary>
  int Page { get; }

  /// <summary>
  /// The size of the page in records (result will be 0 => PageSize records)
  /// </summary>
  int PageSize { get; }

  /// <summary>
  /// How to sort the records, hence allowing consistent navigation in the previous or next page
  /// </summary>
  EFilterSortOrder SortOrder { get; }

  /// <summary>
  /// Clear all the arguments of the filter
  /// </summary>
  void Clear();

  /// <summary>
  /// Increment the page number if allowed
  /// </summary>
  void NextPage();

  /// <summary>
  /// Decrement the page number if allowed
  /// </summary>
  void PreviousPage();

  /// <summary>
  /// Goes directly to the first page
  /// </summary>
  void FirstPage();

  /// <summary>
  /// Change the page number to a target, if allowed
  /// </summary>
  /// <param name="pageNumber">The page number to reach</param>
  void SetPage(int pageNumber);
}
