namespace MediaSearch.Models;

public abstract class AMediaInfo : IMediaInfo {
  public ELanguage Language { get; init; }
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public List<string> Tags { get; init; } = new();

  public string Group { get; set; } = string.Empty;
  public bool IsGroupMember => Group != string.Empty;
  public DateOnly CreationDate { get; set; } = DateOnly.MinValue;
  public int CreationYear => CreationDate == DateOnly.MinValue ? 0 : CreationDate.Year;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  protected AMediaInfo() {
    Language = AMedia.DEFAULT_LANGUAGE;
  }
  protected AMediaInfo(IMediaInfo mediaInfo) {
    Language = mediaInfo.Language;
    Title = mediaInfo.Title;
    Description = mediaInfo.Description;
    Tags.AddRange(mediaInfo.Tags);
    Group = mediaInfo.Group;
    CreationDate = mediaInfo.CreationDate;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public virtual string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendIndent($"{nameof(Language)} = {Language}", indent);
    RetVal.AppendIndent($"- {nameof(Title)} = {Title.WithQuotes()}", indent + 2);
    RetVal.AppendIndent($"- {nameof(Description)} = {Description.WithQuotes()}", indent + 2);
    RetVal.AppendIndent($"- {nameof(CreationDate)} = {CreationDate}", indent + 2);
    RetVal.AppendIndent($"- {nameof(CreationYear)} = {CreationYear}", indent + 2);

    if (IsGroupMember) {
      RetVal.AppendIndent($"- {nameof(Group)} = {Group.WithQuotes()}", indent + 2);
    } else {
      RetVal.AppendIndent("- No group membership");
    }

    if (Tags.Any()) {
      RetVal.AppendIndent($"- {nameof(Tags)} = {string.Join(" ", Tags.Select(t => $"[{t}]"))}", indent + 2);
    } else {
      RetVal.AppendIndent("- Tags is empty", indent + 2);
    }

    return RetVal.ToString();
  }


  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------
}
