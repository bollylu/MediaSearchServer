namespace MediaSearch.Database;

public partial interface IMSDatabase : IDisposable {

  /// <summary>
  /// The name of the database
  /// </summary>
  string Name { get; set; }

  /// <summary>
  /// The description of the database
  /// </summary>
  string Description { get; set; }



  /// <summary>
  /// Transforms the database into a string with indentation for inner values
  /// </summary>
  /// <param name="indent">The width of the indentation</param>
  string ToString(int indent);

  /// <summary>
  /// Indicate if the database is opened or not
  /// </summary>
  bool IsOpened { get; }

  /// <summary>
  /// Dump the raw content of the database for debug purpose
  /// </summary>
  /// <returns>A string describing the raw content</returns>
  string Dump();




}
