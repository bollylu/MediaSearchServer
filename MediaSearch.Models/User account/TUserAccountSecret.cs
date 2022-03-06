namespace MediaSearch.Models;
public class TUserAccountSecret : IUserAccountSecret, IJson<TUserAccountSecret> {

  public string Name { get; set; } = "";

  public string Password { get; set; } = "";

  public bool MustChangePassword { get; set; } = false;

  public IUserToken Token { get; set; } = TUserToken.ExpiredUserToken;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TUserAccountSecret() { }
  public TUserAccountSecret(IUserAccountSecret secret) {
    Name = secret.Name;
    Password = secret.Password;
    MustChangePassword = secret.MustChangePassword;
    Token = new TUserToken(secret.Token);
  }

  public TUserAccountSecret(IUserAccount user) {
    Name = user.Name;
    Password = user.Secret.Password;
    MustChangePassword = user.Secret.MustChangePassword;
    Token = new TUserToken(user.Secret.Token);
  }

  public void Duplicate(IUserAccountSecret secret) {
    Name = secret.Name;
    Password = secret.Password;
    MustChangePassword = secret.MustChangePassword;
    Token.Duplicate(secret.Token);
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.Append($"{nameof(Name)}={Name}");
    RetVal.Append($", {nameof(Password)}={Password}");
    RetVal.Append($", {nameof(MustChangePassword)}={MustChangePassword}");
    RetVal.Append($", {nameof(Token)}={Token}");
    return RetVal.ToString();
  }
}
