namespace MediaSearch.Models;

public class TMediaSourceMovie : AMediaSource, IJson<TMediaSourceMovie> {

  public string Name {
    get {
      return _Name ??= $"{MediaSourceType}";
    }
    set {
      _Name = value;
    }
  }
  private string? _Name;

  public EMovieExtension Extension =>
    FileExtension.ToLowerInvariant() switch {
      "avi" => EMovieExtension.AVI,
      "mkv" => EMovieExtension.MKV,
      "mp4" => EMovieExtension.MP4,
      "iso" => EMovieExtension.ISO,
      _ => EMovieExtension.Unknown,
    };

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSourceMovie() : base() {
    MediaSourceType = EMediaType.Movie;
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaSourceMovie>();
  }
  public TMediaSourceMovie(string rootStorage) : this() {
    StorageRoot = rootStorage;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder(base.ToString(indent));
    RetVal.AppendLine($"{nameof(Name)} = {Name.WithQuotes()}");
    if (!string.IsNullOrWhiteSpace(Description)) {
      RetVal.AppendLine($"  {nameof(Description)} : {Description.WithQuotes()}");
    }
    return RetVal.ToString();
  }

}
