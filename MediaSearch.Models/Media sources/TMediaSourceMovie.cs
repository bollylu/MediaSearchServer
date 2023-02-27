namespace MediaSearch.Models;
public class TMediaSourceMovie : IMediaSource, IJson<TMediaSourceMovie>, ILoggable {

  public EMediaSourceType MediaSourceType { get; } = EMediaSourceType.Movie;
  public IStorage? Storage { get; set; }
  public string Name {
    get {
      return _Name ??= $"{MediaSourceType}";
    }
    set {
      _Name = value;
    }
  }
  private string? _Name;
  public string Description { get; set; } = "";
  public ILogger Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMediaSourceMovie>();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSourceMovie() { }
  public TMediaSourceMovie(IStorage storage) {
    Storage = storage;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{nameof(TMediaSourceMovie)} = {Name.WithQuotes()}");
    if (!string.IsNullOrWhiteSpace(Description)) {
      RetVal.AppendLine($"  {nameof(Description)} : {Description.WithQuotes()}");
    }
    RetVal.AppendLine(Database?.ToString(2));
    return RetVal.ToString();
  }
}
