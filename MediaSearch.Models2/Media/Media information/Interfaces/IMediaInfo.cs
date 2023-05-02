namespace MediaSearch.Models;
public interface IMediaInfo : IMediaInfoGroupMembership, IMediaInfoCreation {

  ELanguage Language { get; init; }
  string Title { get; init; }
  string Description { get; set; }
  List<string> Tags { get; init; }

}
