using System.Net;

namespace MediaSearch.Models;

public class TUserAccountInfo : IUserAccountInfo, IJson {

  public string Name { get; set; } = "";

  public string Description { get; set; } = "";

  public IPAddress RemoteIp { get; set; } = IPAddress.Loopback;

  public DateTime LastSuccessfulLogin { get; set; } = DateTime.MinValue;

  public DateTime LastFailedLogin { get; set; } = DateTime.MinValue;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TUserAccountInfo() { }

  public TUserAccountInfo(IUserAccount user) {
    Name = user.Name;
    Description = user.Description;
    RemoteIp = user.RemoteIp;
    LastSuccessfulLogin = user.LastSuccessfulLogin;
    LastFailedLogin = user.LastFailedLogin;
  }

  public TUserAccountInfo(IUserAccountInfo user) {
    Name = user.Name;
    Description = user.Description;
    RemoteIp = user.RemoteIp;
    LastSuccessfulLogin = user.LastSuccessfulLogin;
    LastFailedLogin = user.LastFailedLogin;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------


  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"User account : {nameof(Name)}={Name}");
    RetVal.AppendLine($"{nameof(Description)}={Description}");
    RetVal.AppendLine($"{nameof(RemoteIp)}={RemoteIp}");
    RetVal.AppendLine($"{nameof(LastSuccessfulLogin)}={LastSuccessfulLogin}");
    RetVal.AppendLine($"{nameof(LastFailedLogin)}={LastFailedLogin}");
    return RetVal.ToString();
  }
}
