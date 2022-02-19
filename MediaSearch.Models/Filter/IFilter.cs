namespace MediaSearch.Models;

public interface IFilter : IJson<IFilter>, IFilterDaysBack, IFilterKeywords, IFilterTags, IFilterOutputDate, IFilterGroup {

  
  int PageSize { get; }

  EFilterSortOrder SortOrder { get; }

  int Page { get; }

  void Clear();
  void NextPage();
  void PreviousPage();
  void FirstPage();
  void SetPage(int pageNumber);
}
