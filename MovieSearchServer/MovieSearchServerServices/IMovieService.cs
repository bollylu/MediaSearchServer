using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MovieSearch.Models;

namespace MovieSearchServerServices.MovieService {
  public interface IMovieService {

    const int DEFAULT_PAGE_SIZE = 20;

    string RootPath { get; }

    #region --- Movies --------------------------------------------
    List<string> MoviesExtensions { get; }

    Task Initialize();

    /// <summary>
    /// The quantity of movies in the cache, optionally matching the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    int MoviesCount(string filter = "");

    /// <summary>
    /// The number of pages given a specific page size (the last page can be incomplete)
    /// </summary>
    /// <param name="pageSize">The quantity of movies in a page (must be >= 1)</param>
    /// <returns>The number of pages including the last one</returns>
    int PagesCount(int pageSize = DEFAULT_PAGE_SIZE);

    /// <summary>
    /// The number of pages given a specific page size (the last page can be incomplete)
    /// </summary>
    /// <param name="filter">The data to check in name and alt. names</param>
    /// <param name="pageSize">The quantity of movies in a page (must be >= 1)</param>
    /// <returns>The number of pages including the last one</returns>
    int PagesCount(string filter, int pageSize = DEFAULT_PAGE_SIZE);

    /// <summary>
    /// Get all the movies
    /// </summary>
    /// <returns>The complete list of movies in cache</returns>
    IAsyncEnumerable<IMovie> GetAllMovies();

    /// <summary>
    /// Get a page of movies
    /// </summary>
    /// <param name="startPage">Which page to start with</param>
    /// <param name="pageSize">How many movies on a page</param>
    /// <returns>A list of IMovie</returns>
    IAsyncEnumerable<IMovie> GetMovies(int startPage = 1, int pageSize = DEFAULT_PAGE_SIZE);

    /// <summary>
    /// Get a page of movies matching a filter
    /// </summary>
    /// <param name="filter">The data to check in name and alt. names</param>
    /// <param name="startPage">Which page to start with</param>
    /// <param name="pageSize">How many movies on a page</param>
    /// <returns>A list of IMovie</returns>
    IAsyncEnumerable<IMovie> GetMovies(string filter, int startPage = 1, int pageSize = DEFAULT_PAGE_SIZE);

    /// <summary>
    /// Get a page of movies matching a group and a filter
    /// </summary>
    /// <param name="group">The group to match (empty means all)</param>
    /// <param name="filter">The filter to match (empty means all)</param>
    /// <param name="startPage">Which page to start with</param>
    /// <param name="pageSize">How many movies on a page</param>
    /// <returns>A list of IMovie</returns>
    IAsyncEnumerable<IMovie> GetMovies(string group, string filter, int startPage = 1, int pageSize = DEFAULT_PAGE_SIZE);

    Task<byte[]> GetPicture(string pathname, int timeout = 5000);
    Task<string> GetPicture64(string pathname, int timeout = 5000);
    #endregion --- Movies --------------------------------------------

    //#region --- Groups --------------------------------------------
    //string CurrentGroup { get; }
    //Task<IMovieGroups> GetGroups(string group = "", string filter = "");
    //#endregion --- Groups --------------------------------------------



  }
}
