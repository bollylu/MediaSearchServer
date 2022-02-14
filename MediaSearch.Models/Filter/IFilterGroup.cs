namespace MediaSearch.Models;

public interface IFilterGroup {

  List<string> GroupMemberships { get; }

  EFilterGroup GroupFilter { get; }

  bool GroupOnly { get; }
  string Group { get; }
  string SubGroup { get; }


}
