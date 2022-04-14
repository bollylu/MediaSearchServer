namespace MediaSearch.Models;
public class TMediaSearchDatabaseHeader : IMediaSearchDatabaseHeader {
  public string Name { get; set; } = "";
  public string Description { get; set; } = "";
  public DateTime LastUpdate { get; set; } = DateTime.MinValue;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSearchDatabaseHeader() { }
  public TMediaSearchDatabaseHeader(IMediaSearchDatabaseHeader header) {
    Name = header.Name;
    Description = header.Description;
    LastUpdate = DateTime.Now;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    string IndentSpace = new string(' ', indent);
    RetVal.AppendLine($"{IndentSpace}{nameof(Name)} : {Name.WithQuotes()}");
    RetVal.AppendLine($"{IndentSpace}{nameof(Description)} : {Description.WithQuotes()}");
    RetVal.AppendLine($"{IndentSpace}{nameof(LastUpdate)} : {LastUpdate.ToYMDHMS()}");
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  } 
  #endregion --- Converters ----------------------------------------------------------------------------------
}
