namespace MediaSearch.Models;

public abstract class AMediaSource : ALoggable, IMediaSource {

  #region --- Public properties ------------------------------------------------------------------------------
  public DateOnly DateAdded { get; set; } = DateOnly.FromDateTime(DateTime.Today);

  public DateOnly CreationDate { get; set; } = new DateOnly();
  public int CreationYear => CreationDate.Year;

  public string Description { get; set; } = string.Empty;

  public IListWithPrincipal<ELanguage> Languages { get; set; } = new TListWithPrincipal<ELanguage>() { ELanguage.Unknown };
  #endregion --- Public properties ---------------------------------------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  protected AMediaSource() {
    Logger = GlobalSettings.LoggerPool.GetLogger<AMediaSource>();
  }
  protected AMediaSource(IMediaSource mediaSource) : this() {
    Languages = new TListWithPrincipal<ELanguage>(mediaSource.Languages);
    DateAdded = mediaSource.DateAdded;
    CreationDate = mediaSource.CreationDate;
    Description = mediaSource.Description;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public virtual string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendIndent($"- {nameof(DateAdded)} : {DateAdded}", indent);
    RetVal.AppendIndent($"- {nameof(CreationDate)} : {CreationDate}", indent);
    RetVal.AppendIndent($"- {nameof(CreationYear)} : {CreationYear}", indent);
    RetVal.AppendIndent($"- {nameof(Description)} : {Description.WithQuotes()}", indent);
    RetVal.AppendIndent($"- {nameof(Languages)}");
    RetVal.AppendIndent(Languages.ToString(indent), indent + 2);
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }

  public virtual string Dump() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"# {nameof(DateAdded)} : {DateAdded}");
    RetVal.AppendLine($"  - {nameof(CreationDate)} : {CreationDate}");
    RetVal.AppendLine($"  - {nameof(CreationYear)} : {CreationYear}");
    RetVal.AppendLine($"  - {nameof(Description)} : {Description.WithQuotes()}");
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------
}
