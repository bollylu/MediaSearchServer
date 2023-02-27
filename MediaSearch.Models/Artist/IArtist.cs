﻿namespace MediaSearch.Models;
public interface IArtist : IRecord {
  string Name { get; set; }
  string FirstName { get; set; }
  string LastName { get; set; }
  string Alias { get; set; }
  string Description { get; set; }
}
