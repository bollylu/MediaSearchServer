namespace MediaSearch.Database;

public partial interface IDatabase {

  /// <summary>
  /// Get the schema
  /// </summary>
  /// <returns></returns>
  ISchema Schema { get; }

  /// <summary>
  /// Build the schema of the database from a discovery of its tables
  /// </summary>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool SchemaBuild();

  /// <summary>
  /// Read the schema of the database
  /// </summary>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool SchemaRead();

  /// <summary>
  /// Save the schema of the database
  /// </summary>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool SchemaSave();

  /// <summary>
  /// Check if the schema if physically present
  /// </summary>
  /// <returns><see langword="true"/> if schema if available, <see langword="false"/> otherwise</returns>
  bool SchemaExists();

  ///// <summary>
  ///// Add a table to the database
  ///// </summary>
  ///// <param name="table">The table to add</param>
  ///// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  //bool AddTableToSchema(IMSTable table);

  ///// <summary>
  ///// Remove a table from the database
  ///// </summary>
  ///// <param name="table">The table to remove</param>
  ///// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  //bool RemoveTable(IMSTable table);

  ///// <summary>
  ///// Remove a table from the database
  ///// </summary>
  ///// <param name="table">The name of the table to remove</param>
  ///// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  //bool RemoveTable(string table);

  ///// <summary>
  ///// Retrieve a table from the list of tables
  ///// </summary>
  ///// <param name="tableName">The name of the table (case insensitive)</param>
  ///// <returns>The table or <see langword="null"/></returns>
  //IMSTable? GetTable(string tableName);

  ///// <summary>
  ///// Retrieve the complete list of user tables
  ///// </summary>
  ///// <returns>An enumeration of the user tables</returns>
  //IEnumerable<IMSTable> GetTables();
}
