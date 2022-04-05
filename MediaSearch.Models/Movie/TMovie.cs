using BLTools.Encryption;

namespace MediaSearch.Models;

public class TMovie : IMovie, IJson<TMovie> {

  #region --- Public properties ------------------------------------------------------------------------------
  public string Id {
    get {
      return _Id ??= _BuildId();
    }
    protected set {
      _Id = value;
    }
  }
  private string? _Id;

  #region --- IName --------------------------------------------
  public string Name { get; set; } = "";
  public string Description { get; set; } = "";
  #endregion --- IName --------------------------------------------

  #region --- ITitles --------------------------------------------
  public ILanguageDictionary<string> Titles { get; } = new TLanguageDictionary<string>();
  #endregion --- ITitles --------------------------------------------

  #region --- ITags --------------------------------------------
  public List<string> Tags { get; } = new();
  #endregion --- ITags --------------------------------------------

  #region --- Storage --------------------------------------------
  public string StorageRoot { get; set; } = "";
  public string StoragePath { get; set; } = "";
  public string FileName { get; set; } = "";
  public string FileExtension { get; set; } = "";
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

  public long Size { get; set; }
  public DateOnly DateAdded { get; set; }
  #endregion --- Storage --------------------------------------------

  public string Group { get; set; } = "";
  public bool IsGroupMember => !string.IsNullOrWhiteSpace(Group);

  public int CreationYear { get; set; }

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

  public TMovie(IMovie movie) {
    Name = movie.Name;
    Description = movie.Description;

    StorageRoot = movie.StorageRoot;
    StoragePath = movie.StoragePath;
    FileName = movie.FileName;
    FileExtension = movie.FileExtension;

    foreach (var TitleItem in movie.Titles) {
      Titles.Add(TitleItem);
    }

    foreach (string TagItem in movie.Tags) {
      Tags.Add(TagItem);
    }

    Size = movie.Size;
    CreationYear = movie.CreationYear;
    Group = movie.Group;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  protected string _BuildId() {
    return $"{Name}{CreationYear}".HashToBase64();
  }

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString() {
    StringBuilder RetVal = new();
    RetVal.AppendLine($"{nameof(Id)} = {Id.WithQuotes()}");
    RetVal.AppendLine($"{nameof(Name)} = {Name.WithQuotes()}");
    RetVal.AppendLine($"{nameof(Description)} = {Description.WithQuotes()}");
    RetVal.AppendLine($"{nameof(StorageRoot)} = {StorageRoot.WithQuotes()}");
    RetVal.AppendLine($"{nameof(StoragePath)} = {StoragePath.WithQuotes()}");
    RetVal.AppendLine($"{nameof(FileName)} = {FileName.WithQuotes()}");
    RetVal.AppendLine($"{nameof(FileExtension)} = {FileExtension.WithQuotes()}");
    RetVal.AppendLine($"{nameof(Extension)} = {Extension}");
    if (Titles.Any()) {
      RetVal.AppendLine(nameof(Titles));
      foreach (var TitleItem in Titles) {
        RetVal.AppendLine($"|- {TitleItem.Key}: {TitleItem.Value.WithQuotes()}");
      }
    } else {
      RetVal.AppendLine($"{nameof(Titles)} is empty");
    }
    if (Soundtracks.Any()) {
      RetVal.AppendLine(nameof(Soundtracks));
      foreach (ELanguage SoundtrackItem in Soundtracks) {
        RetVal.AppendLine($"|- {SoundtrackItem}");
      }
    } else {
      RetVal.AppendLine($"{nameof(Soundtracks)} is empty");
    }
    if (Subtitles.Any()) {
      RetVal.AppendLine(nameof(Subtitles));
      foreach (ELanguage SubtitleItem in Subtitles) {
        RetVal.AppendLine($"|- {SubtitleItem}");
      }
    } else {
      RetVal.AppendLine($"{nameof(Subtitles)} is empty");
    }

    if (Tags.Any()) {
      RetVal.AppendLine(nameof(Tags));
      foreach (string TagItem in Tags) {
        RetVal.AppendLine($"|- {TagItem.WithQuotes()}");
      }
    } else {
      RetVal.AppendLine($"{nameof(Tags)} is empty");
    }

    if (IsGroupMember) {
      RetVal.AppendLine($"{nameof(Group)} = {Group.WithQuotes()}");
    } else {
      RetVal.AppendLine("No group membership");
    }
    RetVal.AppendLine($"{nameof(Size)} = {Size} bytes");
    RetVal.AppendLine($"{nameof(CreationYear)} = {CreationYear}");
    return RetVal.ToString();
  }
  #endregion --- Converters ----------------------------------------------------------------------------------

  public void Duplicate(IMovie movie) {
    if (movie is null) {
      return;
    }
    Name = movie.Name;
    Description = movie.Description;
    Titles.Clear();
    foreach (var TitleItem in movie.Titles) {
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
    CreationYear = movie.CreationYear;

  }
}
