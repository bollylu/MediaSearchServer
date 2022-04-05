namespace MediaSearch.Models;
public class TLanguageTextInfo : ILanguageTextInfo {

  public ELanguage Language { get; set; } = ELanguage.Unknown;

  public string Value { get; set; } = "";

  public bool IsPrincipal { get; set; } = false;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TLanguageTextInfo() { }
  public TLanguageTextInfo(ELanguage language, string value, bool isPrincipal = false) {
    Language = language;
    Value = value;
    IsPrincipal = isPrincipal;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override string ToString() {
    return ToString(0);
  }

  public string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    string IndentSpace = new string(' ', indent);
    RetVal.Append(IndentSpace);
    RetVal.Append(IsPrincipal);
    RetVal.Append(':');
    RetVal.Append(Language.ToString());
    RetVal.Append(':');
    RetVal.Append(Value.WithQuotes());
    
    return RetVal.ToString();
  }

}
