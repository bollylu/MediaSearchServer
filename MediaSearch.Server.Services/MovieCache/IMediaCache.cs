namespace MediaSearch.Server.Services;

public interface IMediaCache : IName {

  /// <summary>
  /// Path to the storage
  /// </summary>
  string RootStoragePath { get; }

  #region --- Cache management --------------------------------------------
  /// <summary>
  /// Add a media to the cache
  /// </summary>
  /// <param name="media"></param>
  void AddMedia(IMedia media);

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
  IEnumerable<IMedia> GetAll();

  /// <summary>
  /// Get a page of movie matching a filter
  /// </summary>
  /// <param name="filter">The data to check against</param>
  /// <returns>A list of IMovie</returns>
  IMediasPage? GetPage(IFilter filter);

  /// <summary>
  /// Get a movie from the cache
  /// </summary>
  /// <param name="id">The id of the movie</param>
  /// <returns>The requested movie or null if error</returns>
  IMedia? Get(string id);
  #endregion --- Movies access --------------------------------------------

  IAsyncEnumerable<string> GetGroups();
  //IEnumerable<string> GetSubGroups(string group);
}
