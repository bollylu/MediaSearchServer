namespace MediaSearch.Models;
public interface IDirty {

  /// <summary>
  /// Indicate when the record has been modified since its last save
  /// </summary>
  bool IsDirty { get; }

  /// <summary>
  /// Set the dirty indicator
  /// </summary>
  void SetDirty();

  /// <summary>
  /// Clear the dirty indicator
  /// </summary>
  void ClearDirty();

}
