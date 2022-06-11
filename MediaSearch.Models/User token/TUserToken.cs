using BLTools.Encryption;

namespace MediaSearch.Models;

public class TUserToken : IUserToken {
  public const int SESSION_DURATION_IN_MIN = 15;
  public const string TOKEN_NONE = "(none)";

  [JsonPropertyName(nameof(TokenId))]
  public string TokenId { get; set; } = TOKEN_NONE;

  [JsonPropertyName(nameof(Expiration))]
  public DateTime Expiration { get; set; } = DateTime.MaxValue;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TUserToken() {
    TokenId = Random.Shared.NextInt64().ToString().HashToBase64();
    Expiration = DateTime.UtcNow.AddMinutes(SESSION_DURATION_IN_MIN);
  }
  public TUserToken(string token) {
    TokenId = token;
  }
  public TUserToken(string token, DateTime expiration) {
    TokenId = token;
    Expiration = expiration;
  }
  public TUserToken(IUserToken userToken) {
    TokenId = userToken.TokenId;
    Expiration = userToken.Expiration;
  }

  public void Duplicate(IUserToken userToken) {
    TokenId = userToken.TokenId;
    Expiration = userToken.Expiration;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------


  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.Append($"{nameof(TokenId)}={TokenId}");
    RetVal.Append($", {nameof(Expiration)}={Expiration.ToYMDHMS()}");
    return RetVal.ToString();
  }

  public static TUserToken ExpiredUserToken => new TUserToken(TOKEN_NONE, DateTime.MinValue);
}
