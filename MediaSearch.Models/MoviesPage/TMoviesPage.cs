namespace MediaSearch.Models;

public class TMoviesPage : AJson<TMoviesPage>, IMoviesPage {

  #region --- Public properties ------------------------------------------------------------------------------
  public string Name { get; set; }

  public List<IMovie> Movies { get; set; } = new();

  public int Page { get; set; }
  public int AvailablePages { get; set; }
  public int AvailableMovies { get; set; }
  public string Source { get; set; }
  #endregion --- Public properties ---------------------------------------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMoviesPage() { }
  public TMoviesPage(IMoviesPage movies) {
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
    RetVal.AppendLine($"page {Page}/{AvailablePages} [{Movies.Count} movies on {AvailableMovies}] ");
    return RetVal.ToString();
  }

  public string ToString(bool withDetails) {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{Name} / page {Page}/{AvailablePages} [{AvailableMovies}] {Movies.Count} movies");
    if (withDetails) {
      foreach (IMovie MovieItem in Movies) {
        RetVal.AppendLine($"# {MovieItem}");
      }
    }
    return RetVal.ToString();
  }

  public override string ToJson(JsonWriterOptions options) {
    using (MemoryStream Utf8JsonStream = new()) {
      using (Utf8JsonWriter Writer = new Utf8JsonWriter(Utf8JsonStream, options)) {

        Writer.WriteStartObject();

        //Writer.WriteString(nameof(IMoviesPage.Name), Name);
        //Writer.WriteString(nameof(IMoviesPage.Source), Source);
        Writer.WriteNumber(nameof(IMoviesPage.Page), Page);
        Writer.WriteNumber(nameof(IMoviesPage.AvailablePages), AvailablePages);
        Writer.WriteNumber(nameof(IMoviesPage.AvailableMovies), AvailableMovies);

        Writer.WriteStartArray(nameof(IMoviesPage.Movies));
        foreach (IJson MovieItem in Movies.OfType<IJson>()) {
          Writer.WriteRawValue(MovieItem.ToJson(options));
        }
        Writer.WriteEndArray();

        Writer.WriteEndObject();
      }

      return Encoding.UTF8.GetString(Utf8JsonStream.ToArray());
    }
  }

  public override TMoviesPage ParseJson(string source) {
    if (string.IsNullOrWhiteSpace(source)) {
      return default;
    }

    JsonDocument JsonMovies = JsonDocument.Parse(source);
    JsonElement Root = JsonMovies.RootElement;

    //Name = Root.GetPropertyEx(nameof(IMoviesPage.Name)).GetString();
    //Source = Root.GetPropertyEx(nameof(IMoviesPage.Source)).GetString();
    Page = Root.GetPropertyEx(nameof(IMoviesPage.Page)).GetInt32();
    AvailablePages = Root.GetPropertyEx(nameof(IMoviesPage.AvailablePages)).GetInt32();
    AvailableMovies = Root.GetPropertyEx(nameof(IMoviesPage.AvailableMovies)).GetInt32();

    foreach (JsonElement MovieItem in Root.GetPropertyEx(nameof(IMoviesPage.Movies)).EnumerateArray()) {
      Movies.Add(TMovie.FromJson(MovieItem));
    }

    return this;
  }

  public static IMoviesPage Empty => _Empty ??= new TMoviesPage();
  private static IMoviesPage _Empty;
}
