namespace MediaSearch.Database;

public partial interface IMSDatabase {

  /// <summary>
  /// Get the schema of the database (in json)
  /// </summary>
  /// <returns>A json string representing the database schema</returns>
  string GetSchema();

  /// <summary>
  /// Add a table to the database
  /// </summary>
  /// <param name="table">The table to add</param>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  public bool AddTable(IMSTable table);

  /// <summary>
  /// Remove a table from the database
  /// </summary>
  /// <param name="table">The table to remove</param>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  public bool RemoveTable(IMSTable table);

  /// <summary>
  /// Remove a table from the database
  /// </summary>
  /// <param name="table">The name of the table to remove</param>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  public bool RemoveTable(string table);

  /// <summary>
  /// Retrieve a table for the list of tables
  /// </summary>
  /// <param name="tableName">The name of the table (case insensitive)</param>
  /// <returns>The table or <see langword="null"/></returns>
  protected IMSTable? GetTable(string tableName);
}
