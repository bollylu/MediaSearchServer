using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models {
  public interface IMovieGroup {
    string Name { get; }
    int Count { get; }
  }
}
