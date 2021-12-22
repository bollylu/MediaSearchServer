namespace MovieSearchTest;

[TestClass]
public static class Global {

  public static IMovieService MovieService;
  public static IMovieCache MovieCache;

  public const string STORAGE = @"\\andromeda.sharenet.priv\films";

}
