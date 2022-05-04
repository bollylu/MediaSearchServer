namespace MediaSearch.Database;

public interface IMSTableHeader<RECORD> where RECORD : IID<string> {

  /// <summary>
  /// The name of the table
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// A description of the table content
  /// </summary>
  public string Description { get; set; }

  /// <summary>
  /// When is the last update to any item in this table
  /// </summary>
  public DateTime LastUpdate { get; set; }

  /// <summary>
  /// Table can contain only only data type
  /// </summary>
  public Type TableType { get; }

  /// <summary>
  /// Transforms the table to a string, with indentation
  /// </summary>
  /// <param name="indent">The current indentation</param>
  /// <returns>A string representing the table</returns>
  string ToString(int indent);

  /// <summary>
  /// The index of the records of the table, Name <=> Id
  /// </summary>
  TMSIndex<RECORD, string, string> IndexByName { get; }

}
