namespace MediaSearch.Models;
public interface IRecord : IId<string> {

  #region --- Converters -------------------------------------------------------------------------------------
  /// <summary>
  /// Extended version of ToString with an optional indent size
  /// </summary>
  /// <param name="indent">The number of spaces to indent content</param>
  /// <returns>A string representation of the object</returns>
  string ToString(int indent);

  public string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.Append(Id);
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

}
