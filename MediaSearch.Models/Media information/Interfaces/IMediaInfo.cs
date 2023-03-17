namespace MediaSearch.Models;
public interface IMediaInfo {

  ELanguage Language { get; }
  string Title { get; set; }
  string Description { get; set; }
  List<string> Tags { get; }

}
