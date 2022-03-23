namespace MediaSearch.Models;
public class TMediaSourceMovie : IMediaSource, IJson<TMediaSourceMovie>, IMediaSearchLoggable<TMediaSourceMovie> {

  public EMediaSourceType MediaSourceType { get; } = EMediaSourceType.Movies;
  public IMediaSearchDatabase? Database { get; set; }
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
  public IMediaSearchLogger<TMediaSourceMovie> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMediaSourceMovie>();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSourceMovie() { }
  public TMediaSourceMovie(IMediaSearchDatabase database) {
    Database = database;
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
