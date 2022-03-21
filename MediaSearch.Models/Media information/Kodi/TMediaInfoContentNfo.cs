using MediaSearch.Models;

namespace MediaSearch.Models;
public class TMediaInfoContentNfo : IMediaInfoContent {

  public IMediaSearchLogger<TMediaInfoContentNfo> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMediaInfoContentNfo>();

  public ILanguageDictionary<string> Titles { get; } = new TLanguageDictionary<string>();
  public ILanguageDictionary<string> Descriptions { get; } = new TLanguageDictionary<string>();
  public long Size { get; } = -1;
}
