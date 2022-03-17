namespace MediaSearch.Client.Services;
 public interface ILoginService {

  Task<IUserAccountInfo?> Login(IUserAccountSecret user);
  Task<bool> Logout(IUserAccountSecret user);
  Task<bool> IsUserLoggedIn(IUserAccountSecret user);

  Task<bool> ChangePassword(string oldPassword, string newPassword);

  Task<string> GetToken();
  Task<bool> RenewToken(string token);
  Task<bool> ExpireToken(string token);

}
