namespace MediaSearch.Models;

public class TMovieInfoContentMeta : IMediaInfoContent, 
                                     IJson<TMovieInfoContentMeta>, 
                                     IMediaSearchLoggable<TMovieInfoContentMeta>, 
                                     IMovieInfoContent, 
                                     IMediaInfoContentTitles {

  public IMediaSearchLogger<TMovieInfoContentMeta> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMovieInfoContentMeta>();

  public ILanguageTextInfos Titles { get; } = new TLanguageTextInfos();
  public ILanguageTextInfos Descriptions { get; } = new TLanguageTextInfos();
  public List<ELanguage> Soundtracks { get; } = new();
  public List<ELanguage> Subtitles { get; } = new();
  public long Size { get; set; } = -1;
  public int CreationYear { get; set; } = -1;

  public List<string> Genres { get; } = new();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMovieInfoContentMeta() { }
  public TMovieInfoContentMeta(IMovie movie) {
    foreach(var TitleItem in movie.Titles.GetAll()) {
      Titles.Add(TitleItem);
    }
    
  } 
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    StringBuilder RetVal = new();
    string IndentSpace = new string(' ', indent);
    RetVal.AppendLine($"{IndentSpace}- {nameof(Titles)}");
    RetVal.AppendLine($"{IndentSpace}{Titles.ToString(2)}");

    RetVal.AppendLine($"{IndentSpace}- {nameof(Descriptions)}");
    RetVal.AppendLine($"{IndentSpace}{Descriptions.ToString(2)}");

    RetVal.AppendLine($"{IndentSpace}{nameof(Size)} = {Size}");

    RetVal.AppendLine($"{IndentSpace}{nameof(CreationYear)} = {CreationYear}");

    RetVal.AppendLine($"{IndentSpace}- {nameof(Soundtracks)}");
    foreach (ELanguage LanguageItem in Soundtracks) {
      RetVal.AppendLine($"{IndentSpace}  {LanguageItem}");
    }

    RetVal.AppendLine($"{IndentSpace}- {nameof(Subtitles)}");
    foreach (ELanguage LanguageItem in Subtitles) {
      RetVal.AppendLine($"{IndentSpace}  {LanguageItem}");
    }

    RetVal.AppendLine($"{IndentSpace}- {nameof(Genres)}");
    foreach (string GenreItem in Genres) {
      RetVal.AppendLine($"{IndentSpace}  {GenreItem}");
    }

    return RetVal.ToString();
  }


  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public void Duplicate(TMovieInfoContentMeta meta) {
    Titles.Clear();
    foreach (var TitleItem in meta.Titles.GetAll()) {
      Titles.Add(TitleItem);
    }
    Descriptions.Clear();
    foreach (var DescriptionItem in meta.Descriptions.GetAll()) {
      Descriptions.Add(DescriptionItem);
    }
    Soundtracks.Clear();
    foreach(var SoundtrackItem in meta.Soundtracks) {
      Soundtracks.Add(SoundtrackItem);
    }
    Subtitles.Clear();
    foreach (var SubtitleItem in meta.Subtitles) {
      Soundtracks.Add(SubtitleItem);
    }
    Size = meta.Size;
    CreationYear = meta.CreationYear;

    Genres.Clear();
    foreach (string GenreItem in meta.Genres) {
      Genres.Add(GenreItem);
    }
  }
}
