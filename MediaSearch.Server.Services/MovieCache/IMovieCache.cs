namespace MediaSearch.Server.Services;

public interface IMovieCache : ILoggable, IName {

  /// <summary>
  /// Path to the storage
  /// </summary>
  string RootStoragePath { get; }

  #region --- Cache I/O --------------------------------------------
  /// <summary>
  /// Fetch the files from storage
  /// </summary>
  /// <returns>An IEnumerable of FileInfo</returns>
  IEnumerable<IFileInfo> FetchFiles();

  /// <summary>
  /// Fetch the files from storage
  /// </summary>
  /// <param name="token">A token to cancel operation</param>
  /// <returns>An IEnumerable of FileInfo</returns>
  IEnumerable<IFileInfo> FetchFiles(CancellationToken token);

  /// <summary>
  /// Load the data into the cache (if the storage is available)
  /// </summary>
  /// <param name="fileSource">The list of files to process</param>
  /// <param name="token">A token to cancel operation</param>
  /// <returns>A background task</returns>
  Task Parse(IEnumerable<IFileInfo> fileSource, CancellationToken token);
  #endregion --- Cache I/O --------------------------------------------

  #region --- Cache management --------------------------------------------
  /// <summary>
  /// Clear the cache
  /// </summary>
  void Clear();

  /// <summary>
  /// Indicate if the cache is empty
  /// </summary>
  /// <returns>true if the cache is empty, false otherwise</returns>
  bool IsEmpty();

  /// <summary>
  /// Indicate if the cache has at least on item
  /// </summary>
  /// <returns>true if the cache is not empty, false otherwise</returns>
  bool Any();

  /// <summary>
  /// The count of movies in cache
  /// </summary>
  /// <returns>An integer >= 0</returns>
  int Count();
  #endregion --- Cache management --------------------------------------------

  #region --- Movies access --------------------------------------------
  /// <summary>
  /// Get all the movies in cache
  /// </summary>
  /// <returns>The complete list of movies in cache</returns>
  IEnumerable<IMovie> GetAllMovies();

  /// <summary>
  /// Get a page of movies
  /// </summary>
  /// <param name="startPage">Which page to start with</param>
  /// <param name="pageSize">How many movies on a page</param>
  /// <returns>A list of IMovie</returns>
  IMoviesPage GetMoviesPage(int startPage = 1, int pageSize = 20);

  /// <summary>
  /// Get a page of movie matching a filter
  /// </summary>
  /// <param name="filter">The data to check against</param>
  /// <param name="startPage">Which page to start with</param>
  /// <param name="pageSize">How many movies on a page</param>
  /// <returns>A list of IMovie</returns>
  IMoviesPage GetMoviesPage(TFilter filter, int startPage = 1, int pageSize = 20);

  /// <summary>
  /// Get a movie from the cache
  /// </summary>
  /// <param name="id">The id of the movie</param>
  /// <returns>The requested movie or null if error</returns>
  IMovie GetMovie(string id);
  #endregion --- Movies access --------------------------------------------

}
