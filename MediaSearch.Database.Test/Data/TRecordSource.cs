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

  public static IList<IRecord> GetMovieRecords() {
    List<IRecord> RetVal = new();
    RetVal.Add(GetMovieRecord("La grande vadrouille", "Une belle ballade", 1960));
    RetVal.Add(GetMovieRecord("Star Trek", "Exploration", 1980));
    RetVal.Add(GetMovieRecord("Star Wars", "Dark Vador & Co", 1981));
    return RetVal;
  }

}
