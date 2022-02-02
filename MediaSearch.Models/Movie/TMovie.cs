namespace MediaSearch.Models;

/// <summary>
/// Implement a movie
/// </summary>
public class TMovie : AMovie, IJson<IMovie> {

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMovie() : base() { }
  public TMovie(IMovie movie) : base(movie) { }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- IJson --------------------------------------------
  public static JsonSerializerOptions DefaultJsonSerializerOptions {
    get {
      lock (_DefaultJsonSerializerOptionsLock) {
        if (_DefaultJsonSerializerOptions is null) {
          _DefaultJsonSerializerOptions = new JsonSerializerOptions() {
            WriteIndented = true,
            NumberHandling = JsonNumberHandling.Strict
          };
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
  public IMovie ParseJson(string source) {
    return ParseJson(source, DefaultJsonSerializerOptions);
  }

  public IMovie ParseJson(string source, JsonSerializerOptions options) {
    #region === Validate parameters ===
    if (string.IsNullOrWhiteSpace(source)) {
      throw new JsonException("Json movie source is null");
    }
    #endregion === Validate parameters ===

    IMovie? Deserialized = JsonSerializer.Deserialize<TMovie>(source, options);

    if (Deserialized is null) {
      string Error = $"Unable to deserialize json string \"{source}\"";
      LogError(Error);
      throw new JsonException(Error);
    }

    FileName = Deserialized.FileName;
    Name = Deserialized.Name;
    Group = Deserialized.Group;
    SubGroup = Deserialized.SubGroup;
    StoragePath = Deserialized.StoragePath;
    FileExtension = Deserialized.FileExtension;
    Size = Deserialized.Size;
    OutputYear = Deserialized.OutputYear;
    foreach (KeyValuePair<string, string> AltNamesItem in Deserialized.AltNames) {
      AltNames.Add(AltNamesItem.Key, AltNamesItem.Value);
    }
    Tags.AddRange(Deserialized.Tags);
    DateAdded = Deserialized.DateAdded;

    return this;
  }
  #endregion --- Deserializer --------------------------------------------

  #region --- Static Deserializer --------------------------------------------

  public static IMovie? FromJson(string source) {
    if (string.IsNullOrWhiteSpace(source)) {
      throw new ArgumentNullException(nameof(source));
    }
    return JsonSerializer.Deserialize<TMovie>(source, DefaultJsonSerializerOptions);
  }

  public static IMovie? FromJson(string source, JsonSerializerOptions options) {
    if (string.IsNullOrWhiteSpace(source)) {
      throw new ArgumentNullException(nameof(source));
    }
    return JsonSerializer.Deserialize<TMovie>(source, options);
  }


  #endregion --- Static Deserializer --------------------------------------------

  #endregion --- IJson --------------------------------------------
}
