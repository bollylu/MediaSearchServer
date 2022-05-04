namespace MediaSearch.Models.Databases;

public interface IMSDatabase {

  /// <summary>
  /// The name of the database
  /// </summary>
  string Name { get; set; }

  /// <summary>
  /// The description of the database
  /// </summary>
  string Description { get; set; }

  /// <summary>
  /// The path where the database is stored
  /// </summary>
  string RootPath { get; set; }

  /// <summary>
  /// The tables of the database
  /// </summary>
  List<IMSTable> Tables { get; }

  /// <summary>
  /// Get the schema of the database (in json)
  /// </summary>
  /// <returns>A json string representing the database schema</returns>
  string GetSchema();

  /// <summary>
  /// Transforms the database into a string with indentation for inner values
  /// </summary>
  /// <param name="indent">The width of the indentation</param>
  string ToString(int indent);

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
