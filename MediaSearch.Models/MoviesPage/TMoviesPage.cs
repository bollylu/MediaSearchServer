namespace MediaSearch.Models;

public class TMoviesPage : ALoggable, IMoviesPage, IJson<IMoviesPage> {

  [JsonIgnore]
  public override ILogger Logger {
    get {
      return base.Logger;
    }

    set {
      base.Logger = value;
    }
  }
  #region --- Public properties ------------------------------------------------------------------------------
  public string Name { get; set; } = "";

  public List<IMovie> Movies { get; set; } = new();

  public int Page { get; set; } = 0;
  public int AvailablePages { get; set; } = 0;
  public int AvailableMovies { get; set; } = 0;
  public string Source { get; set; } = "";
  #endregion --- Public properties ---------------------------------------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMoviesPage() { }
  public TMoviesPage(IMoviesPage page) {
    if (page is null) {
      return;
    }
    Name = page.Name;
    Page = page.Page;
    AvailablePages = page.AvailablePages;
    AvailableMovies = page.AvailableMovies;
    Source = page.Source;
    Movies.AddRange(page.Movies);
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{Name} / page {Page}/{AvailablePages} [{Movies.Count} of {AvailableMovies} movies]");
    return RetVal.ToString();
  }

  public string ToString(bool withDetails) {
    StringBuilder RetVal = new StringBuilder(ToString());
    if (withDetails) {
      foreach (IMovie MovieItem in Movies) {
        RetVal.AppendLine($"# {MovieItem}");
      }
    }
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  #region --- IJson<IMoviesPage> --------------------------------------------
  public static JsonSerializerOptions DefaultJsonSerializerOptions {
    get {
      lock (_DefaultJsonSerializerOptionsLock) {
        if (_DefaultJsonSerializerOptions is null) {
          _DefaultJsonSerializerOptions = new JsonSerializerOptions() {
            WriteIndented = true,
            NumberHandling = JsonNumberHandling.Strict
          };
          _DefaultJsonSerializerOptions.Converters.Add(new TMoviesPageJsonConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TMovieJsonConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TDateOnlyJsonConverter());

        }
        return _DefaultJsonSerializerOptions;
      }
    }
    set {
      lock (_DefaultJsonSerializerOptionsLock) {
        _DefaultJsonSerializerOptions = value;
      }
    }
  }
  private static JsonSerializerOptions? _DefaultJsonSerializerOptions;
  private static readonly object _DefaultJsonSerializerOptionsLock = new object();

  #region --- Serializer --------------------------------------------
  public string ToJson() {
    return ToJson(DefaultJsonSerializerOptions);
  }

  public string ToJson(JsonSerializerOptions options) {
    return JsonSerializer.Serialize(this, options);
  }
  #endregion --- Serializer --------------------------------------------

  #region --- Deserializer --------------------------------------------

  public IMoviesPage ParseJson(string source) {
    return ParseJson(source, DefaultJsonSerializerOptions);
  }

  public IMoviesPage ParseJson(string source, JsonSerializerOptions options) {
    #region === Validate parameters ===
    if (string.IsNullOrWhiteSpace(source)) {
      throw new JsonException("Json MoviesPage source is null");
    }
    #endregion === Validate parameters ===

    TMoviesPage? Deserialized = JsonSerializer.Deserialize<TMoviesPage>(source, options);

    if (Deserialized is null) {
      string Error = $"Unable to deserialize json string \"{source}\"";
      LogError(Error);
      throw new JsonException(Error);
    }

    Name = Deserialized.Name;
    Source = Deserialized.Source;
    Page = Deserialized.Page;
    AvailablePages = Deserialized.AvailablePages;
    AvailableMovies = Deserialized.AvailableMovies;
    Movies.AddRange(Deserialized.Movies);

    return this;
  }
  #endregion --- Deserializer --------------------------------------------

  #region --- Static Deserializer --------------------------------------------
  public static IMoviesPage? FromJson(string source) {
    if (string.IsNullOrWhiteSpace(source)) {
      throw new ArgumentNullException(nameof(source));
    }
    return JsonSerializer.Deserialize<TMoviesPage>(source, DefaultJsonSerializerOptions);
  }

  public static IMoviesPage? FromJson(string source, JsonSerializerOptions options) {
    if (string.IsNullOrWhiteSpace(source)) {
      throw new ArgumentNullException(nameof(source));
    }
    return JsonSerializer.Deserialize<TMoviesPage>(source, options);
  }


  #endregion --- Static Deserializer --------------------------------------------

  #endregion --- IJson<IMoviesPage> --------------------------------------------

  #region --- Static instance --------------------------------------------
  public static IMoviesPage Empty => _Empty ??= new TMoviesPage();
  private static IMoviesPage? _Empty;
  #endregion --- Static instance --------------------------------------------
}
