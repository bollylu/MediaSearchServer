namespace MediaSearch.Database;

public partial interface IDatabase {

  /// <summary>
  /// Create a table in storage
  /// </summary>
  /// <param name="table">The table to create</param>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool TableCreate(ITable table);

  /// <summary>
  /// Test the existence of a table in storage
  /// </summary>
  /// <param name="table">The table to test</param>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool TableExists(ITable table);

  /// <summary>
  /// Test the existence of a table in storage
  /// </summary>
  /// <param name="table">The name of the table to test</param>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool TableExists(string tableName);

  /// <summary>
  /// Test the integrity of a table in storage
  /// </summary>
  /// <param name="table">The table to test</param>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool TableCheck(ITable table);

  /// <summary>
  /// Test the integrity of a table in storage
  /// </summary>
  /// <param name="table">The name of the table to test</param>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool TableCheck(string tableName);

  /// <summary>
  /// Remove a table from storage
  /// </summary>
  /// <param name="table">The table to remove</param>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool TableRemove(ITable table);

  /// <summary>
  /// Remove a table from storage
  /// </summary>
  /// <param name="table">The name of the table to remove</param>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool TableRemove(string tableName);

  /// <summary>
  /// Read the header of a table from storage
  /// </summary>
  /// <param name="table">The table that we want to read the header</param>
  /// <returns>The header or <see langword="null"/> if any error</returns>
  ITableHeader? TableReadHeader(ITable table);
  /// <summary>
  /// Read the header of a table from storage
  /// </summary>
  /// <param name="table">The name of the table that we want to read the header</param>
  /// <returns>The header or <see langword="null"/> if any error</returns>
  ITableHeader? TableReadHeader(string tableName);

  /// <summary>
  /// Rebuild the indexes of a tables
  /// </summary>
  /// <param name="table">The table to rebuild the indexes</param>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool TableReindex(ITable table);

  /// <summary>
  /// Rebuild the indexes of a tables
  /// </summary>
  /// <param name="table">The name of the table to rebuild the indexes</param>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool TableReindex(string tableName);

  /// <summary>
  /// Dump the raw content of a table for debug purpose
  /// </summary>
  /// <returns>A string describing the raw content</returns>
  string TableDump(ITable table);

  /// <summary>
  /// List all the physically present user tables from the database
  /// </summary>
  /// <returns></returns>
  IEnumerable<ITable> TableList();

  /// <summary>
  /// List all the physically present system tables from the database
  /// </summary>
  /// <returns></returns>
  IEnumerable<ITable> TableSystemList();

}
