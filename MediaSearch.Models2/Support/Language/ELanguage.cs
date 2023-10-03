namespace MediaSearch.Models;

public enum ELanguage {
  Unknown,
  French,
  FrenchFrance,
  FrenchQuebec,
  FrenchBelgium,
  English,
  EnglishUs,
  EnglishUk,
  Chinese,
  ChineseCantonese,
  ChineseMandarin,
  Japanese,
  Korean,
  German,
  Dutch
}

public static class ELanguageConverter {

  private static readonly Dictionary<string, ELanguage> _MatchTableSL = new Dictionary<string, ELanguage>() {
    { "fre", ELanguage.French },
    { "eng", ELanguage.English }
  };

  private static readonly Dictionary<ELanguage, string> _MatchTableLS = new Dictionary<ELanguage, string>() {
    { ELanguage.French, "fre"  },
    { ELanguage.English, "eng"  }
  };

  public static ELanguage FromAudioStreamValue(string source) {
    if (_MatchTableSL.Keys.Contains(source)) {
      return _MatchTableSL[source];
    }
    return ELanguage.Unknown;
  }

  public static string ToAudioStreamValue(ELanguage source) {
    if (_MatchTableLS.Keys.Contains(source)) {
      return _MatchTableLS[source];
    }
    return "Unk";
  }
}
