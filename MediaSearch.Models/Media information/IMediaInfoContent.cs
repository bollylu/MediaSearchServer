namespace MediaSearch.Models;

public interface IMediaInfoContent : IJson<IMediaInfoContent> {

  ILanguageDictionary<string> Titles { get; }
  ILanguageDictionary<string> Descriptions { get; }
  int Size { get; }

}
