namespace MediaSearch.Server.Services;
public class TLoginService : ILoginService {

  #region --- ILoggable --------------------------------------------
  public void SetLogger(ILogger logger) {
    Logger = ALogger.Create(logger);
  }

  public ILogger Logger { get; set; }
  #endregion --- ILoggable --------------------------------------------

  private readonly List<IUserAccount> _UserAccounts = new();
  private readonly ReaderWriterLockSlim _LockUserAccounts = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TLoginService() {
    Logger = ALogger.Create(GlobalSettings.GlobalLogger);
    _Initialize();
  }

  private bool _IsInitialized = false;
  private bool _IsInitializing = false;
  protected void _Initialize() {
    if (_IsInitialized) {
      return;
    }
    if (_IsInitializing) {
      return;
    }
    _IsInitializing = true;

    _UserAccounts.Clear();
    _UserAccounts.Add(new TUserAccount() { Name = "bollylu", Description = "Administrator", Password = "xxx" });
    _UserAccounts.Add(new TUserAccount() { Name = "brilly", Description = "User", Password = "xxx" });
    _IsInitializing = false;
    _IsInitialized = true;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public IUserAccount? Login(TUserAccountSecret user) {
    try {
      _LockUserAccounts.EnterWriteLock();
      int UserIndex = _UserAccounts.FindIndex(u => ((IUserAccountSecret)u).Name == user.Name);
      if (UserIndex < 0) {
        return null;
      }
      if (_UserAccounts[UserIndex].Password == user.Password) {
        _UserAccounts[UserIndex].LastSuccessfulLogin = DateTime.Now;
        _UserAccounts[UserIndex].Token = new TUserToken();
        return _UserAccounts[UserIndex];
      } else {
        _UserAccounts[UserIndex].LastFailedLogin = DateTime.Now;
        return null;
      }

    } finally {
      _LockUserAccounts.ExitWriteLock();
    }
  }

  public bool Login(TUserToken token) {
    try {
      _LockUserAccounts.EnterReadLock();
      IUserAccount? UserAccount = _UserAccounts.SingleOrDefault(u => u.Token.Token == token.Token);
      if (UserAccount is null) {
        return false;
      }
      if (UserAccount.Token.IsExpired) {
        return false;
      }
      return true;
    } finally {
      _LockUserAccounts.ExitReadLock();
    }
  }
  public bool Logout(TUserAccountSecret user) {
    throw new NotImplementedException();
  }

  public bool IsUserLoggedIn(TUserAccountSecret user) {
    throw new NotImplementedException();
  }

  public TUserToken GetToken(TUserAccountSecret user) {
    throw new NotImplementedException();
  }

  public TUserToken RenewToken(TUserAccountSecret user) {
    throw new NotImplementedException();
  }

}
