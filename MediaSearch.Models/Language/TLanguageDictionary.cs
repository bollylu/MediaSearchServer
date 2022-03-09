namespace MediaSearch.Models.Language;

public class TLanguageDictionary<T> : Dictionary<ELanguage, T>, ILanguageDictionary<T> {

  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    foreach (KeyValuePair<ELanguage, T> kvp in this) {
      RetVal.Append(kvp.Key.ToString());
      RetVal.Append(" : ");
      RetVal.AppendLine((kvp.Value?.ToString() ?? "(null)").WithQuotes());
    }
    return RetVal.ToString();
  }

}
