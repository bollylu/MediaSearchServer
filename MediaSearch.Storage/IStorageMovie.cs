namespace MediaSearch.Storage;

public interface IStorageMovie {

  Task<IMovie?> GetMovieAsync(string movieId);
  Task<byte[]?> GetMoviePictureAsync(string movieId, string pictureName, int width, int height);

  IAsyncEnumerable<IMovie> GetAllMoviesAsync();

  Task<IMoviesPage?> GetMoviesPageAsync(IFilter filter);
  Task<IMoviesPage?> GetMoviesLastPageAsync(IFilter filter);

  ValueTask<int> MoviesCount(IFilter filter);
  ValueTask<int> PagesCount(IFilter filter);

  ValueTask<bool> AddMovieAsync(IMovie movie);

  ValueTask<bool> RemoveMovieAsync(IMovie movie);

}
