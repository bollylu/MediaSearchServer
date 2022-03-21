namespace MediaSearch.Models;

public interface IMediaInfoContent : IMediaInfoContentTitles, IMediaInfoContentDescriptions, IJson<IMediaInfoContent> {

  long Size { get; }

}
