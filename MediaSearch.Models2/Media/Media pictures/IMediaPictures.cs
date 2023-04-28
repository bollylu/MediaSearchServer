using MediaSearch.Models;

namespace MediaSearch.Models2;

public interface IMediaPictures : ILanguageDictionary<IMediaPicture> {

  IMediaPicture? Default { get; set; }

}
