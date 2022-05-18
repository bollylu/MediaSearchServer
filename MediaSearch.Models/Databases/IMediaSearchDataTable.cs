﻿namespace MediaSearch.Models;

public interface IMediaSearchDataTable : IDisposable, IAsyncDisposable {

  #region --- Converters -------------------------------------------------------------------------------------
  /// <summary>
  /// Extended version of ToString with an optional indent size
  /// </summary>
  /// <param name="indent">The number of spaces to indent content</param>
  /// <returns>A string representation of the object</returns>
  string ToString(int indent);
  #endregion --- Converters -------------------------------------------------------------------------------------

  #region --- I/O --------------------------------------------
  /// <summary>
  /// Determine if the database exists
  /// </summary>
  /// <returns>true if the database exists, false otherwise</returns>
  public bool Exists();

  /// <summary>
  /// Create a new database
  /// </summary>
  /// <returns>true if successful, false otherwise</returns>
  public bool Create();

  /// <summary>
  /// Remove the database
  /// </summary>
  public void Remove();

  /// <summary>
  /// Open the access to the persistent storage
  /// </summary>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool OpenOrCreate();

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
  bool AutoSave { get; set; }

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

  /// <summary>
  /// Commit all modifications to storage, asynchronously, with a CancellationToken.None
  /// </summary>
  /// <returns>true if ok, false otherwise</returns>
  Task<bool> SaveAsync();

  bool IsDirty { get; }
  #endregion --- I/O --------------------------------------------
}

public interface IMediaSearchDataTable<T>  where T : IMedia    {

  IMSTableHeader<T> Header { get; }

  #region --- Content information --------------------------------------------
  /// <summary>
  /// Indicate if there is data
  /// </summary>
  /// <returns>true if no data, false otherwise</returns>
  bool IsEmpty();

  /// <summary>
  /// Indicate if there is at least one item
  /// </summary>
  /// <returns>true if there is data, false otherwise</returns>
  bool Any();

  /// <summary>
  /// The count of data
  /// </summary>
  /// <returns>An integer >= 0</returns>
  int Count();
  #endregion --- Content information --------------------------------------------

  #region --- Content management --------------------------------------------
  /// <summary>
  /// Add a media
  /// </summary>
  /// <param name="item">The media to add</param>
  void Add(T item);
  
  /// <summary>
  /// Add a media when it is not yet in the list, otherwise update its properties (all but Id)
  /// </summary>
  /// <param name="item">The media to add or update</param>
  void AddOrUpdate(T item);

  /// <summary>
  /// Update a media properties if it is in the list
  /// </summary>
  /// <param name="item">The media to add or update</param>
  void Update(T item);

  /// <summary>
  /// Clear the data
  /// </summary>
  void Clear();

  /// <summary>
  /// Delete a media properties if it is in the list
  /// </summary>
  /// <param name="item">The media to add or update</param>
  void Delete(T item);

  /// <summary>
  /// Get all records
  /// </summary>
  /// <returns>The complete list of media</returns>
  IEnumerable<T> GetAll(int maxRecords = 0);

  /// <summary>
  /// Get filtered records
  /// </summary>
  /// <param name="filter">The filter for the data</param>
  /// <returns>The filtered list of IMedia</returns>
  IEnumerable<T> GetFiltered(IFilter filter, int maxRecords = 0);

  /// <summary>
  /// Get a record
  /// </summary>
  /// <param name="id">The identifier of the record</param>
  /// <returns>The requested record or null if unavailable</returns>
  T? Get(string id);
  #endregion --- Content management --------------------------------------------

  #region --- Content management async--------------------------------------------
  /// <summary>
  /// Add a media, asynchronously
  /// </summary>
  /// <param name="item">The media to add</param>
  /// <param name="token">A way to cancel action</param>
  Task AddAsync(T item, CancellationToken token);

  /// <summary>
  /// Add a media when it is not yet in the list, otherwise update its properties (all but Id), asynchronously
  /// </summary>
  /// <param name="item">The media to add or update</param>
  /// <param name="token">A way to cancel action</param>
  Task AddOrUpdateAsync(T item, CancellationToken token);

  /// <summary>
  /// Update a media properties if it is in the list, asynchronously
  /// </summary>
  /// <param name="item">The media to add or update</param>
  /// <param name="token">A way to cancel action</param>
  Task UpdateAsync(T item, CancellationToken token);

  /// <summary>
  /// Clear the data
  /// </summary>
  Task ClearAsync(CancellationToken token);

  /// <summary>
  /// Delete a media properties if it is in the list, asynchronously
  /// </summary>
  /// <param name="item">The media to add or update</param>
  /// <param name="token">A way to cancel action</param>
  Task DeleteAsync(T item, CancellationToken token);

  /// <summary>
  /// Get all the records, asynchronously
  /// </summary>
  /// <param name="token">A way to cancel action</param>
  /// <returns>A enumeration of the records, asynchronous</returns>
  IAsyncEnumerable<T> GetAllAsync(CancellationToken token);

  /// <summary>
  /// Get all the records filtered, asynchronously
  /// </summary>
  /// <param name="token">A way to cancel action</param>
  /// <returns>A enumeration of the records, asynchronous</returns>
  IAsyncEnumerable<T> GetFilteredAsync(TFilter filter, CancellationToken token);
  #endregion --- Content management async -------------------------------------------

}