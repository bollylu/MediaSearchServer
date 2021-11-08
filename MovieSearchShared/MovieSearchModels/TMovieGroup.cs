using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearchModels {
  public class TMovieGroup : IMovieGroup {
    public string Name { get; set; }
    public int Count { get; set; }
  }
}
