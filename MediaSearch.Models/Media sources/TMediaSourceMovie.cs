namespace MediaSearch.Models;

public class TMediaSourceMovie : ALoggable, IMediaSource, IJson<TMediaSourceMovie> {

  public EMediaType MediaSourceType { get; } = EMediaType.Movie;
  //public IStorage? Storage { get; set; }
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

  public string RootStorage { get; set; } = "";

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSourceMovie() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaSourceMovie>();
  }
  public TMediaSourceMovie(string rootStorage) : this() {
    RootStorage = rootStorage;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{nameof(TMediaSourceMovie)} = {Name.WithQuotes()}");
    if (!string.IsNullOrWhiteSpace(Description)) {
      RetVal.AppendLine($"  {nameof(Description)} : {Description.WithQuotes()}");
    }
    //RetVal.AppendLine(Database?.ToString(2));
    return RetVal.ToString();
  }

  public string ToString(int indent) {
    throw new NotImplementedException();
  }

  public Type? MediaType { get; }
}
