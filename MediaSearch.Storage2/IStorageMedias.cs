namespace MediaSearch.Storage;

public interface IStorageMedias : IStorage {

  /// <summary>
  /// Get one media from the storage, based on its Id
  /// </summary>
  /// <param name="mediaId">The id of the requested movie</param>
  /// <returns>The requested movie or (null) if not available</returns>
  Task<IMedia?> GetMediaAsync(IRecord mediaId);

  /// <summary>
  /// Get all the medias from storage
  /// </summary>
  /// <returns>An enumeration of the medias</returns>
  IAsyncEnumerable<IMedia> GetMediasAsync();

  /// <summary>
  /// Get one page of medias for a given filter
  /// </summary>
  /// <param name="filter">The filter to apply (includes also the size of the page)</param>
  /// <returns>A page of medias (0->n) or (null) if something goes wrong during operation</returns>
  Task<IMediasPage?> GetMediasPageAsync(IFilter filter);

  /// <summary>
  /// Get the last page of movies for a given filter and its sort order
  /// </summary>
  /// <param name="filter">The filter to apply (includes also the size of the page)</param>
  /// <returns>A page of medias (0->n) or (null) if something goes wrong during operation</returns>
  Task<IMediasPage?> GetMediasLastPageAsync(IFilter filter);

  /// <summary>
  /// Calculate the number of medias
  /// </summary>
  /// <returns>The number of medias related</returns>
  ValueTask<int> MediasCount();

  /// <summary>
  /// Calculate the number of medias for a given filter
  /// </summary>
  /// <param name="filter">The filter to apply</param>
  /// <returns>The number of movies related to the filter</returns>
  ValueTask<int> MediasCount(IFilter filter);

  /// <summary>
  /// Calculate the number of pages for a given filter
  /// </summary>
  /// <param name="filter">The filter to apply (includes also the size of a page)</param>
  /// <returns>The number of pages related to the filter</returns>
  ValueTask<int> PagesCount(IFilter filter);

  /// <summary>
  /// Add one media to the storage. Its Id must be unique.
  /// </summary>
  /// <param name="media">The movie to add, with a unique Id</param>
  /// <returns><see langword="true"/> if successful, <see langword="false"/> otherwise</returns>
  ValueTask<bool> AddMediaAsync(IMedia media);

  ///// <summary>
  ///// Remove one media from storage
  ///// </summary>
  ///// <param name="media">The media to remove (selection will be based on Id)</param>
  ///// <returns><see langword="true"/> if successful, <see langword="false"/> otherwise</returns>
  //ValueTask<bool> RemoveMediaAsync(IMedia media);

  /// <summary>
  /// Remove one movie and associated records from storage
  /// </summary>
  /// <param name="id">The Id of the movie to remove</param>
  /// <returns><see langword="true"/> if successful, <see langword="false"/> otherwise</returns>
  ValueTask<bool> RemoveMediaAsync(IRecord id);

  /// <summary>
  /// Removes all medias and associated records from storage
  /// </summary>
  /// <returns><see langword="true"/> if successful, <see langword="false"/> otherwise</returns>
  ValueTask<bool> RemoveAllMediasAsync();

}
