﻿namespace MediaSearch.Models;
public interface IMediaInfoGroupMembership {

  /// <summary>
  /// The group where the item belongs
  /// </summary>
  string Group { get; set; }

  /// <summary>
  /// Indicate if the item belongs to a group
  /// </summary>
  bool IsGroupMember { get; }

}
