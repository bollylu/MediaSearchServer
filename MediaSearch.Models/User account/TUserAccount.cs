using System.Net;

namespace MediaSearch.Models;

public class TUserAccount : IUserAccount {

  #region --- IName --------------------------------------------
  public string Name { get; set; } = "";

  public string Description { get; set; } = "";
  #endregion --- IName --------------------------------------------

  #region --- IUserAccountInfo --------------------------------------------
  public IPAddress RemoteIp { get; set; } = IPAddress.Loopback;

  public DateTime LastSuccessfulLogin { get; set; } = DateTime.MinValue;

  public DateTime LastFailedLogin { get; set; } = DateTime.MinValue;
  #endregion --- IUserAccountInfo --------------------------------------------

  #region --- IUserAccountSecret --------------------------------------------
  public IUserAccountSecret Secret { get; } = new TUserAccountSecret();
  #endregion --- IUserAccountSecret --------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TUserAccount() { }

  public TUserAccount(IUserAccountSecret secret) {
    Secret = new TUserAccountSecret(secret);
    Name = secret.Name;
  }

  public TUserAccount(IUserAccount user) {
    Name = user.Name;
    Description = user.Description;
    RemoteIp = user.RemoteIp;
    LastSuccessfulLogin = user.LastSuccessfulLogin;
    LastFailedLogin = user.LastFailedLogin;
    Secret = new TUserAccountSecret(user.Secret);
  }

  public TUserAccount(IUserAccountInfo user) {
    Name = user.Name;
    Description = user.Description;
    RemoteIp = user.RemoteIp;
    LastSuccessfulLogin = user.LastSuccessfulLogin;
    LastFailedLogin = user.LastFailedLogin;
    Secret = new TUserAccountSecret();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------


  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"User account : {nameof(Name)}={Name}");
    RetVal.AppendLine($"{nameof(Description)}={Description}");
    RetVal.AppendLine($"{nameof(RemoteIp)}={RemoteIp}");
    RetVal.AppendLine($"{nameof(LastSuccessfulLogin)}={LastSuccessfulLogin}");
    RetVal.AppendLine($"{nameof(LastFailedLogin)}={LastFailedLogin}");
    RetVal.AppendLine($"{nameof(Secret)}={Secret}");
    return RetVal.ToString();
  }
}
