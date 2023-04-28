using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace MediaSearch.Models;

public interface IJson {
  public static JsonSerializerOptions DefaultJsonSerializerOptions {
    get {
      lock (_DefaultJsonSerializerOptionsLock) {
        if (_DefaultJsonSerializerOptions is null) {
          _DefaultJsonSerializerOptions = new JsonSerializerOptions() {
#if DEBUG
            WriteIndented = true,
#else
            WriteIndented = false,
#endif
            NumberHandling = JsonNumberHandling.Strict,
            DictionaryKeyPolicy = new TIdenticalJsonNamingPolicy(),
            IgnoreReadOnlyFields = true,
            IgnoreReadOnlyProperties = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement)
          };
          _DefaultJsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TLanguageDictionaryStringConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TAboutJsonConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TDateOnlyJsonConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TFilterJsonConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TIPAddressJsonConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TMovieJsonConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TMoviesPageJsonConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TUserAccountInfoJsonConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TUserAccountJsonConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TUserAccountSecretJsonConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TUserTokenJsonConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TMovieInfoContentMetaConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TMultiItemsSelectionJsonConverter());
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

  public static void AddJsonConverter(JsonConverter converter) {
    lock (_DefaultJsonSerializerOptionsLock) {
      _DefaultJsonSerializerOptions?.Converters.Add(converter);
    }
  }

  /// <summary>
  /// Convert the object to a Json string
  /// </summary>
  /// <returns>A Json string</returns>
  public string ToJson() {
    return JsonSerializer.Serialize(this, GetType(), DefaultJsonSerializerOptions);
  }

  /// <summary>
  /// Convert the object to a Json string using JsonWriterOptions
  /// </summary>
  /// <param name="options">The options to convert to Json</param>
  /// <returns>A Json string</returns>
  public string ToJson(JsonSerializerOptions options) {
    return JsonSerializer.Serialize(this, GetType(), options);
  }

}

public interface IJson<T> : IJson where T : class {

  public static T? FromJson(string source) {
    if (string.IsNullOrWhiteSpace(source)) {
      throw new ArgumentNullException(nameof(source));
    }
    return JsonSerializer.Deserialize<T?>(source, DefaultJsonSerializerOptions);
  }

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

