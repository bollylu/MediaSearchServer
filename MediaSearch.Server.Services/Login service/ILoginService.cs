namespace MediaSearch.Server.Services;
public interface ILoginService {

  IUserAccount? Login(TUserAccountSecret user);
  bool Logout(TUserAccountSecret user);

  bool IsUserLoggedIn(TUserAccountSecret user);

  TUserToken GetToken(TUserAccountSecret user);
  IUserToken RenewToken(IUserToken userToken);

}
