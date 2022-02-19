using System.Net;

namespace MediaSearch.Models;

public class TUserAccount : ADataModel, IUserAccount, IJson<TUserAccount> {

  #region --- IName --------------------------------------------
  //[JsonPropertyName(nameof(Name))]
  public string Name { get; set; } = "";

  //[JsonPropertyName(nameof(Description))]
  public string Description { get; set; } = "";
  #endregion --- IName --------------------------------------------

  #region --- IUserAccountInfo --------------------------------------------
  //[JsonPropertyName(nameof(RemoteIp))]
  public IPAddress RemoteIp { get; set; } = IPAddress.Loopback;

  //[JsonPropertyName(nameof(LastSuccessfulLogin))]
  public DateTime LastSuccessfulLogin { get; set; } = DateTime.MinValue;

  //[JsonPropertyName(nameof(LastFailedLogin))]
  public DateTime LastFailedLogin { get; set; } = DateTime.MinValue;
  #endregion --- IUserAccountInfo --------------------------------------------

  #region --- IUserAccountSecret --------------------------------------------
  //[JsonPropertyName(nameof(Password))]
  public string Password { get; set; } = "";

  //[JsonPropertyName(nameof(MustChangePassword))]
  public bool MustChangePassword { get; set; } = false;

  //[JsonPropertyName(nameof(Token))]
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
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

}
