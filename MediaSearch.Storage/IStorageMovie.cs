namespace MediaSearch.Storage;

public interface IStorageMovie {

  //IMovie GetMovie(string movieId);
  Task<IMovie?> GetMovieAsync(string movieId);
  Task<byte[]?> GetMoviePictureAsync(string movieId, string pictureName, int width, int height);

  //IEnumerable<IMovie> GetAllMovies();
  IAsyncEnumerable<IMovie> GetAllMoviesAsync();

  Task<IMoviesPage?> GetMoviesPageAsync(IFilter filter);
  Task<IMoviesPage?> GetMoviesLastPageAsync(IFilter filter);

  ValueTask<int> MoviesCount(IFilter filter);
  ValueTask<int> PagesCount(IFilter filter);

  //bool AddMovie(IMovie movie);
  ValueTask<bool> AddMovieAsync(TMovie movie);

  //bool RemoveMovie(IMovie movie);
  ValueTask<bool> RemoveMovieAsync(IMovie movie);

}
