using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Pages;

public partial class About : ComponentBase, IMediaSearchLoggable<About> {

  TAbout? AboutClient { get; set; }
  TAbout? AboutClientServices { get; set; }
  TAbout? AboutModels { get; set; }
  TAbout? AboutServer { get; set; }
  TAbout? AboutServerServices { get; set; }

  [Inject]
  public IAboutService AboutService {
    get {
      if (_AboutService is null) {
        Logger.LogFatal("Movie service is missing");
        throw new ApplicationException("MovieService");
      }
      return _AboutService;
    }
    set {
      _AboutService = value;
    }
  }
  private IAboutService? _AboutService;

  [Inject]
  public NavigationManager? NavigationManager {get; set; }

  protected override async Task OnInitializedAsync() {
    if (GlobalSettings.Account is null) {
      NavigationManager?.NavigateTo("/login", false, true);
    }
    AboutClient = MediaSearch.Client.GlobalSettings.ExecutingAbout;
    AboutClientServices = MediaSearch.Client.Services.GlobalSettings.ExecutingAbout;
    AboutModels = MediaSearch.Models.GlobalSettings.ExecutingAbout;
    await AboutClient.Initialize();
    await AboutClientServices.Initialize();
    await AboutModels.Initialize();

    AboutServer = await AboutService.GetAboutAsync("server");
    AboutServerServices = await AboutService.GetAboutAsync("serverservices");
  }

  #region --- ILoggable --------------------------------------------
  public IMediaSearchLogger<About> Logger { get;  } = GlobalSettings.LoggerPool.GetLogger<About>();
  #endregion --- ILoggable --------------------------------------------
}
