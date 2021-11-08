using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MovieSearchModels {
  public interface IMovies {
    string Name { get; }
    string Source { get; }

    List<IMovie> Movies { get; }
    
    int Page { get; }
    int AvailablePages { get; }
    int AvailableMovies { get; }

    string ToJson();
    string ToJson(JsonWriterOptions options);

    IMovies ParseJson(string source);
    IMovies ParseJson(JsonElement source);

  }
}
