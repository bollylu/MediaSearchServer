namespace MediaSearch.Models;

public interface IFilter : IFilterDaysBack, IFilterKeywords, IFilterTags, IFilterOutputDate, IFilterGroup {


  int PageSize { get; }

  EFilterSortOrder SortOrder { get; }

  int Page { get; }

  void Clear();
  void NextPage();
  void PreviousPage();
  void FirstPage();
  void SetPage(int pageNumber);

  string ToString();
  string ToString(int indent);
}
