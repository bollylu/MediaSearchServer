namespace MediaSearch.Models;
public interface IMediaInfos : ILanguageDictionary<IMediaInfo> {

  IMediaInfo? Default { get; set; }

}
