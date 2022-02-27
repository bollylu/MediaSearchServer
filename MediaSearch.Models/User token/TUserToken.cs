using BLTools;
using BLTools.Encryption;

namespace MediaSearch.Models;

public class TUserToken : IUserToken, IJson<TUserToken> {
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


  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.Append($"{nameof(Token)} = {Token}");
    RetVal.Append($", {nameof(Expiration)} = {Expiration.ToYMDHMS()}");
    return RetVal.ToString();
  }

  public static TUserToken ExpiredUserToken => new TUserToken("", DateTime.MinValue);
}
