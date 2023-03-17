namespace MediaSearch.Models;

public abstract class AMediaInfo : IMediaInfo {
  public ELanguage Language { get; init; }
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public List<string> Tags { get; } = new();


  #region --- Constructor(s) ---------------------------------------------------------------------------------
  protected AMediaInfo() { }
  protected AMediaInfo(IMediaInfo mediaInfo) {
    Language = mediaInfo.Language;
    Title = mediaInfo.Title;
    Description = mediaInfo.Description;
    Tags.AddRange(mediaInfo.Tags);
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public virtual string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendIndent($"{nameof(Language)} = {Language}", indent);
    RetVal.AppendIndent($"{nameof(Title)} = {Title}", indent + 2);
    RetVal.AppendIndent($"{nameof(Description)} = {Description}", indent + 2);
    RetVal.AppendIndent($"{nameof(Tags)} = {string.Join(" ", Tags.Select(t => $"[{t}]"))}", indent + 2);
    return RetVal.ToString();
  }


  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------
}
