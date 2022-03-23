using System.Xml.Linq;

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

  #region --- IMultiNames --------------------------------------------
  public Dictionary<string, string> AltNames { get; } = new();
  #endregion --- IMultiNames --------------------------------------------

  #region --- ITags --------------------------------------------
  public List<string> Tags { get; } = new();
  #endregion --- ITags --------------------------------------------

  public string StorageRoot { get; set; } = "";
  public string StoragePath { get; set; } = "";
  public string FileName { get; set; } = "";
  public string FileExtension { get; set; } = "";
  public long Size { get; set; }

  public EMovieExtension Extension =>
    FileExtension switch {
      "avi" => EMovieExtension.AVI,
      "mkv" => EMovieExtension.MKV,
      "mp4" => EMovieExtension.MP4,
      "iso" => EMovieExtension.ISO,
      _ => EMovieExtension.Unknown,
    };

  public string Group { get; set; } = "";
  public string SubGroup { get; set; } = "";

  public bool IsGroupMember => !string.IsNullOrWhiteSpace(Group);


  public int CreationYear { get; set; }

  public IMovieInfoContent MovieInfoContent { get; } = new TMovieInfoContentMeta();
  public TMovieInfoContentMeta MovieInfoContentMeta => MovieInfoContent as TMovieInfoContentMeta ?? new TMovieInfoContentMeta();

  public DateOnly DateAdded { get; set; }
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

    foreach (KeyValuePair<string, string> AltNameItem in movie.AltNames) {
      AltNames.Add(AltNameItem.Key, AltNameItem.Value);
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
    RetVal.AppendLine($"{nameof(Id)} = \"{Id}\"");
    RetVal.AppendLine($"{nameof(Name)} = {Name}");
    RetVal.AppendLine($"{nameof(Description)} = {Description}");
    RetVal.AppendLine($"{nameof(StorageRoot)} = {StorageRoot}");
    RetVal.AppendLine($"{nameof(StoragePath)} = {StoragePath}");
    RetVal.AppendLine($"{nameof(FileName)} = {FileName}");
    RetVal.AppendLine($"{nameof(FileExtension)} = {FileExtension}");
    if (AltNames.Any()) {
      RetVal.AppendLine("Alt. names");
      foreach (KeyValuePair<string, string> AltNameItem in AltNames) {
        RetVal.AppendLine($"|- {AltNameItem.Key}:{AltNameItem.Value}");
      }
    } else {
      RetVal.AppendLine($"{nameof(AltNames)} is empty");
    }
    if (Tags.Any()) {
      RetVal.AppendLine("Tags");
      foreach (string TagItem in Tags) {
        RetVal.AppendLine($"|- {TagItem}");
      }
    } else {
      RetVal.AppendLine($"{nameof(Tags)} is empty");
    }
    RetVal.AppendLine($"{nameof(Extension)} = {Extension}");
    if (IsGroupMember) {
      RetVal.AppendLine($"{nameof(Group)} = {Group}");
    }
    RetVal.AppendLine($"{nameof(Size)} = {Size}");
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
    AltNames.Clear();
    foreach (var NameItem in movie.AltNames) {
      AltNames.Add(NameItem.Key, NameItem.Value);
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
    SubGroup = movie.SubGroup;
    CreationYear = movie.CreationYear;

  }
}
