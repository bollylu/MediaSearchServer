using BLTools.Encryption;

namespace MediaSearch.Models;

public class TMovie : AMedia, IMovie, IJson<TMovie>, IDirty {

  #region --- Public properties ------------------------------------------------------------------------------

  public EMovieExtension Extension {
    get {
      return (FileExtension?.TrimStart('.') ?? "") switch {
        "avi" => EMovieExtension.AVI,
        "mkv" => EMovieExtension.MKV,
        "mp4" => EMovieExtension.MP4,
        "iso" => EMovieExtension.ISO,
        _ => EMovieExtension.Unknown,
      };
    }
  }

  public List<ELanguage> Soundtracks { get; } = new();
  public List<ELanguage> Subtitles { get; } = new();

  
  #endregion --- Public properties ---------------------------------------------------------------------------

  #region --- Picture --------------------------------------------
  public static byte[] PictureMissing {
    get {
      return _PictureMissingBytes ??= Support.GetPicture("missing", ".jpg");
    }
  }

  private static byte[]? _PictureMissingBytes;
  #endregion --- Picture --------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMovie() : base() { }

  public TMovie(string name, int creationYear = -1) : base() {
    Titles.Add(ELanguage.Unknown, name, true);
    CreationDate = new DateOnly(creationYear, 1, 1);
  }

  public TMovie(ELanguage language, string name, int creationYear = -1) : base() {
    Titles.Add(language, name, true);
    CreationDate = new DateOnly(creationYear, 1, 1);
  }

  public TMovie(IMovie movie) : base(movie) {
    foreach(var SoundtrackItem in movie.Soundtracks) {
      Soundtracks.Add(SoundtrackItem);
    }
    foreach (var SubtitleItem in movie.Subtitles) {
      Subtitles.Add(SubtitleItem);
    }
    SetDirty();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  protected override string _BuildId() {
    return $"{Name}{CreationYear}".HashToBase64().ToByteArray().ToHexString("");
  }

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int Indent) {
    StringBuilder RetVal = new(base.ToString(Indent));
    string IndentSpace = new string(' ', Indent);
    
    RetVal.AppendLine($"{IndentSpace}{nameof(Extension)} = {Extension}");
    
    if (Soundtracks.Any()) {
      RetVal.AppendLine($"{IndentSpace}{nameof(Soundtracks)}");
      foreach (ELanguage SoundtrackItem in Soundtracks) {
        RetVal.AppendLine($"{IndentSpace}|- {SoundtrackItem}");
      }
    } else {
      RetVal.AppendLine($"{IndentSpace}{nameof(Soundtracks)} is empty");
    }
    if (Subtitles.Any()) {
      RetVal.AppendLine($"{IndentSpace}{nameof(Subtitles)}");
      foreach (ELanguage SubtitleItem in Subtitles) {
        RetVal.AppendLine($"{IndentSpace}|- {SubtitleItem}");
      }
    } else {
      RetVal.AppendLine($"{IndentSpace}{nameof(Subtitles)} is empty");
    }
    return RetVal.ToString();
  }
  
  #endregion --- Converters ----------------------------------------------------------------------------------

  public void ReplaceBy(IMovie movie) {
    if (movie is null) {
      return;
    }
    Titles.Clear();
    foreach (var TitleItem in movie.Titles.GetAll()) {
      Titles.Add(TitleItem);
    }
    Tags.Clear();
    Tags.AddRange(movie.Tags);
    StorageRoot = movie.StorageRoot;
    StoragePath = movie.StoragePath;
    FileName = movie.FileName;
    FileExtension = movie.FileExtension;
    DateAdded = movie.DateAdded;
    Size = movie.Size;
    Group = movie.Group;
    CreationDate = movie.CreationDate;

  }
}
