namespace MediaSearch.Models;

public interface IMediaSearchDatabase : IName, IAsyncDisposable {

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
  /// <returns>An integer >= 0</returns>
  int Count();
  #endregion --- Content information --------------------------------------------

  #region --- Content management --------------------------------------------

  /// <summary>
  /// Add a media
  /// </summary>
  /// <param name="item">The media to add</param>
  void Add(IMedia item);
  
  /// <summary>
  /// Add a media when it is not yet in the list, otherwise update its properties (all but Id)
  /// </summary>
  /// <param name="item">The media to add or update</param>
  void AddOrUpdate(IMedia item);

  /// <summary>
  /// Update a media properties if it is in the list
  /// </summary>
  /// <param name="item">The media to add or update</param>
  void Update(IMedia item);

  /// <summary>
  /// Clear the data
  /// </summary>
  void Clear();

  /// <summary>
  /// Delete a media properties if it is in the list
  /// </summary>
  /// <param name="item">The media to add or update</param>
  void Delete(IMedia item);

  /// <summary>
  /// Get all records
  /// </summary>
  /// <returns>The complete list of media</returns>
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
  /// <param name="id">The identifier of the record</param>
  /// <returns>The requested record or null if unavailable</returns>
  IMedia? Get(string id);

  #endregion --- Content management --------------------------------------------

}
