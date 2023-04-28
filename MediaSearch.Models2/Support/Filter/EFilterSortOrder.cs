namespace MediaSearch.Models2;

/// <summary>
/// How to sort records
/// </summary>
public enum EFilterSortOrder {
  /// <summary>
  /// Sort by name
  /// </summary>
  Name,
  /// <summary>
  /// Sort by output year
  /// </summary>
  OutputYear,
  /// <summary>
  /// Sort by group
  /// </summary>
  Group,
  /// <summary>
  /// Custom sort order
  /// </summary>
  Custom
}
