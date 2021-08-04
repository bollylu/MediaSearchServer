using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

using MovieSearch.Services;
using MovieSearch.Models;

using BLTools.Diagnostic.Logging;

using Microsoft.AspNetCore.Components;



namespace MovieSearch.Pages {
  public partial class Videos : ComponentBase, ILoggable {

    [Inject]
    public IMovieService MovieService { get; set; }

    [Parameter]
    public string Group { get; set; }

    [Parameter]
    public string Filter { get; set; }

    public IMovies Movies { get; set; }

    public string MoviesTitle {
      get {
        if (Movies == null || string.IsNullOrEmpty(Movies.Name) || Movies.Name == "/") {
          return "-- All movies --";
        } else {
          return Movies.Name.Trim('/');
        }
      }
    }

    public int CurrentPage { get; set; }

    public string Pagination => $"{CurrentPage}/{Movies.AvailablePages}";

    public bool BackDisabled => CurrentPage==0;
    public bool NextDisabled => CurrentPage == Movies.AvailablePages;

    #region --- ILoggable --------------------------------------------
    public ILogger Logger { get; set; }
    public void SetLogger(ILogger logger) {
      Logger = ALogger.Create(logger);
    }
    #endregion --- ILoggable --------------------------------------------

    protected override async Task OnInitializedAsync() {
      SetLogger(ALogger.SYSTEM_LOGGER);
      Movies = await MovieService.GetMovies(Group, Filter);
      CurrentPage = 0;
    }

    protected override async Task OnParametersSetAsync() {
      Movies = await MovieService.GetMovies(Group, Filter);
      CurrentPage = 0;
      StateHasChanged();
    }

    private async Task RefreshHome() {
      Movies = await MovieService.GetMovies(Group, "");
      CurrentPage = 0;
      StateHasChanged();
    }

    private async Task RefreshPrevious() {
      if (CurrentPage > 0) {
        CurrentPage--;
      }
      Movies = await MovieService.GetMovies(Group, "", CurrentPage);
      StateHasChanged();
    }

    private async Task RefreshNext() {
      if (CurrentPage < Movies.AvailablePages) {
        CurrentPage++;
      }
      Movies = await MovieService.GetMovies(Group, "", CurrentPage);
      StateHasChanged();
    }

    private async Task RefreshEnd() {
      CurrentPage = Movies.AvailablePages;
      Movies = await MovieService.GetMovies(Group, "", CurrentPage);
      StateHasChanged();
    }
  }
}
