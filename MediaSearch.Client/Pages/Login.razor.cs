using Microsoft.AspNetCore.Components;
using BLTools.Encryption;

namespace MediaSearch.Client.Pages;

public partial class Login : ComponentBase, IMediaSearchLoggable<Login> {

  public const string PARAM_ORIGIN = "origin";
  public const string ORIGIN_SETTINGS = "settings";
  public const string ORIGIN_ADMIN = "admincontrol";

  public IMediaSearchLogger<Login> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<Login>();

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

  [Inject]
  public NavigationManager? NavigationManager { get; set; }

  public TUserAccountSecret UserAccountSecret { get; } = new TUserAccountSecret();

  public string Password {
    get {
      return _Password ?? string.Empty;
    } set { 
      _Password = value;
      UserAccountSecret.PasswordHash = value.HashToBase64(EHashingMethods.SHA512);
    } }
  private string? _Password;
  protected override void OnInitialized() {
    base.OnInitialized();
    Logger.LogDebugExBox("Uri", NavigationManager?.Uri ?? "");
    string ParametersString = (NavigationManager?.Uri ?? "").AfterLast('/').After('?');
    ISplitArgs Parameters = new SplitArgs();
    Parameters.Parse(ParametersString);
    FromPage = Parameters.GetValue(PARAM_ORIGIN, "");
  }
  public string FromPage { get; set; } = "";

  private async Task DoLogin() {
    if (UserAccountSecret is null || string.IsNullOrWhiteSpace(UserAccountSecret.Name)) {
      return;
    }

    IUserAccountInfo? ConnectedUser = await LoginService.Login(UserAccountSecret);

    if (ConnectedUser is null) {
      GlobalSettings.Account = null;
      return;
    }

    GlobalSettings.Account = new TUserAccountInfo(ConnectedUser);
    
    if (FromPage != "") {
      NavigationManager?.NavigateTo(FromPage, false, true);
      return;
    }
  }

  
}
