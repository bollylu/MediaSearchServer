namespace MediaSearch.Client.Services;
public class TLoginService : ALoggable, ILoginService {

  public IApiServer ApiServer { get; set; } = new TApiServer();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TLoginService() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TLoginService>();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public async Task<IUserAccountInfo?> Login(IUserAccountSecret user) {

    if (user is null) {
      return null;
    }

    try {

      string RequestUrl = "login";

      using (CancellationTokenSource Timeout = new CancellationTokenSource(GlobalSettings.HTTP_TIMEOUT_IN_MS)) {

        TUserAccountInfo? Result = await ApiServer.GetJsonAsync<IUserAccountSecret, TUserAccountInfo>(RequestUrl, user, CancellationToken.None).ConfigureAwait(false);
        if (Result is null) {
          return null;
        }
        LogDebugExBox("Login result", Result);

        return Result;

      }
    } catch (Exception ex) {
      LogErrorBox("Unable to log in server", ex);
      if (ex.InnerException is not null) {
        LogErrorBox("  Inner exception :", ex.InnerException, 110, "Login", true);
      }
      return null;
    }
  }

  public Task<bool> Logout(IUserAccountSecret user) {
    throw new NotImplementedException();
  }

  public Task<bool> IsUserLoggedIn(IUserAccountSecret user) {
    throw new NotImplementedException();
  }

  public Task<bool> ChangePassword(string oldPassword, string newPassword) {
    throw new NotImplementedException();
  }

  public Task<string> GetToken() {
    throw new NotImplementedException();
  }

  public Task<bool> RenewToken(string token) {
    throw new NotImplementedException();
  }

  public Task<bool> ExpireToken(string token) {
    throw new NotImplementedException();
  }

}
