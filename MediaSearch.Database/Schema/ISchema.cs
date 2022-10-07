namespace MediaSearch.Database;

public interface ISchema : IDisposable {

  /// <summary>
  /// Get The list of tables from the schema
  /// </summary>
  /// <returns>A enumerable list of ITable</returns>
  IEnumerable<ITable> GetAllTables();

  /// <summary>
  /// Get one table from the schema
  /// </summary>
  /// <param name="name">The name of the table</param>
  /// <returns>The trable or <see langword="null"/> if unavailable</returns>
  ITable? Get(string name);

  /// <summary>
  /// Get the text list of tables from the schema
  /// </summary>
  /// <returns>A list of string</returns>
  IEnumerable<string> List();

  /// <summary>
  /// Add a table to the schema
  /// </summary>
  /// <param name="table">The table to add</param>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool AddTable(ITable table);

  /// <summary>
  /// Remove a table from the schema
  /// </summary>
  /// <param name="table">The table to remove</param>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool Remove(ITable table);

  /// <summary>
  /// Remove a table from the schema
  /// </summary>
  /// <param name="table">The table to remove</param>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool Remove(string tableName);

  /// <summary>
  /// Clear the schema content
  /// </summary>
  void Clear();

  /// <summary>
  /// The database that the schema belongs to
  /// </summary>
  IDatabase? Database { get; set; }

  /// <summary>
  /// Indicate if the schema exists physically
  /// </summary>
  /// <returns><see langword="true"/> when the schema is available, <see langword="false"/> otherwise</returns>
  bool Exists();

  /// <summary>
  /// Read the content of the schema
  /// </summary>
  /// <returns><see langword="true"/> when ok, <see langword="false"/> otherwise</returns>
  bool Read();

  /// <summary>
  /// Save the content of the schema
  /// </summary>
  /// <returns><see langword="true"/> when ok, <see langword="false"/> otherwise</returns>
  bool Save();

  /// <summary>
  /// Build the content of the schema
  /// </summary>
  /// <returns><see langword="true"/> when ok, <see langword="false"/> otherwise</returns>
  bool Build();
}
