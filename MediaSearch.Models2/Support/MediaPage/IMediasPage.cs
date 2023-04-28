namespace MediaSearch.Models;

public interface IMediasPage : IJson {

  string Name { get; }
  string Source { get; }

  List<IMedia> Medias { get; }

  int Page { get; }
  int AvailablePages { get; set; }
  int AvailableMedias { get; set; }

  string ToString(bool withDetails);
}
