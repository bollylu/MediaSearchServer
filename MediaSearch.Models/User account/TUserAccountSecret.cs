namespace MediaSearch.Models;
public class TUserAccountSecret : IUserAccountSecret, IJson<TUserAccountSecret> {

  public string Name { get; set; } = "";

  public string PasswordHash { get; set; } = "";

  public bool MustChangePassword { get; set; } = false;

  public IUserToken Token { get; set; } = TUserToken.ExpiredUserToken;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TUserAccountSecret() { }
  public TUserAccountSecret(IUserAccountSecret secret) {
    Name = secret.Name;
    PasswordHash = secret.PasswordHash;
    MustChangePassword = secret.MustChangePassword;
    Token = new TUserToken(secret.Token);
  }

  public TUserAccountSecret(IUserAccount user) {
    Name = user.Name;
    PasswordHash = user.Secret.PasswordHash;
    MustChangePassword = user.Secret.MustChangePassword;
    Token = new TUserToken(user.Secret.Token);
  }

  public void Duplicate(IUserAccountSecret secret) {
    Name = secret.Name;
    PasswordHash = secret.PasswordHash;
    MustChangePassword = secret.MustChangePassword;
    Token.Duplicate(secret.Token);
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.Append($"{nameof(Name)}={Name}");
    RetVal.Append($", {nameof(PasswordHash)}={PasswordHash}");
    RetVal.Append($", {nameof(MustChangePassword)}={MustChangePassword}");
    RetVal.Append($", {nameof(Token)}={{{Token}}}");
    return RetVal.ToString();
  }
}
