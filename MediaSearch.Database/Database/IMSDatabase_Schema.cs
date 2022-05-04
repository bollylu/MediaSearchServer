namespace MediaSearch.Database;

public partial interface IMSDatabase {

  /// <summary>
  /// Get the schema of the database (in json)
  /// </summary>
  /// <returns>A json string representing the database schema</returns>
  string GetSchema();

  /// <summary>
  /// The tables of the database
  /// </summary>
  List<IMSTable> Tables { get; }

  /// <summary>
  /// Add a table to the database
  /// </summary>
  /// <param name="table">The table to add</param>
  /// <returns>true if ok, false otherwise</returns>
  bool AddTable(IMSTable table);
}
