using BLTools.Text;

namespace MediaSearch.Client.Services;
public class TLoginService : ALoggable, ILoginService {

  public IApiServer ApiServer { get; set; } = new TApiServer();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TLoginService() {
    SetLogger(GlobalSettings.GlobalLogger);
  } 
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public async Task<bool> Login(IUserAccountSecret user) {

    if (user is null) {
      return false;
    }

    try {

      string RequestUrl = "login";

      using (CancellationTokenSource Timeout = new CancellationTokenSource(GlobalSettings.HTTP_TIMEOUT_IN_MS)) {

        string? Result = await ApiServer.GetStringAsync(RequestUrl, user.ToJson(), CancellationToken.None).ConfigureAwait(false);

        LogDebugEx(Result?.ToString().BoxFixedWidth($"Login result", GlobalSettings.DEBUG_BOX_WIDTH));
        return true;

      }
    } catch (Exception ex) {
      LogError($"Unable to log in server : {ex.Message}");
      if (ex.InnerException is not null) {
        LogError($"  Inner exception : {ex.InnerException.Message}");
        LogError($"  Inner call stack : {ex.InnerException.StackTrace}");
      }
      return false;
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
