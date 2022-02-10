using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace MediaSearch.Client.Pages;

public partial class About : ComponentBase, ILoggable {

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

  string[] AssembliesList = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetName().Name ?? "-").ToArray();

  protected override async Task OnInitializedAsync() {
    AboutClient = new TAbout(AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name == "MediaSearch.Client"));
    AboutClientServices = new TAbout(AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name == "MediaSearch.Client.Services"));
    AboutModels = new TAbout(AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name == "MediaSearch.Models"));
    await AboutClient.Initialize();
    await AboutClientServices.Initialize();
    await AboutModels.Initialize();

    AboutServer = await AboutService.GetAboutAsync("server");
    AboutServerServices = await AboutService.GetAboutAsync("serverservices");
  }

  #region --- ILoggable --------------------------------------------
  public ILogger Logger { get; set; } = GlobalSettings.GlobalLogger;
  public void SetLogger(ILogger logger) {
    Logger = ALogger.Create(logger);
  }
  #endregion --- ILoggable --------------------------------------------
}
