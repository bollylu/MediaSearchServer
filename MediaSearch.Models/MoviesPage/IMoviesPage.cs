namespace MediaSearch.Models;

public interface IMoviesPage : IJson {

  string Name { get; }
  string Source { get; }

  List<IMovie> Movies { get; }

  int Page { get; }
  int AvailablePages { get; set; }
  int AvailableMovies { get; set; }

  string ToString(bool withDetails);
}
