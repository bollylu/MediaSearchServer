namespace MediaSearch.Client.Services;

public class TLoginService : ILoginService, ILoggable {

  public IApiServer ApiServer { get; set; } = new TApiServer();

  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TLoginService>();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TLoginService() { }
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
        Logger.LogDebugExBox("Login result", Result);

        return Result;

      }
    } catch (Exception ex) {
      Logger.LogError($"Unable to log in server : {ex.Message}");
      if (ex.InnerException is not null) {
        Logger.LogError($"  Inner exception : {ex.InnerException.Message}");
        Logger.LogError($"  Inner call stack : {ex.InnerException.StackTrace}");
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
