using MovieSearch.Models;
using System.Linq;
using BLTools;
using BLTools.Diagnostic.Logging;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using MovieSearch.Client.Services;
using MovieSearch.Services;

namespace MovieSearch.Pages {
  public partial class Groups : ComponentBase, ILoggable {

    private const string ROOT_GROUP = "/";
    public const string SVC_NAME = "groups";

    [Inject]
    public IMovieService MovieService { get; set; }

    [Inject]
    public IBusService<string> BusService { get; set; }



    public IMovieGroups MovieGroups { get; set; }

    public string SelectedGroup { get; set; }


    #region --- ILoggable --------------------------------------------
    public ILogger Logger { get; set; }
    public void SetLogger(ILogger logger) {
      Logger = ALogger.Create(logger);
    }
    #endregion --- ILoggable --------------------------------------------

    public bool BackDisabled => string.IsNullOrEmpty(SelectedGroup) || SelectedGroup == "/";



    protected override async Task OnInitializedAsync() {
      SetLogger(ALogger.SYSTEM_LOGGER);
      MovieGroups = await MovieService.GetGroups(ROOT_GROUP);

      if (MovieGroups == null) {
        return;
      }

      SelectedGroup = ROOT_GROUP;
      BusService.SendMessage(SVC_NAME, SelectedGroup);

    }


    private async Task Refresh(string name = "") {
      Logger?.Log($"Called Refresh, name=[{name ?? ""}]");
      if (name == "<back>") {
        name = SelectedGroup.BeforeLast('/');
        if (name == "") {
          name = ROOT_GROUP;
        }
      }

      SelectedGroup = name;
      BusService.SendMessage(SVC_NAME, SelectedGroup);

      MovieGroups = await MovieService.GetGroups(name);
      if (MovieGroups == null) {
        return;
      }
    }



  }
}
