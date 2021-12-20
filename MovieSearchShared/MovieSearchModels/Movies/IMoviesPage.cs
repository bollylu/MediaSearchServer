namespace MovieSearchModels;

public interface IMoviesPage {

  string Name { get; }
  string Source { get; }

  List<IMovie> Movies { get; }

  int Page { get; }
  int AvailablePages { get; set; }
  int AvailableMovies { get; set; }

  string ToJson();
  string ToJson(JsonWriterOptions options);

  IMoviesPage ParseJson(string source);
  IMoviesPage ParseJson(JsonElement source);

}
