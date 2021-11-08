using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BLTools.Diagnostic.Logging;

using MovieSearchModels;

namespace MovieSearchServerServices.MovieService {
  public interface IMovieCache : ILoggable {

    /// <summary>
    /// Path to the storage
    /// </summary>
    string Storage { get; }

    /// <summary>
    /// Name of the source
    /// </summary>
    string StorageName { get; }

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
    IEnumerable<IMovie> GetMovies(int startPage = 1, int pageSize = 20);

    /// <summary>
    /// Get a page of movie matching a filter
    /// </summary>
    /// <param name="filter">The data to check in name and alt. names</param>
    /// <param name="startPage">Which page to start with</param>
    /// <param name="pageSize">How many movies on a page</param>
    /// <returns>A list of IMovie</returns>
    IEnumerable<IMovie> GetMovies(string filter, int startPage = 1, int pageSize = 20);

    /// <summary>
    /// Get the list of movies with a valid group name
    /// </summary>
    IEnumerable<IMovie> GetMoviesWithGroup();

    /// <summary>
    /// Get the list of movie that belong to the group name
    /// </summary>
    /// <param name="groupName">The name of the group</param>
    /// <returns>A readonly list of movies</returns>
    IReadOnlyList<IMovie> GetMoviesInGroup(string groupName);

    /// <summary>
    /// Get the list of movies that doesn't belong to the group name
    /// </summary>
    /// <param name="groupName">The name of the group</param>
    /// <returns>A readonly list of movies</returns>
    IReadOnlyList<IMovie> GetMoviesNotInGroup(string groupName);

    /// <summary>
    /// Get the list of movies that belong to the group, while matching the filter
    /// </summary>
    /// <param name="groupName">The name of the group</param>
    /// <param name="filter">The string that needs to be contained in the name of the movie</param>
    /// <returns>A readonly list of movies</returns>
    Task<IReadOnlyList<IMovie>> GetMoviesForGroupAndFilter(string groupName, string filter);

    /// <summary>
    /// Get the list of movies that belong to the group, while matching a filter. The number of records are limited to <paramref name="pageSize"/> and the search start at <paramref name="startPage"/>
    /// </summary>
    /// <param name="groupName">The name of the group</param>
    /// <param name="filter">The string that needs to be contained in the name of the movie</param>
    /// <param name="startPage">The start page (it is a multiple of pageSize) to get the records</param>
    /// <param name="pageSize">How many records do we request</param>
    /// <returns>One IMovies containg name, start page, page size and available pages</returns>
    Task<IMovies> GetMoviesForGroupAndFilterInPages(string groupName, string filter, int startPage, int pageSize);
    #endregion --- Movies access --------------------------------------------

    #region --- Groups access --------------------------------------------
    /// <summary>
    /// Get the list of groups under a groupName, matching <see langword="abstract"/>filter
    /// </summary>
    /// <param name="groupName">The name of master group</param>
    /// <param name="filter">The string that needs to be contained in the name of the group</param>
    /// <returns><A readonly list of IMovieGroup/returns>
    Task<IReadOnlyList<IMovieGroup>> GetGroupsByGroupAndFilter(string groupName, string filter);
    #endregion --- Groups access --------------------------------------------
  }
}
