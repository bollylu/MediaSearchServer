using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MovieSearchModels;

namespace MovieSearchServerServices.MovieService {
  public interface IMovieService {

    public const int DEFAULT_PAGE_SIZE = 20;

    public const int DEFAULT_PICTURE_WIDTH = 128;
    public const int DEFAULT_PICTURE_HEIGHT = 160;

    public const string DEFAULT_PICTURE_NAME = "folder.jpg";

    string Storage { get; }

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
    /// The quantity of movies in the cache, optionally matching the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>The quantity of movies in the cache/></returns>
    ValueTask<int> MoviesCount(string filter = "");

    /// <summary>
    /// The number of pages given a specific page size (the last page can be incomplete)
    /// </summary>
    /// <param name="pageSize">The quantity of movies in a page (must be >= 1)</param>
    /// <returns>The number of pages including the last one</returns>
    ValueTask<int> PagesCount(int pageSize = DEFAULT_PAGE_SIZE);

    /// <summary>
    /// The number of pages given a specific page size (the last page can be incomplete)
    /// </summary>
    /// <param name="filter">The data to check in name and alt. names</param>
    /// <param name="pageSize">The quantity of movies in a page (must be >= 1)</param>
    /// <returns>The number of pages including the last one</returns>
    ValueTask<int> PagesCount(string filter, int pageSize = DEFAULT_PAGE_SIZE);

    /// <summary>
    /// Get all the movies
    /// </summary>
    /// <returns>The complete list of movies in cache</returns>
    IAsyncEnumerable<TMovie> GetAllMovies();

    /// <summary>
    /// Get a page of movies
    /// </summary>
    /// <param name="startPage">Which page to start with</param>
    /// <param name="pageSize">How many movies on a page</param>
    /// <returns>A list of IMovie</returns>
    IAsyncEnumerable<TMovie> GetMovies(int startPage = 1, int pageSize = DEFAULT_PAGE_SIZE);

    /// <summary>
    /// Get a page of movies matching a filter
    /// </summary>
    /// <param name="filter">The data to check in name and alt. names</param>
    /// <param name="startPage">Which page to start with</param>
    /// <param name="pageSize">How many movies on a page</param>
    /// <returns>A list of IMovie</returns>
    IAsyncEnumerable<TMovie> GetMovies(string filter, int startPage = 1, int pageSize = DEFAULT_PAGE_SIZE);

    ///// <summary>
    ///// Get a page of movies matching a group and a filter
    ///// </summary>
    ///// <param name="group">The group to match (empty means all)</param>
    ///// <param name="filter">The filter to match (empty means all)</param>
    ///// <param name="startPage">Which page to start with</param>
    ///// <param name="pageSize">How many movies on a page</param>
    ///// <returns>A list of IMovie</returns>
    //IAsyncEnumerable<TMovie> GetMovies(string group, string filter, int startPage = 1, int pageSize = DEFAULT_PAGE_SIZE);

    #endregion --- Movies --------------------------------------------

    Task<byte[]> GetPicture(string picturePath,
                            string pictureName = DEFAULT_PICTURE_NAME,
                            int width = DEFAULT_PICTURE_WIDTH,
                            int height = DEFAULT_PICTURE_HEIGHT);



  }
}
