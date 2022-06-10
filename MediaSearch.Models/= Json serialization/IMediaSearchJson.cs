namespace MediaSearch.Models;

public interface IJson {

  static IJson() {
    lock (_DefaultJsonSerializerOptionsLock) {
      if (_DefaultJsonSerializerOptions is null) {
        throw new ApplicationException("DefaultJsonSerializerOption is null");
      }
#if DEBUG
      _DefaultJsonSerializerOptions.WriteIndented = true;
#else
      _DefaultJsonSerializerOptions.WriteIndented = false;
#endif
      _DefaultJsonSerializerOptions.NumberHandling = JsonNumberHandling.Strict;
      _DefaultJsonSerializerOptions.DictionaryKeyPolicy = new TIdenticalJsonNamingPolicy();
      _DefaultJsonSerializerOptions.IgnoreReadOnlyFields = true;
      _DefaultJsonSerializerOptions.IgnoreReadOnlyProperties = true;
      _DefaultJsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement);
      _DefaultJsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
      _DefaultJsonSerializerOptions.Converters.Add(new TLanguageDictionaryStringConverter());
      _DefaultJsonSerializerOptions.Converters.Add(new TAboutJsonConverter());
      _DefaultJsonSerializerOptions.Converters.Add(new TFilterJsonConverter());
      _DefaultJsonSerializerOptions.Converters.Add(new TMovieJsonConverter());
      _DefaultJsonSerializerOptions.Converters.Add(new TMoviesPageJsonConverter());
      _DefaultJsonSerializerOptions.Converters.Add(new TUserAccountInfoJsonConverter());
      _DefaultJsonSerializerOptions.Converters.Add(new TUserAccountJsonConverter());
      _DefaultJsonSerializerOptions.Converters.Add(new TUserAccountSecretJsonConverter());
      _DefaultJsonSerializerOptions.Converters.Add(new TUserTokenJsonConverter());
      _DefaultJsonSerializerOptions.Converters.Add(new TMovieInfoContentMetaConverter());
      _DefaultJsonSerializerOptions.Converters.Add(new TLanguageTextInfoConverter());
      _DefaultJsonSerializerOptions.Converters.Add(new TLanguageTextInfosConverter());
      _DefaultJsonSerializerOptions.Converters.Add(new TMovieInfoFileMetaConverter());
    }
  }

  /// <summary>
  /// Convert the object to a Json string
  /// </summary>
  /// <returns>A Json string</returns>
  public new string ToJson() {
    return JsonSerializer.Serialize(this, GetType(), DefaultJsonSerializerOptions);
  }
}

public interface IJson<T> : IJson {

  public static T? FromJson(string source) {
    if (string.IsNullOrWhiteSpace(source)) {
      throw new ArgumentNullException(nameof(source));
    }
    return JsonSerializer.Deserialize<T?>(source, DefaultJsonSerializerOptions);
  }

  /// <summary>
  /// Deserialize a Json to a T type
  /// </summary>
  /// <param name="source">The Json source</param>
  /// <param name="options">The serialization options to use</param>
  /// <returns>The deserialized value as T type</returns>
  /// <exception cref="ArgumentNullException"></exception>
  public static T? FromJson(string source, JsonSerializerOptions options) {
    if (string.IsNullOrWhiteSpace(source)) {
      throw new ArgumentNullException(nameof(source));
    }
    return JsonSerializer.Deserialize<T?>(source, options);
  }
}

//public interface IJsonElement<T> : IJson where T : class {
//  /// <summary>
//  /// Convert a JsonElement to an object of type <typeparamref name="T"/>
//  /// </summary>
//  /// <typeparam name="T">The type of the object to convert the Json string to</typeparam>
//  /// <param name="source">The JsonElement to convert</param>
//  /// <returns>An object of type <typeparamref name="T"/></returns>
//  T ParseJson(JsonElement source);

//  /// <summary>
//  /// Convert a JsonElement to an object of type <typeparamref name="T"/>
//  /// </summary>
//  /// <typeparam name="T">The type of the object to convert the Json string to</typeparam>
//  /// <param name="source">The JsonElement to convert</param>
//  /// <param name="options">Options for the serializer</param>
//  /// <returns>An object of type <typeparamref name="T"/></returns>
//  T ParseJson(JsonElement source, JsonSerializerOptions options);
//}

