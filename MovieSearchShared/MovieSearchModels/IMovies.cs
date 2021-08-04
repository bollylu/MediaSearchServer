using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models {
  public interface IMovies {
    string Name { get; }
    string Source { get; }
    List<TMovie> Movies { get; }
    int Page { get; }
    int AvailablePages { get; }

  }
}
