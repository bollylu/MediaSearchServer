using MediaSearch.Storage;

namespace MediaSearch.Server.Services.Test;

[TestClass]
public static class Global {

  public static IMovieService MovieService = new TMovieService(new TStorageMemoryMovies(), new TMediaSourceMovie(STORAGE));
  public static IMovieCache MovieCache = new TMovieCache();

  public const string STORAGE = @"\\andromeda.sharenet.priv\films";

}
