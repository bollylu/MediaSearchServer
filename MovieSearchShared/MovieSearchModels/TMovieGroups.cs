﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models {
  public class TMovieGroups : IMovieGroups {
    public string Name { get; set; }
    public List<TMovieGroup> Groups { get; set; } = new List<TMovieGroup>();
  }
}
