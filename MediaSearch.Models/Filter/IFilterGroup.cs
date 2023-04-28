﻿namespace MediaSearch.Models;

public interface IFilterGroup {

  /// <summary>
  /// A list of group memberships
  /// </summary>
  List<string> GroupMemberships { get; }

  /// <summary>
  /// How to filter on group membership
  /// </summary>
  EFilterGroup GroupFilter { get; }

  /// <summary>
  /// Only the media with a group assigned
  /// </summary>
  bool GroupOnly { get; }

  string Group { get; }
  string SubGroup { get; }


}
