namespace MediaSearch.Server.Services.Test;

[TestClass]
public static class Global {

  public static IMovieService MovieService = new TMovieService();
  public static TMediaSearchDatabaseJson? Database;
  public const string STORAGE = @"\\andromeda.sharenet.priv\films";

}
