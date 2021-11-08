using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearchModels {
  public class TMovieGroups : IMovieGroups {
    public string Name { get; set; }
    public List<TMovieGroup> Groups { get; set; } = new List<TMovieGroup>();
  }
}
