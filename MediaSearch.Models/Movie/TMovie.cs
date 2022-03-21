using BLTools.Encryption;

namespace MediaSearch.Models;

public class TMovie : AMedia, IMovie, IJson<TMovie> {

  #region --- Public properties ------------------------------------------------------------------------------
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

  public long Size { get; set; }
  public int OutputYear { get; set; }

  public IMovieInfoContent MovieInfoContent { get; } = new TMovieInfoContentMeta();
  public TMovieInfoContentMeta MovieInfoContentMeta => MovieInfoContent as TMovieInfoContentMeta ?? new TMovieInfoContentMeta();
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
  public TMovie() : base() {}

  public TMovie(IMovie movie) : base(movie) {
    Size = movie.Size;
    OutputYear = movie.OutputYear;
    Group = movie.Group;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  protected override string _BuildId() {
    return $"{Name}{OutputYear}".HashToBase64();
  }

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{nameof(Extension)} = {Extension}");
    if (IsGroupMember) {
      RetVal.AppendLine($"{nameof(Group)} = {Group}");
      RetVal.AppendLine($", {nameof(SubGroup)} = {SubGroup}");
    }
    RetVal.AppendLine($"{nameof(Size)} = {Size}");
    RetVal.AppendLine($"{nameof(OutputYear)} = {OutputYear}");

    return RetVal.ToString();
  } 
  #endregion --- Converters ----------------------------------------------------------------------------------

}
