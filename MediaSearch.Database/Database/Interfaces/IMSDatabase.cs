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
  /// The full name of the database, including rootpath and name
  /// </summary>
  string DatabaseFullName { get; }

  /// <summary>
  /// Transforms the database into a string with indentation for inner values
  /// </summary>
  /// <param name="indent">The width of the indentation</param>
  string ToString(int indent);

  /// <summary>
  /// Indicate if the database is opened or not
  /// </summary>
  bool IsOpened { get; }

}
