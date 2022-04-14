using System.Runtime.CompilerServices;

namespace MediaSearch.Models;

public interface IMediaSearchDatabasePersistent : IMediaSearchDatabase {

  #region --- I/O --------------------------------------------
  /// <summary>
  /// Open the access to the persistent storage
  /// </summary>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool Open();

  /// <summary>
  /// Flushes all data to the persistence layer
  /// </summary>
  /// <returns>An awaitable task</returns>
  void Close();

  /// <summary>
  /// Flushes all data to the persistence layer, asynchronously
  /// </summary>
  /// <param name="token">Token for optional cancellation</param>
  /// <returns>An awaitable task</returns>
  Task CloseAsync(CancellationToken token);

  /// <summary>
  /// Load the data (if the storage is available)
  /// </summary>
  /// <param name="token">A token to cancel operation</param>
  /// <returns>true when ok, false otherwise</returns>
  bool Load();

  /// <summary>
  /// Load the data (if the storage is available), asynchronously
  /// </summary>
  /// <param name="token">A token to cancel operation</param>
  /// <returns>true if ok, false otherwise</returns>
  Task<bool> LoadAsync(CancellationToken token);

  /// <summary>
  /// When set to true, any update is automatically replicated to storage
  /// </summary>
  bool AutoSave { get; }

  /// <summary>
  /// Commit all modifications to storage
  /// </summary>
  /// <param name="token">Token for optional cancellation</param>
  /// <returns>true if ok, false otherwise</returns>
  bool Save();
  
  /// <summary>
  /// Commit all modifications to storage, asynchronously
  /// </summary>
  /// <param name="token">Token for optional cancellation</param>
  /// <returns>true if ok, false otherwise</returns>
  Task<bool> SaveAsync(CancellationToken token);
  #endregion --- I/O --------------------------------------------

  #region --- Content management --------------------------------------------

  /// <summary>
  /// Add a media, asynchronously
  /// </summary>
  /// <param name="item">The media to add</param>
  /// <param name="token">A way to cancel action</param>
  Task AddAsync(IMedia item, CancellationToken token);

  /// <summary>
  /// Add a media when it is not yet in the list, otherwise update its properties (all but Id), asynchronously
  /// </summary>
  /// <param name="item">The media to add or update</param>
  /// <param name="token">A way to cancel action</param>
  Task AddOrUpdateAsync(IMedia item, CancellationToken token);

  /// <summary>
  /// Update a media properties if it is in the list, asynchronously
  /// </summary>
  /// <param name="item">The media to add or update</param>
  /// <param name="token">A way to cancel action</param>
  Task UpdateAsync(IMedia item, CancellationToken token);

  /// <summary>
  /// Clear the data
  /// </summary>
  Task ClearAsync(CancellationToken token);

  /// <summary>
  /// Delete a media properties if it is in the list, asynchronously
  /// </summary>
  /// <param name="item">The media to add or update</param>
  /// <param name="token">A way to cancel action</param>
  Task DeleteAsync(IMedia item, CancellationToken token);

  #endregion --- Content management --------------------------------------------

  IAsyncEnumerable<IMedia> GetAllAsync(CancellationToken token);
  IAsyncEnumerable<IMedia> GetFilteredAsync(TFilter filter, CancellationToken token);
}
