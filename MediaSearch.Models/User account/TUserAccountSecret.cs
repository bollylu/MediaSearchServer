namespace MediaSearch.Models;
public class TUserAccountSecret : ADataModel, IUserAccountSecret, IJson<TUserAccountSecret> {

  [JsonPropertyName(nameof(Name))]
  public string Name { get; set; } = "";

  [JsonPropertyName(nameof(Password))]
  public string Password { get; set; } = "";

  [JsonPropertyName(nameof(MustChangePassword))]
  public bool MustChangePassword { get; set; }

  [JsonPropertyName(nameof(Token))]
  public IUserToken Token { get; set; } = TUserToken.ExpiredUserToken;

  //public string ToJson() {
  //  return JsonSerializer.Serialize(this, DefaultJsonSerializerOptions);
  //}

  //public string ToJson(JsonSerializerOptions options) {
  //  return JsonSerializer.Serialize(this, options);
  //}

  //public TUserAccountSecret ParseJson(string source) {
  //  return ParseJson(source, DefaultJsonSerializerOptions);
  //}

  //public static JsonSerializerOptions DefaultJsonSerializerOptions {
  //  get {
  //    lock (_DefaultJsonSerializerOptionsLock) {
  //      if (_DefaultJsonSerializerOptions is null) {
  //        _DefaultJsonSerializerOptions = new JsonSerializerOptions() {
  //          WriteIndented = true,
  //          NumberHandling = JsonNumberHandling.Strict
  //        };
  //        _DefaultJsonSerializerOptions.Converters.Add(new TUserAccountSecretJsonConverter());
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



  //public TUserAccountSecret ParseJson(string source, JsonSerializerOptions options) {
  //  #region === Validate parameters ===
  //  if (string.IsNullOrWhiteSpace(source)) {
  //    throw new JsonException("Json source is null");
  //  }
  //  #endregion === Validate parameters ===

  //  TUserAccountSecret? Deserialized = JsonSerializer.Deserialize<TUserAccountSecret>(source, options);
  //  if (Deserialized is null) {
  //    string Error = $"Unable to deserialize json string \"{source}\"";
  //    LogError(Error);
  //    throw new JsonException(Error);
  //  }

  //  Name = Deserialized.Name;
  //  Password = Deserialized.Password;
  //  MustChangePassword = Deserialized.MustChangePassword;
  //  Token = new TUserToken(Deserialized.Token);

  //  return this;
  //}

  //public static TUserAccountSecret? FromJson(string source) {
  //  if (string.IsNullOrWhiteSpace(source)) {
  //    throw new ArgumentNullException(nameof(source));
  //  }
  //  return JsonSerializer.Deserialize<TUserAccountSecret>(source, DefaultJsonSerializerOptions);
  //}

  //public static TUserAccountSecret? FromJson(string source, JsonSerializerOptions options) {
  //  if (string.IsNullOrWhiteSpace(source)) {
  //    throw new ArgumentNullException(nameof(source));
  //  }
  //  return JsonSerializer.Deserialize<TUserAccountSecret>(source, options);
  //}
}
