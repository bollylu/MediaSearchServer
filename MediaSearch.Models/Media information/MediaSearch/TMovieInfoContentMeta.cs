namespace MediaSearch.Models;

public class TMovieInfoContentMeta : ALoggable, IMovieInfoContent, IMediaInfoTitles, IJson<TMovieInfoContentMeta> {

  public ILanguageDictionary<string> Titles { get; } = new TLanguageDictionary<string>();
  public ILanguageDictionary<string> Descriptions { get; } = new TLanguageDictionary<string>();
  public List<ELanguage> Soundtracks { get; } = new();
  public List<ELanguage> Subtitles { get; } = new();
  public long Size { get; set; } = -1;


  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMovieInfoContentMeta() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMovieInfoContentMeta>();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString() {
    StringBuilder RetVal = new();
    RetVal.AppendLine($"- {nameof(Titles)}");
    RetVal.AppendLine(Titles.ToString());
    RetVal.AppendLine($"- {nameof(Descriptions)}");
    RetVal.AppendLine(Descriptions.ToString());
    RetVal.AppendLine($"Size={Size}");
    RetVal.AppendLine($"- {nameof(Soundtracks)}");
    foreach (ELanguage LanguageItem in Soundtracks) {
      RetVal.AppendLine(LanguageItem.ToString());
    }
    RetVal.AppendLine($"- {nameof(Subtitles)}");
    foreach (ELanguage LanguageItem in Subtitles) {
      RetVal.AppendLine(LanguageItem.ToString());
    }
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public void Duplicate(TMovieInfoContentMeta meta) {
    Titles.Clear();
    foreach (var TitleItem in meta.Titles) {
      Titles.Add(TitleItem);
    }
    Descriptions.Clear();
    foreach (var DescriptionItem in meta.Descriptions) {
      Titles.Add(DescriptionItem);
    }
    Soundtracks.Clear();
    foreach (var SoundtrackItem in meta.Soundtracks) {
      Soundtracks.Add(SoundtrackItem);
    }
    Subtitles.Clear();
    foreach (var SubtitleItem in meta.Subtitles) {
      Soundtracks.Add(SubtitleItem);
    }
    Size = meta.Size;
  }
}
