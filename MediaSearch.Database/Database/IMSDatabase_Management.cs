namespace MediaSearch.Database;

public partial interface IMSDatabase {

  /// <summary>
  /// Create an empty non-existing database (based on RootPath and Name)
  /// </summary>
  /// <returns>true if ok, false otherwise</returns>
  bool Create();

  /// <summary>
  /// Create a non-existing database (based on RootPath and Name and schema)
  /// </summary>
  /// <param name="schema">A json string representing the database schema</param>
  /// <returns>true if ok, false otherwise</returns>
  bool Create(string schema);

  /// <summary>
  /// Remove an existing database 
  /// </summary>
  /// <returns>true if ok, false otherwise</returns>
  bool Remove();

  /// <summary>
  /// Test for database existence
  /// </summary>
  /// <returns>true if ok, false otherwise</returns>
  bool Exists();

  /// <summary>
  /// Rebuild all the indexes of the database (for all the tables)
  /// </summary>
  /// <returns>true if ok, false otherwise</returns>
  bool Reindex();

  /// <summary>
  /// Check if everything is ok with database schema, structure, content, index
  /// </summary>
  /// <returns>true if ok, false otherwise</returns>
  bool DbCheck();


}
