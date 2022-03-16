namespace MediaSearch.Models;
public class TMovieInfoContentMeta : TMediaInfoContentMeta, IMovieInfoContent, IJson<TMovieInfoContentMeta> {

  public List<ELanguage> Soundtracks { get; } = new();
  public List<ELanguage> Subtitles { get; } = new();

  public TMovieInfoContentMeta() { }

  public override string ToString() {
    StringBuilder RetVal = new StringBuilder(base.ToString());
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

}
