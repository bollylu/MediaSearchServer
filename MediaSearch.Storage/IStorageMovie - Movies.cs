namespace MediaSearch.Storage;

public partial interface IStorageMovie : IStorage {

  #region --- Movies --------------------------------------------
  /// <summary>
  /// Get one movie from the storage, based on its Id
  /// </summary>
  /// <param name="movieId">The id of the requested movie</param>
  /// <returns>The requested movie or (null) if not available</returns>
  Task<IMovie?> GetMovieAsync(IRecord movieId);

  /// <summary>
  /// Get all the movies from storage
  /// </summary>
  /// <returns>An enumeration of the movies</returns>
  IAsyncEnumerable<IMovie> GetAllMoviesAsync();

  /// <summary>
  /// Get one page of movies for a given filter
  /// </summary>
  /// <param name="filter">The filter to apply (includes also the size of the page)</param>
  /// <returns>A page of movies (0->n) or (null) if something goes wrong during operation</returns>
  Task<IMoviesPage?> GetMoviesPageAsync(IFilter filter);

  /// <summary>
  /// Get the last page of movies for a given filter and its sort order
  /// </summary>
  /// <param name="filter">The filter to apply (includes also the size of the page)</param>
  /// <returns>A page of movies (0->n) or (null) if something goes wrong during operation</returns>
  Task<IMoviesPage?> GetMoviesLastPageAsync(IFilter filter);

  /// <summary>
  /// Calculate the number of movies
  /// </summary>
  /// <returns>The number of movies related to the filter</returns>
  ValueTask<int> MoviesCount();

  /// <summary>
  /// Calculate the number of movies for a given filter
  /// </summary>
  /// <param name="filter">The filter to apply</param>
  /// <returns>The number of movies related to the filter</returns>
  ValueTask<int> MoviesCount(IFilter filter);

  /// <summary>
  /// Calculate the number of pages for a given filter
  /// </summary>
  /// <param name="filter">The filter to apply (includes also the size of a page)</param>
  /// <returns>The number of pages related to the filter</returns>
  ValueTask<int> PagesCount(IFilter filter);

  /// <summary>
  /// Add one movie to the storage. Its Id must be unique.
  /// </summary>
  /// <param name="movie">The movie to add, with a unique Id</param>
  /// <returns><see langword="true"/> if successful, <see langword="false"/> otherwise</returns>
  ValueTask<bool> AddMovieAsync(IMovie movie);

  ///// <summary>
  ///// Remove one movie from storage
  ///// </summary>
  ///// <param name="movie">The movie to remove (selection will be based on Id)</param>
  ///// <returns><see langword="true"/> if successful, <see langword="false"/> otherwise</returns>
  //ValueTask<bool> RemoveMovieAsync(IMovie movie);

  /// <summary>
  /// Remove one movie and associated records from storage
  /// </summary>
  /// <param name="id">The Id of the movie to remove</param>
  /// <returns><see langword="true"/> if successful, <see langword="false"/> otherwise</returns>
  ValueTask<bool> RemoveMovieAsync(IRecord id);

  /// <summary>
  /// Removes all movies and associated records from storage
  /// </summary>
  /// <returns><see langword="true"/> if successful, <see langword="false"/> otherwise</returns>
  ValueTask<bool> RemoveAllMoviesAsync();
  #endregion --- Movies --------------------------------------------

  #region --- Groups --------------------------------------------
  Task<IGroup?> GetGroupsAsync(IRecord id);
  IAsyncEnumerable<IGroup> GetGroupsListAsync(IRecord id);
  #endregion --- Groups --------------------------------------------
}
