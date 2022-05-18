using BLTools.Encryption;

namespace MediaSearch.Models;

public class TMovie : AMedia, IMovie, IJson<TMovie>, IDirty, IMediaSearchLoggable<TMovie> {

  
  public IMediaSearchLogger<TMovie> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMovie>();

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
  public TMovie() : base() {
    MediaType = EMediaSourceType.Movie;
  }

  public TMovie(string name, int creationYear = -1) : base() {
    MediaType = EMediaSourceType.Movie;
    Titles.Add(DEFAULT_LANGUAGE, name);
    try {
      if (creationYear == -1) {
        CreationDate = DateOnly.MinValue;
      } else {
        CreationDate = new DateOnly(creationYear, 1, 1);
      }
    } catch {
      Logger.LogWarning($"Unable to set {nameof(CreationDate)} of {nameof(TMovie)} {name.WithQuotes()} : {nameof(creationYear)} is invalid : {creationYear}");
      CreationDate = DateOnly.MinValue;
    }
  }

  public TMovie(ELanguage language, string name, int creationYear = -1) : base() {
    MediaType = EMediaSourceType.Movie;
    Titles.Add(language, name);
    try {
      if (creationYear == -1) {
        CreationDate = DateOnly.MinValue;
      } else {
        CreationDate = new DateOnly(creationYear, 1, 1);
      }
    } catch {
      Logger.LogWarning($"Unable to set {nameof(CreationDate)} of {nameof(TMovie)} {language}:{name.WithQuotes()} : {nameof(creationYear)} is invalid : {creationYear}");
      CreationDate = DateOnly.MinValue;
    }
  }

  public TMovie(IMovie movie) : base(movie) {
    MediaType = EMediaSourceType.Movie;
    foreach (var SoundtrackItem in movie.Soundtracks) {
      Soundtracks.Add(SoundtrackItem);
    }
    foreach (var SubtitleItem in movie.Subtitles) {
      Subtitles.Add(SubtitleItem);
    }
    SetDirty();
  }

  public override void Dispose() {
    base.Dispose();
    Soundtracks.Clear();
    Subtitles.Clear();
  }
  public override async ValueTask DisposeAsync() {
    await base.DisposeAsync();
    Soundtracks.Clear();
    Subtitles.Clear();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  protected override string _BuildId() {
    return $"{Name}{CreationYear}".HashToBase64().ToByteArray().ToHexString("");
  }

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new(base.ToString(indent));

    RetVal.AppendIndent($"- {nameof(Extension)} = {Extension}", indent);

    if (Soundtracks.Any()) {
      RetVal.AppendIndent($"- {nameof(Soundtracks)}", indent);
      foreach (ELanguage SoundtrackItem in Soundtracks) {
        RetVal.AppendIndent($"- {SoundtrackItem}", indent + 2);
      }
    } else {
      RetVal.AppendIndent($"- {nameof(Soundtracks)} is empty", indent);
    }
    if (Subtitles.Any()) {
      RetVal.AppendIndent($"- {nameof(Subtitles)}", indent);
      foreach (ELanguage SubtitleItem in Subtitles) {
        RetVal.AppendIndent($"- {SubtitleItem}", indent + 2);
      }
    } else {
      RetVal.AppendIndent($"- {nameof(Subtitles)} is empty", indent);
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
