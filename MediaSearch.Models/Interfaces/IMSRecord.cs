namespace MediaSearch.Models;
public interface IMSRecord : IID<string> {

  #region --- Converters -------------------------------------------------------------------------------------
  /// <summary>
  /// Extended version of ToString with an optional indent size
  /// </summary>
  /// <param name="indent">The number of spaces to indent content</param>
  /// <returns>A string representation of the object</returns>
  string ToString(int indent);
  #endregion --- Converters -------------------------------------------------------------------------------------

}
