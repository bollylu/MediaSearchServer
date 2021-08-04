using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models {
  public interface IMovieGroups {
    string Name { get; }
    List<TMovieGroup> Groups { get; }
  }
}
