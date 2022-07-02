namespace MediaSearch.Database;

public partial interface IDatabase {

  /// <summary>
  /// Open the database
  /// </summary>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool Open();

  /// <summary>
  /// Close the database
  /// </summary>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool Close();

}
