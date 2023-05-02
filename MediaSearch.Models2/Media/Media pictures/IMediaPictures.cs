using MediaSearch.Models;

namespace MediaSearch.Models;

public interface IMediaPictures : ILanguageDictionary<IPicture> {

  IPicture? Default { get; set; }

}
