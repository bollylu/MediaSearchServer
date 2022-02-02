namespace MediaSearch.Models;

public interface IJson {

  /// <summary>
  /// Convert the object to a Json string
  /// </summary>
  /// <returns>A Json string</returns>
  string ToJson();

  /// <summary>
  /// Convert the object to a Json string using JsonWriterOptions
  /// </summary>
  /// <param name="options">The options to convert to Json</param>
  /// <returns>A Json string</returns>
  string ToJson(JsonSerializerOptions options);

  static abstract JsonSerializerOptions DefaultJsonSerializerOptions { get; }
}

public interface IJson<T> : IJson {
  /// <summary>
  /// Convert a Json string to an object of type <typeparamref name="T"/>
  /// </summary>
  /// <typeparam name="T">The type of the object to convert the Json string to</typeparam>
  /// <typeparam name="J">The admissible type to convert to</typeparam>
  /// <param name="source">The Json string</param>
  /// <returns>An object of type <typeparamref name="T"/></returns>
  T? ParseJson(string source);

  /// <summary>
  /// Convert a Json string to an object of type <typeparamref name="T"/>
  /// </summary>
  /// <typeparam name="T">The type of the object to convert the Json string to</typeparam>
  /// <typeparam name="J">The admissible type to convert to</typeparam>
  /// <param name="source">The Json string</param>
  /// <param name="options">Options for the serializer</param>
  /// <returns>An object of type <typeparamref name="T"/></returns>
  T? ParseJson(string source, JsonSerializerOptions options);

  static abstract T? FromJson(string source);
  static abstract T? FromJson(string source, JsonSerializerOptions options);
}

public interface IJsonElement<T> : IJson {
  /// <summary>
  /// Convert a JsonElement to an object of type <typeparamref name="T"/>
  /// </summary>
  /// <typeparam name="T">The type of the object to convert the Json string to</typeparam>
  /// <param name="source">The JsonElement to convert</param>
  /// <returns>An object of type <typeparamref name="T"/></returns>
  T ParseJson(JsonElement source);

  /// <summary>
  /// Convert a JsonElement to an object of type <typeparamref name="T"/>
  /// </summary>
  /// <typeparam name="T">The type of the object to convert the Json string to</typeparam>
  /// <param name="source">The JsonElement to convert</param>
  /// <param name="options">Options for the serializer</param>
  /// <returns>An object of type <typeparamref name="T"/></returns>
  T ParseJson(JsonElement source, JsonSerializerOptions options);
}

