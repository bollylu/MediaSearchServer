namespace MediaSearch.Server.Services;

public interface IMovieService {

  //public const int DEFAULT_PAGE_SIZE = 20;

  public const int MIN_PICTURE_WIDTH = 128;
  public const int MAX_PICTURE_WIDTH = 1024;
  public const int MIN_PICTURE_HEIGHT = 160;
  public const int MAX_PICTURE_HEIGHT = 1280;

  public const string DEFAULT_PICTURE_NAME = "folder.jpg";

  string RootStoragePath { get; }

  /// <summary>
  /// The source of the data for the service
  /// </summary>
  IMediaSearchDataTable Database { get; }

  #region --- Database I/O --------------------------------------------
  /// <summary>
  /// Load the data into the database (if the storage is available), asynchronously
  /// </summary>
  /// <param name="token">A token to cancel operation</param>
  /// <returns>A background task</returns>
  Task ParseAsync(CancellationToken token);

  /// <summary>
  /// Load the data into the database (if the storage is available)
  /// </summary>
  void Parse();
  #endregion --- Database I/O --------------------------------------------

  #region --- Movies --------------------------------------------
  List<string> MoviesExtensions { get; }

  /// <summary>
  /// Initialize the cache content by reading data from storage
  /// </summary>
  /// <returns>An awaitable task</returns>
  Task Initialize();

  /// <summary>
  /// Clear the cache content. Next call to initialize will read data.
  /// </summary>
  void Reset();

  /// <summary>
  /// Clear the cache content and call initialize will read data.
  /// </summary>
  Task RefreshData();

  /// <summary>
  /// Indicate the number of records already processed
  /// </summary>
  /// <returns>The number of processed records or -1 when completed</returns>
  int GetRefreshStatus();

  /// <summary>
  /// The quantity of movies in the cache, optionally matching the filter
  /// </summary>
  /// <param name="filter"></param>
  /// <returns>The quantity of movies in the cache/></returns>
  ValueTask<int> MoviesCount(IFilter filter);

  /// <summary>
  /// The number of pages given a specific page size (the last page can be incomplete)
  /// </summary>
  /// <param name="filter">The data to check</param>
  /// <param name="pageSize">The quantity of movies in a page (must be >= 1)</param>
  /// <returns>The number of pages including the last one</returns>
  ValueTask<int> PagesCount(IFilter filter);

  /// <summary>
  /// Get all the movies
  /// </summary>
  /// <returns>The complete list of movies in cache</returns>
  IAsyncEnumerable<TMovie> GetAllMovies();

  /// <summary>
  /// Get a page of movies matching a filter
  /// </summary>
  /// <param name="filter">The data to check in name and alt. names</param>
  /// <returns>A page of IMovie</returns>
  Task<TMoviesPage> GetMoviesPage(IFilter filter);

  // <summary>
  /// Get a page of movies matching a filter
  /// </summary>
  /// <param name="filter">The data to check in name and alt. names</param>
  /// <param name="startPage">Which page to start with</param>
  /// <param name="pageSize">How many movies on a page</param>
  /// <returns>A page of IMovie</returns>
  Task<TMoviesPage> GetMoviesLastPage(IFilter filter);

  /// <summary>
  /// Get a movie based on it's Id
  /// </summary>
  /// <param name="id">The Id in memory for the requeted movie</param>
  /// <returns>An movie or null is not found</returns>
  Task<IMovie?> GetMovie(string id);
  #endregion --- Movies --------------------------------------------

  IAsyncEnumerable<string> GetGroups();

  Task<byte[]> GetPicture(string id,
                          string pictureName,
                          int width,
                          int height);

}
