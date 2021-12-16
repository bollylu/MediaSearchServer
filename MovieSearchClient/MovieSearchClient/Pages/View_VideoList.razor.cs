using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

using MovieSearchClientServices;

using BLTools.Diagnostic.Logging;

using Microsoft.AspNetCore.Components;
using MovieSearchModels;
using MovieSearchClientModels;

namespace MovieSearchClient.Pages {
  public partial class View_VideoList : ComponentBase, ILoggable {

    [Inject]
    public IMovieService MovieService { get; set; }

    [Parameter]
    public string Filter { get; set; }

    [Parameter]
    public EViewType ViewType { get; set; } = EViewType.List;

    public IMoviesPage Movies { get; set; }

    public string MoviesTitle {
      get {
        return $"{Movies.AvailableMovies} movies";
      }
    }

    public int CurrentPage { get; set; }

    public string Pagination => $"{CurrentPage}/{Movies.AvailablePages}";

    public bool BackDisabled => CurrentPage == 1;
    public bool NextDisabled => CurrentPage == Movies.AvailablePages;

    #region --- ILoggable --------------------------------------------
    public ILogger Logger { get; set; }
    public void SetLogger(ILogger logger) {
      Logger = ALogger.Create(logger);
    }
    #endregion --- ILoggable --------------------------------------------

    protected override async Task OnInitializedAsync() {
      SetLogger(new TConsoleLogger());
      Logger.Log("Initialized async");
      Movies = await MovieService.GetMovies(Filter);
      CurrentPage = 1;
    }

    protected override async Task OnParametersSetAsync() {
      Logger.Log("Parameters set async");
      Movies = await MovieService.GetMovies(Filter);
      CurrentPage = 1;
      StateHasChanged();
    }

    private async Task RefreshHome() {
      Movies = await MovieService.GetMovies("");
      CurrentPage = 1;
      StateHasChanged();
    }

    private async Task RefreshPrevious() {
      if (CurrentPage > 1) {
        CurrentPage--;
      }
      Movies = await MovieService.GetMovies("", CurrentPage);
      StateHasChanged();
    }

    private async Task RefreshNext() {
      if (CurrentPage < Movies.AvailablePages) {
        CurrentPage++;
      }
      Movies = await MovieService.GetMovies("", CurrentPage);
      StateHasChanged();
    }

    private async Task RefreshEnd() {
      CurrentPage = Movies.AvailablePages;
      Movies = await MovieService.GetMovies("", CurrentPage);
      StateHasChanged();
    }

    private void ViewList() {
      ViewType = EViewType.List;
      StateHasChanged();
    }

    private void ViewCard() {
      ViewType = EViewType.Card;
      StateHasChanged();
    }
  }
}
