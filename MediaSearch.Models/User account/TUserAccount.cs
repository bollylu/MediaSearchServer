using System.Net;

namespace MediaSearch.Models;

public class TUserAccount : ADataModel, IUserAccount, IJson<TUserAccount> {

  #region --- IName --------------------------------------------
  [JsonPropertyName(nameof(Name))]
  public string Name { get; set; } = "";

  [JsonPropertyName(nameof(Description))]
  public string Description { get; set; } = "";
  #endregion --- IName --------------------------------------------

  #region --- IUserAccountInfo --------------------------------------------
  [JsonPropertyName(nameof(RemoteIp))]
  public IPAddress RemoteIp { get; set; } = IPAddress.Loopback;

  [JsonPropertyName(nameof(LastSuccessfulLogin))]
  public DateTime LastSuccessfulLogin { get; set; } = DateTime.MinValue;

  [JsonPropertyName(nameof(LastFailedLogin))]
  public DateTime LastFailedLogin { get; set; } = DateTime.MinValue;
  #endregion --- IUserAccountInfo --------------------------------------------

  #region --- IUserAccountSecret --------------------------------------------
  [JsonPropertyName(nameof(Password))]
  public string Password { get; set; } = "";

  [JsonPropertyName(nameof(MustChangePassword))]
  public bool MustChangePassword { get; set; } = false;

  [JsonPropertyName(nameof(Token))]
  public IUserToken Token { get; set; } = TUserToken.ExpiredUserToken;

  #endregion --- IUserAccountSecret --------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TUserAccount() {
    SetLogger(GlobalSettings.GlobalLogger);
  }
  public TUserAccount(IUserAccountSecret user) : this() {
    Name = user.Name;
    MustChangePassword = user.MustChangePassword;
    Password = user.Password;
    Token = new TUserToken(user.Token);
    //IJson.DefaultJsonSerializerOptions.Converters.Add(new TUserAccountJsonConverter());
    //IJson.DefaultJsonSerializerOptions.Converters.Add(new TUserTokenJsonConverter());
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
  //        _DefaultJsonSerializerOptions.Converters.Add(new TUserAccountConverter());
  //        _DefaultJsonSerializerOptions.Converters.Add(new TUserTokenConverter());
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

  //public TUserAccount ParseJson(string source) {
  //  return ParseJson(source, IJson.DefaultJsonSerializerOptions);
  //}

  //public TUserAccount ParseJson(string source, JsonSerializerOptions options) {
  //  #region === Validate parameters ===
  //  if (string.IsNullOrWhiteSpace(source)) {
  //    throw new JsonException("Json source is null");
  //  }
  //  #endregion === Validate parameters ===

  //  TUserAccount? Deserialized = JsonSerializer.Deserialize<TUserAccount>(source, options);
  //  if (Deserialized is null) {
  //    string Error = $"Unable to deserialize json string \"{source}\"";
  //    LogError(Error);
  //    throw new JsonException(Error);
  //  }

  //  Name = Deserialized.Name;
  //  Description = Deserialized.Description;
  //  MustChangePassword = Deserialized.MustChangePassword;
  //  Password = Deserialized.Password;
  //  Token = Deserialized.Token;

  //  return this;
  //}

  //public static TUserAccount? FromJson(string source) {
  //  throw new NotImplementedException();
  //}

  //public static TUserAccount? FromJson(string source, JsonSerializerOptions options) {
  //  throw new NotImplementedException();
  //}
}
