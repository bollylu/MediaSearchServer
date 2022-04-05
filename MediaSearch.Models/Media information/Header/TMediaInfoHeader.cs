namespace MediaSearch.Models;
public class TMediaInfoHeader : IMediaInfoHeader, IJson<TMediaInfoHeader> {
  public string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    string IndentSpace = new string (' ', indent);
    RetVal.AppendLine($"{IndentSpace}{Name.WithQuotes()}");
    RetVal.AppendLine($"{IndentSpace}{Description.WithQuotes()}");
    return RetVal.ToString();
  }


  public override string ToString() {
    return ToString(0);
  }

  public string Name { get; set; } = "";
  public string Description { get; set; } = "";
}
