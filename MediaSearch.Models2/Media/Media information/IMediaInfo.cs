namespace MediaSearch.Models;
public interface IMediaInfo : IGroupMembership, ICreation {

  ELanguage Language { get; init; }
  string Name { get; set; }
  string Description { get; set; }
  List<string> Tags { get; init; }

  string Dump();
}
