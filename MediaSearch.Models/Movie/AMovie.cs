using BLTools.Encryption;

namespace MediaSearch.Models;

public abstract class AMovie : AMedia, IMovie {

  #region --- Public properties ------------------------------------------------------------------------------
  public EMovieExtension Extension =>
    FileExtension switch {
      "avi" => EMovieExtension.AVI,
      "mkv" => EMovieExtension.MKV,
      "mp4" => EMovieExtension.MP4,
      "iso" => EMovieExtension.ISO,
      _ => EMovieExtension.Unknown,
    };

  public string Group { get; set; }
  public long Size { get; set; }
  public int OutputYear { get; set; }
  #endregion --- Public properties ---------------------------------------------------------------------------

  #region --- Picture --------------------------------------------
  public static byte[] PictureMissing {
    get {
      if (_PictureMissingBytes is null) {
        _PictureMissingBytes = Support.GetPicture("missing", ".jpg");
      }
      return _PictureMissingBytes;
    }
  }

  private static byte[] _PictureMissingBytes;
  #endregion --- Picture --------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  protected AMovie() { }
  protected AMovie(IMovie movie) : base(movie) {
    Size = movie.Size;
    OutputYear = movie.OutputYear;
    Group = movie.Group;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  protected override string _BuildId() {
    return $"{Name}{OutputYear}".HashToBase64();
  }
  public override string ToString() {
    StringBuilder RetVal = new StringBuilder(base.ToString());
    RetVal.AppendLine($"{nameof(Extension)} = {Extension}");
    RetVal.AppendLine($"{nameof(Group)} = {Group}");
    RetVal.AppendLine($"{nameof(Size)} = {Size}");
    RetVal.AppendLine($"{nameof(OutputYear)} = {OutputYear}");

    return RetVal.ToString();
  }

  public abstract override IMovie ParseJson(string source);
  public override IMovie ParseJson(JsonElement source) {
    return ParseJson(source.GetRawText());
  }
}
