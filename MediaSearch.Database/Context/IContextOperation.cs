namespace MediaSearch.Database;

public interface IContextOperation {
  #region --- Converters -------------------------------------------------------------------------------------
  /// <summary>
  /// Extended version of ToString with an optional indent size
  /// </summary>
  /// <param name="indent">The number of spaces to indent content</param>
  /// <returns>A string representation of the object</returns>
  string ToString(int indent);
  #endregion --- Converters -------------------------------------------------------------------------------------

  EContextOperation Operation { get; }

}

public interface IContextOperation<RECORD> : IContextOperation where RECORD : IRecord {
  RECORD? Record { get; }
}
