namespace MediaSearch.Models2;

/// <summary>
/// What to get from back end
/// </summary>
public enum EFilterGroup {
  /// <summary>
  /// Only records with group membership
  /// </summary>
  WithGroup,
  /// <summary>
  /// Only records without any group membership
  /// </summary>
  WithoutGroup,
  /// <summary>
  /// All records, regardless of the group membership
  /// </summary>
  All
}
