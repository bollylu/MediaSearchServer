using MediaSearch.Storage;

namespace MediaSearch.Server.Services.Test;

[TestClass]
public static class Global {

  public static IMediaService MovieService = new TMediaService(new TStorageMemoryMovies(), new TMediaSourceMovie(STORAGE));
  public static IMediaCache MovieCache = new TMovieCache();

  public const string STORAGE = @"\\andromeda.sharenet.priv\films";

}
