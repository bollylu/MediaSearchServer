namespace MediaSearch.Models;

public interface IMSIndex<T, I> where T : IMedia where I : notnull {

  /// <summary>
  /// When is the last update to any item in this table
  /// </summary>
  public DateTime LastUpdate { get; set; }

  /// <summary>
  /// Table can contain only only data type
  /// </summary>
  public Type IndexType { get; }

  /// <summary>
  /// Transforms the index to a string, with indentation
  /// </summary>
  /// <param name="indent">The current indentation</param>
  /// <returns>A string representing the table</returns>
  string ToString(int indent);

  /// <summary>
  /// The index of the records of the table. Key = index key, value = record id.
  /// </summary>
  List<KeyValuePair<I, string>> IndexedValues { get; }

}
