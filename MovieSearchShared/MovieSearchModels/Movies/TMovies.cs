using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using BLTools.Diagnostic.Logging;

namespace MovieSearch.Models {
  public class TMovies : IMovies {
    public string Name { get; set; }

    public List<IMovie> Movies { get; set; } = new();

    public int Page { get; set; }
    public int AvailablePages { get; set; }
    public string Source { get; set; }

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TMovies() { }
    public TMovies(IMovies movies) {
      if (movies is null) {
        return;
      }
      Name = movies.Name;
      Page = movies.Page;
      AvailablePages = movies.AvailablePages;
      Source = movies.Source;
      Movies.AddRange(movies.Movies);
    } 
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendLine($"{Name} / page {Page}/{AvailablePages}");
      foreach(IMovie MovieItem in Movies) {
        RetVal.AppendLine($"# {MovieItem}");
      }
      return RetVal.ToString();
    }

    #region --- ToJson --------------------------------------------
    public string ToJson() {
      return ToJson(new JsonWriterOptions());
    }

    public string ToJson(JsonWriterOptions options) {
      using (MemoryStream Utf8JsonStream = new()) {
        using (Utf8JsonWriter Writer = new Utf8JsonWriter(Utf8JsonStream, options)) {

          Writer.WriteStartObject();

          Writer.WriteString(nameof(IMovies.Name), Name);
          Writer.WriteString(nameof(IMovies.Source), Source);
          Writer.WriteNumber(nameof(IMovies.Page), Page);
          Writer.WriteNumber(nameof(IMovies.AvailablePages), AvailablePages);

          Writer.WriteStartArray(nameof(IMovies.Movies));
          foreach (IMovie MovieItem in Movies) {
            Writer.WriteRawValue(MovieItem.ToJson(options));
          }
          Writer.WriteEndArray();

          Writer.WriteEndObject();
        }

        return Encoding.UTF8.GetString(Utf8JsonStream.ToArray());
      }
    } 
    #endregion --- ToJson --------------------------------------------

    #region --- ParseJson --------------------------------------------
    public IMovies ParseJson(string source) {
      if (string.IsNullOrWhiteSpace(source)) {
        return null;
      }

      JsonDocument JsonMovies = JsonDocument.Parse(source);
      JsonElement Root = JsonMovies.RootElement;

      Name = Root.GetPropertyEx(nameof(IMovies.Name)).GetString();
      Source = Root.GetPropertyEx(nameof(IMovies.Source)).GetString();
      Page = Root.GetPropertyEx(nameof(IMovies.Page)).GetInt32();
      AvailablePages = Root.GetPropertyEx(nameof(IMovies.AvailablePages)).GetInt32();

      foreach (JsonElement MovieItem in Root.GetPropertyEx(nameof(IMovies.Movies)).EnumerateArray()) {
        Movies.Add(TMovie.FromJson(MovieItem));
      }

      return this;
    }

    public IMovies ParseJson(JsonElement source) {
      return ParseJson(source.GetRawText());
    }
    #endregion --- ParseJson --------------------------------------------

    #region --- Static FromJson --------------------------------------------
    public static IMovies FromJson(string source) {
      IMovies Movies = new TMovies();
      return Movies.ParseJson(source);
    }

    public static IMovies FromJson(JsonElement source) {
      if (source.ValueKind != JsonValueKind.Object) {
        throw new JsonException("Json movies source is not an object");
      }

      return FromJson(source.GetRawText());
    } 
    #endregion --- Static FromJson --------------------------------------------
  }

  
}
