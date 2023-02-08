namespace MediaSearch.Models;
public class TMediaInfoContentNfo : ALoggable<TMediaInfoContentNfo>, IMediaInfoContent {

  public TMediaInfoContentNfo() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaInfoContentNfo>();
  }

  public ILanguageDictionary<string> Titles { get; } = new TLanguageDictionary<string>();
  public ILanguageDictionary<string> Descriptions { get; } = new TLanguageDictionary<string>();
  public long Size { get; } = -1;
}
