namespace MediaSearch.Test.Database;

public static class TRecordSource {

  public static IRecord GetMovieRecord(string name, string description, int year) {
    IMovie RetVal = new TMovie(name, year);
    RetVal.Descriptions.Add(ELanguage.French, $"UK : {description}");
    RetVal.Descriptions.Add(ELanguage.English, $"UK : {description}");
    RetVal.Soundtracks.Add(ELanguage.FrenchFrance);
    RetVal.Soundtracks.Add(ELanguage.English);
    RetVal.Subtitles.Add(ELanguage.English);
    RetVal.Subtitles.Add(ELanguage.French);
    return RetVal;
  }

  public static IList<IRecord> GetMovieRecords(int recordCount = 1) {
    List<IRecord> RetVal = new();
    RetVal.Add(GetMovieRecord("La grande vadrouille", "Une belle ballade", 1960));
    RetVal.Add(GetMovieRecord("Star Trek", "Exploration", 1980));
    RetVal.Add(GetMovieRecord("Star Wars", "Dark Vador & Co", 1981));
    RetVal.Add(GetMovieRecord("Star Wars", "Dark Vador & Co, suite", 1982));
    RetVal.Add(GetMovieRecord("Star Wars", "Dark Vador & Co, le retour", 1983));
    RetVal.Add(GetMovieRecord("Stargate", "A new door to ...", 1996));
    return RetVal.Take(recordCount).ToList();
  }

  public static ITable<IMovie> WithRecords(this ITable<IMovie> source, int recordCount = 1) {
    foreach (IMovie RecordItem in GetMovieRecords(recordCount)) {
      source.Add(RecordItem);
    }
    return source;
  }
}
