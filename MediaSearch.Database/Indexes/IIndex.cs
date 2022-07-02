namespace MediaSearch.Database;

public interface IIndex {
  /// <summary>
  /// Transforms the index to a string, with indentation
  /// </summary>
  /// <param name="indent">The current indentation</param>
  /// <returns>A string representing the table</returns>
  string ToString(int indent);
}

public interface IIndex<RECORD> : IIndex
  where RECORD : IID<string> {

}

public interface IIndex<RECORD, INDEX_KEY, INDEX_VALUE> : IIndex<RECORD>
  where RECORD : IID<string>
  where INDEX_KEY : notnull
  where INDEX_VALUE : notnull {

  /// <summary>
  /// When is the last update to any item in this table
  /// </summary>
  public DateTime LastUpdate { get; set; }

  /// <summary>
  /// Table can contain only only data type
  /// </summary>
  public Type IndexType { get; }

  /// <summary>
  /// The index of the records of the table. Key = index key, value = record id.
  /// </summary>
  List<KeyValuePair<INDEX_KEY, INDEX_VALUE>> IndexedValues { get; }

  /// <summary>
  /// Add a new key to the index
  /// </summary>
  /// <param name="key">The indexed key</param>
  /// <param name="value">The record id</param>
  void Add(INDEX_KEY key, INDEX_VALUE id);

  /// <summary>
  /// Add a new key to the index
  /// </summary>
  /// <param name="kvp">The key value pair (indexed key, record id)</param>
  void Add(KeyValuePair<INDEX_KEY, INDEX_VALUE> kvp);

  /// <summary>
  /// Remove an indexed value from the index
  /// </summary>
  /// <param name="key"></param>
  void Delete(INDEX_KEY key);

  /// <summary>
  /// Remove all indexed values from the index
  /// </summary>
  void Clear();

  /// <summary>
  /// Get the ID related to a key
  /// </summary>
  /// <param name="key">The key to search for</param>
  /// <param name="defaultValue">The default value to return when key is not found</param>
  /// <returns>The ID associated with the key or the default value when not found</returns>
  INDEX_VALUE? Get(INDEX_KEY key, INDEX_VALUE? defaultValue = default(INDEX_VALUE?));

  ///// <summary>
  ///// Get the ID related to a key, or optionally the ID of the next record when not found
  ///// </summary>
  ///// <param name="key">The key to search for</param>
  ///// <param name="nextItem">When true, returns the next Id (if possible) when key is not found, otherwise returns null</param>
  ///// <returns>A record ID or null</returns>
  //INDEX_VALUE? Get(INDEX_KEY key, bool nextItem = false);
}
