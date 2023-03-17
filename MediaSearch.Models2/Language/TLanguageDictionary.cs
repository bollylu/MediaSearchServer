namespace MediaSearch.Models.Language;

public class TLanguageDictionary<T> : Dictionary<ELanguage, T>, ILanguageDictionary<T> {

  public string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    string IndentSpace = new string(' ', indent);
    foreach (KeyValuePair<ELanguage, T> kvp in this) {
      RetVal.Append(IndentSpace);
      RetVal.Append(kvp.Key.ToString());
      RetVal.Append(" : ");
      RetVal.AppendLine((kvp.Value?.ToString() ?? "(null)").WithQuotes());
    }
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }

  public static TLanguageDictionary<T> Empty {
    get {
      return _Empty ??= new TLanguageDictionary<T>();
    }
  }
  private static TLanguageDictionary<T>? _Empty;
}
