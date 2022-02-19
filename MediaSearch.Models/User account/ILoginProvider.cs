namespace MediaSearch.Models;

public interface ILoginProvider {
  Task<bool> Login(IUserAccountSecret user);
  Task<bool> Logout(IUserAccountSecret user);
  Task<bool> IsUserLoggedIn(IUserAccountSecret user);
}
