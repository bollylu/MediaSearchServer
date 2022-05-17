namespace MediaSearch.Database;

public partial interface IMSDatabase {

  /// <summary>
  /// Create an empty non-existing database (based on RootPath and Name)
  /// </summary>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool Create();

  /// <summary>
  /// Create a non-existing database (based on RootPath and Name and schema)
  /// </summary>
  /// <param name="schema">A json string representing the database schema</param>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool Create(string schema);

  /// <summary>
  /// Remove an existing database 
  /// </summary>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool Remove();

  /// <summary>
  /// Test for database existence
  /// </summary>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool Exists();

  /// <summary>
  /// Check if everything is ok with database schema, structure, content, index
  /// </summary>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool DbCheck();

}
