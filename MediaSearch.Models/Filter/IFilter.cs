namespace MediaSearch.Models;

public interface IFilter {

  int Page { get; }
  int PageSize { get; }

  string Keywords { get; }
  EFilterType KeywordsSelection { get; }

  string Tags { get;  }
  EFilterType TagSelection { get; }

  int DaysBack { get; }

  int OutputDateMin { get; }
  int OutputDateMax { get; }

  EFilterSortOrder SortOrder { get; }

  List<string> GroupMemberships { get; }

  EFilterGroup GroupFilter { get; }

  bool GroupOnly { get;}
  string Group { get; }
  string SubGroup { get; }

  void Clear();
  void NextPage();
  void PreviousPage();
  void FirstPage();
  void SetPage(int pageNumber);
}
