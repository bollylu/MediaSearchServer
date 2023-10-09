namespace MediaSearch.Models;
public interface IMediaInfoGroupMembership {

  /// <summary>
  /// The groups where the item belongs
  /// </summary>
  List<string> Groups { get; init; }

  /// <summary>
  /// Indicate if the item belongs to a group (i.e. if the item has something in the Group property)
  /// Used as a shortcut for filtering instead of testing if Group!=""
  /// </summary>
  bool IsGroupMember { get; }

}
