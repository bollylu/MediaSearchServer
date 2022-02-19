using BLTools.Encryption;

namespace MediaSearch.Models;

public class TUserToken : ADataModel, IUserToken, IJson<TUserToken> {
  public const int SESSION_DURATION_IN_MIN = 15;

  [JsonPropertyName(nameof(Token))]
  public string Token { get; set; } = "";

  [JsonPropertyName(nameof(Expiration))]
  public DateTime Expiration { get; set; } = DateTime.MaxValue;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TUserToken() { 
    Token = Random.Shared.NextInt64().ToString().HashToBase64();
    Expiration = DateTime.UtcNow.AddMinutes(SESSION_DURATION_IN_MIN);
  }
  public TUserToken(string token) {
    Token = token;
  }
  public TUserToken(string token, DateTime expiration) {
    Token = token;
    Expiration = expiration;
  }
  public TUserToken(IUserToken userToken) {
    Token = userToken.Token;
    Expiration = userToken.Expiration;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  //public string ToJson() {
  //  return JsonSerializer.Serialize(this, DefaultJsonSerializerOptions);
  //}

  //public string ToJson(JsonSerializerOptions options) {
  //  return JsonSerializer.Serialize(this, options);
  //}

  //public static JsonSerializerOptions DefaultJsonSerializerOptions {
  //  get {
  //    lock (_DefaultJsonSerializerOptionsLock) {
  //      if (_DefaultJsonSerializerOptions is null) {
  //        _DefaultJsonSerializerOptions = new JsonSerializerOptions() {
  //          WriteIndented = true,
  //          NumberHandling = JsonNumberHandling.Strict
  //        };
  //        _DefaultJsonSerializerOptions.Converters.Add(new TUserTokenJsonConverter());
  //      }
  //      return _DefaultJsonSerializerOptions;
  //    }
  //  }
  //  set {
  //    lock (_DefaultJsonSerializerOptionsLock) {
  //      _DefaultJsonSerializerOptions = value;
  //    }
  //  }
  //}
  //private static JsonSerializerOptions? _DefaultJsonSerializerOptions;
  //private static readonly object _DefaultJsonSerializerOptionsLock = new object();

  //public TUserToken ParseJson(string source) {
  //  return ParseJson(source, DefaultJsonSerializerOptions);
  //}

//  public TUserToken ParseJson(string source, JsonSerializerOptions options) {
//    #region === Validate parameters ===
//    if (string.IsNullOrWhiteSpace(source)) {
//      throw new JsonException("Json source is null");
//    }
//    #endregion === Validate parameters ===

//    TUserToken? Deserialized = JsonSerializer.Deserialize<TUserToken>(source, options);
//    if (Deserialized is null) {
//      string Error = $"Unable to deserialize json string \"{source}\"";
//      LogError(Error);
//      throw new JsonException(Error);
//}

//    Token = Deserialized.Token;
//    Expiration  = Deserialized.Expiration;

//    return this;
//  }

  //public static TUserToken? FromJson(string source) {
  //  if (string.IsNullOrWhiteSpace(source)) {
  //    throw new ArgumentNullException(nameof(source));
  //  }
  //  return JsonSerializer.Deserialize<TUserToken>(source, DefaultJsonSerializerOptions);
  //}

  //public static TUserToken? FromJson(string source, JsonSerializerOptions options) {
  //  if (string.IsNullOrWhiteSpace(source)) {
  //    throw new ArgumentNullException(nameof(source));
  //  }
  //  return JsonSerializer.Deserialize<TUserToken>(source, options);
  //}

  public static TUserToken ExpiredUserToken => new TUserToken("", DateTime.MinValue);
}
