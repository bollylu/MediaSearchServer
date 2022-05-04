namespace MediaSearch.Models;

public class TMSTableIndexByName : IMSIndex<IMovie, string>  {

  /// <summary>
  /// When is the last update to any item in this table
  /// </summary>
  public DateTime LastUpdate { get; set; }

  /// <summary>
  /// Table can contain only only data type
  /// </summary>
  public Type IndexType => typeof(IMovie);

  /// <summary>
  /// Transforms the index to a string, with indentation
  /// </summary>
  /// <param name="indent">The current indentation</param>
  /// <returns>A string representing the table</returns>
  public string ToString(int indent) {
    StringBuilder RetVal = new();
    RetVal.AppendIndent($"- {nameof(IndexType)} = {IndexType.GetType().Name}")
          .AppendIndent($"- {nameof(LastUpdate)} = {LastUpdate}")
          .AppendIndent($"- {nameof(IndexedValues)} = {IndexedValues.Count} value(s)");
    return RetVal.ToString();
  }

  /// <summary>
  /// The index of the records of the table. Key = index key, value = record id.
  /// </summary>
  public List<KeyValuePair<string, string>> IndexedValues { get; } = new();

}
