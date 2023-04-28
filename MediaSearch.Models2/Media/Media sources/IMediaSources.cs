namespace MediaSearch.Models;
public interface IMediaSources : ILanguageDictionary<IMediaSource> {
  IMediaSource? Default { get; set; }
}
