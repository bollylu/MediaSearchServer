using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Pages;

public partial class Login : ComponentBase {

  [Inject]
  public ILoginService LoginService {
    get {
      return _LoginService ?? new TLoginService();
    }
    set {
      _LoginService = value;
    }
  }
  private ILoginService? _LoginService;

  public TUserAccountSecret UserAccountSecret { get; set; } = new TUserAccountSecret();

  private async Task DoLogin() {
    if (UserAccountSecret is null) {
      return;
    }
    if (await LoginService.Login(UserAccountSecret)) {
      GlobalSettings.Account = new TUserAccount(UserAccountSecret);
    }
  }
}
