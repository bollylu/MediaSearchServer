namespace MediaSearch.Models;

public interface IFilterKeywords {

  string Keywords { get; }
  EFilterType KeywordsSelection { get; }

}
