namespace MediaSearch.Server.Services;
public interface ILoginService : ILoggable {

  IUserAccount? Login(TUserAccountSecret user);
  bool Logout(TUserAccountSecret user);

  bool IsUserLoggedIn(TUserAccountSecret user);

  TUserToken GetToken(TUserAccountSecret user);
  TUserToken RenewToken(TUserAccountSecret user);

}
