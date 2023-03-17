namespace MediaSearch.Models;

public abstract class AMediaInfo : IMediaInfo {
  public ELanguage Language { get; }
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public List<string> Tags { get; } = new();


  public string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendIndent($"{nameof(Language)} = {Language}", indent);
    RetVal.AppendIndent($"{nameof(Title)} = {Title}", indent);
    RetVal.AppendIndent($"{nameof(Description)} = {Description}", indent);
    RetVal.AppendIndent($"{nameof(Tags)} = {string.Join(", ", Tags)}", indent);
    return RetVal.ToString();
  }


  public override string ToString() {
    return ToString(0);
  }
}
