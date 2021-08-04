using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MovieSearch.Models;

namespace MovieSearchServerServices.MovieService {
  public interface IMovieCache {

    public string SourceName { get; }

    /// <summary>
    /// Load the data into the cache
    /// </summary>
    /// <returns>A background task</returns>
    public Task Load();

    /// <summary>
    /// Clear the cache
    /// </summary>
    public void Clear();

    /// <summary>
    /// Indicate if the cache is empty
    /// </summary>
    /// <returns>true if the cache is empty, false otherwise</returns>
    public bool IsEmpty();

    /// <summary>
    /// Get the list of movie that belong to the group name
    /// </summary>
    /// <param name="groupName">The name of the group</param>
    /// <returns>A readonly list of movies</returns>
    public IReadOnlyList<IMovie> GetMoviesInGroup(string groupName);

    /// <summary>
    /// Get the list of movies that doesn't belong to the group name
    /// </summary>
    /// <param name="groupName">The name of the group</param>
    /// <returns>A readonly list of movies</returns>
    public IReadOnlyList<IMovie> GetMoviesNotInGroup(string groupName);

    /// <summary>
    /// Get the list of movies that belong to the group, while matching the filter
    /// </summary>
    /// <param name="groupName">The name of the group</param>
    /// <param name="filter">The string that needs to be contained in the name of the movie</param>
    /// <returns>A readonly list of movies</returns>
    public Task<IReadOnlyList<IMovie>> GetMoviesForGroupAndFilter(string groupName, string filter);

    /// <summary>
    /// Get the list of movies that belong to the group, while matching a filter. The number of records are limited to <paramref name="pageSize"/> and the search start at <paramref name="startPage"/>
    /// </summary>
    /// <param name="groupName">The name of the group</param>
    /// <param name="filter">The string that needs to be contained in the name of the movie</param>
    /// <param name="startPage">The start page (it is a multiple of pageSize) to get the records</param>
    /// <param name="pageSize">How many records do we request</param>
    /// <returns>One IMovies containg name, start page, page size and available pages</returns>
    public Task<IMovies> GetMoviesForGroupAndFilterInPages(string groupName, string filter, int startPage, int pageSize);

    /// <summary>
    /// Get the list of groups under a groupName, matching <see langword="abstract"/>filter
    /// </summary>
    /// <param name="groupName">The name of master group</param>
    /// <param name="filter">The string that needs to be contained in the name of the group</param>
    /// <returns><A readonly list of IMovieGroup/returns>
    public Task<IReadOnlyList<IMovieGroup>> GetGroupsByGroupAndFilter(string groupName, string filter);
  }
}
