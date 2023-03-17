namespace MediaSearch.Models;

public interface IMediaInfoContent : IMediaInfoTitles, IMediaInfoContentDescriptions {

  long Size { get; }

}
