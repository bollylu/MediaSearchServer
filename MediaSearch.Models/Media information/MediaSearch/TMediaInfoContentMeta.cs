namespace MediaSearch.Models;

public class TMediaInfoContentMeta : IMediaInfoContent, IJson<TMediaInfoContentMeta> {

  public IMediaSearchLogger<TMediaInfoContentMeta> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMediaInfoContentMeta>();

  public ILanguageDictionary<string> Titles { get; } = new TLanguageDictionary<string>();
  public ILanguageDictionary<string> Descriptions { get; } = new TLanguageDictionary<string>();
  public int Size { get; set; } = -1;


  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"- {nameof(Titles)}");
    RetVal.AppendLine(Titles.ToString());
    RetVal.AppendLine($"- {nameof(Descriptions)}");
    RetVal.AppendLine(Descriptions.ToString());
    RetVal.AppendLine($"Size={Size}");
    return RetVal.ToString();
  }
}
