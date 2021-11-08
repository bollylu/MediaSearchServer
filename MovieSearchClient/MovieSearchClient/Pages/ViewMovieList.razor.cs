using MovieSearch.Models;
using System.Linq;
using BLTools;
using BLTools.Diagnostic.Logging;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using MovieSearchClient.Services;

namespace MovieSearchClient.Pages {
  public partial class ViewMovieList : ComponentBase, ILoggable {

    private const string ROOT_GROUP = "/";
    public const string SVC_NAME = "groups";

    [Inject]
    public IMovieService MovieService { get; set; }

    [Inject]
    public IBusService<string> BusService { get; set; }



    public IMovies Movies { get; set; }


    #region --- ILoggable --------------------------------------------
    public ILogger Logger { get; set; }
    public void SetLogger(ILogger logger) {
      Logger = ALogger.Create(logger);
    }
    #endregion --- ILoggable --------------------------------------------

    protected override async Task OnInitializedAsync() {
      SetLogger(ALogger.SYSTEM_LOGGER);
      Movies = await MovieService.GetMovies("");

      if (Movies is null) {
        return;
      }

      BusService.SendMessage(SVC_NAME, Movies.Name);
    }


    private async Task Refresh(string name = "") {
      Logger?.Log($"Called Refresh, name=[{name ?? ""}]");
      //if (name == "<back>") {
      //  name = SelectedGroup.BeforeLast('/');
      //  if (name == "") {
      //    name = ROOT_GROUP;
      //  }
      //}

      //SelectedGroup = name;
      //BusService.SendMessage(SVC_NAME, SelectedGroup);

      //MovieGroups = await MovieService.GetGroups(name);
      //if (MovieGroups == null) {
      //  return;
      //}
    }



  }
}
