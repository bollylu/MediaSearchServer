﻿namespace MediaSearch.Models;
public interface IMediaInfo : IMediaInfoGroupMembership, IMediaInfoCreation {

  ELanguage Language { get; init; }
  string Title { get; set; }
  string Description { get; set; }
  List<string> Tags { get; init; }

}
