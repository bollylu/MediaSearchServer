namespace MediaSearch.Database;

public interface ITable : ITable<IRecord>, IDisposable {
  string Name { get; }
  IDatabase? Database { get; set; }
  ITableHeader? Header { get; }

  #region --- Converters -------------------------------------------------------------------------------------
  /// <summary>
  /// Extended version of ToString with an optional indent size
  /// </summary>
  /// <param name="indent">The number of spaces to indent content</param>
  /// <returns>A string representation of the object</returns>
  string ToString(int indent);
  #endregion --- Converters -------------------------------------------------------------------------------------

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
  /// <returns>A long >= 0</returns>
  long Count();

  /// <summary>
  /// Clear the data
  /// </summary>
  void Clear();
  #endregion --- Content information --------------------------------------------

}

public interface ITable<RECORD> where RECORD : class, IRecord {

  List<IIndex<RECORD>> Indexes { get; }

  #region --- Content management --------------------------------------------
  /// <summary>
  /// Add a media
  /// </summary>
  /// <param name="item">The media to add</param>
  void Add(RECORD item);

  /// <summary>
  /// Add a media when it is not yet in the list, otherwise update its properties (all but Id)
  /// </summary>
  /// <param name="item">The media to add or update</param>
  void AddOrUpdate(RECORD item);

  /// <summary>
  /// Update a media properties if it is in the list
  /// </summary>
  /// <param name="item">The media to add or update</param>
  void Update(RECORD item);

  /// <summary>
  /// Delete a media properties if it is in the list
  /// </summary>
  /// <param name="item">The media to add or update</param>
  void Delete(RECORD item);

  /// <summary>
  /// Get all records
  /// </summary>
  /// <returns>The complete list of media</returns>
  IEnumerable<RECORD> GetAll(int maxRecords = 0);

  /// <summary>
  /// Get filtered records
  /// </summary>
  /// <param name="filter">The filter for the data</param>
  /// <returns>The filtered list of IMedia</returns>
  IEnumerable<RECORD> GetFiltered(IFilter filter, int maxRecords = 0);

  /// <summary>
  /// Get a record
  /// </summary>
  /// <param name="id">The identifier of the record</param>
  /// <returns>The requested record or null if unavailable</returns>
  RECORD? Get(string id);
  #endregion --- Content management --------------------------------------------

  #region --- Content management async--------------------------------------------
  /// <summary>
  /// Add a media, asynchronously
  /// </summary>
  /// <param name="item">The media to add</param>
  /// <param name="token">A way to cancel action</param>
  Task AddAsync(RECORD item, CancellationToken token);

  /// <summary>
  /// Add a media when it is not yet in the list, otherwise update its properties (all but Id), asynchronously
  /// </summary>
  /// <param name="item">The media to add or update</param>
  /// <param name="token">A way to cancel action</param>
  Task AddOrUpdateAsync(RECORD item, CancellationToken token);

  /// <summary>
  /// Update a media properties if it is in the list, asynchronously
  /// </summary>
  /// <param name="item">The media to add or update</param>
  /// <param name="token">A way to cancel action</param>
  Task UpdateAsync(RECORD item, CancellationToken token);

  /// <summary>
  /// Clear the data
  /// </summary>
  Task ClearAsync(CancellationToken token);

  /// <summary>
  /// Delete a media properties if it is in the list, asynchronously
  /// </summary>
  /// <param name="item">The media to add or update</param>
  /// <param name="token">A way to cancel action</param>
  Task DeleteAsync(RECORD item, CancellationToken token);

  /// <summary>
  /// Get all the records, asynchronously
  /// </summary>
  /// <param name="token">A way to cancel action</param>
  /// <returns>A enumeration of the records, asynchronous</returns>
  IAsyncEnumerable<RECORD> GetAllAsync(CancellationToken token);

  /// <summary>
  /// Get all the records filtered, asynchronously
  /// </summary>
  /// <param name="token">A way to cancel action</param>
  /// <returns>A enumeration of the records, asynchronous</returns>
  IAsyncEnumerable<RECORD> GetFilteredAsync(TFilter filter, CancellationToken token);
  #endregion --- Content management async -------------------------------------------

}

