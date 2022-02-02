namespace MediaSearch.Server.Services;

public interface IDataProvider : IName, ILoggable {

  string RootStoragePath { get; }

  /// <summary>
  /// Clear the data
  /// </summary>
  void Clear();

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

  /// <summary>
  /// Load the data (if the storage is available)
  /// </summary>
  /// <param name="token">A token to cancel operation</param>
  /// <returns>A background task</returns>
  Task Load(CancellationToken token);

  /// <summary>
  /// Get all records
  /// </summary>
  /// <returns>The complete list of IMedia</returns>
  IEnumerable<IMedia> GetAll();

  /// <summary>
  /// Get filtered records
  /// </summary>
  /// <param name="filter">The filter for the data</param>
  /// <returns>The filtered list of IMedia</returns>
  IEnumerable<IMedia> GetFiltered(TFilter filter);

  /// <summary>
  /// Get a record
  /// </summary>
  /// <returns>The complete list of movies in cache</returns>
  IMedia Get(string id);
}
