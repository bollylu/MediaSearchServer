namespace MediaSearch.Models;

public interface IMediaInfoContent : IMediaInfoTitles, IMediaInfoContentDescriptions, IJson<IMediaInfoContent> {

  long Size { get; }

}
