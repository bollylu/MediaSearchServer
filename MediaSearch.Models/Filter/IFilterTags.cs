namespace MediaSearch.Models;

public interface IFilterTags {

  string Tags { get; }
  EFilterType TagSelection { get; }

}
