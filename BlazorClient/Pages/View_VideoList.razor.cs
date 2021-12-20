using MovieSearchClientServices;

using Microsoft.AspNetCore.Components;
using MovieSearchModels;
using MovieSearchClientModels;

namespace BlazorClient.Pages {
  public partial class View_VideoList : ComponentBase, ILoggable {

    [Inject]
    public IMovieService MovieService { get; set; }

    [Parameter]
    public SearchFilter Filter { get; set; }

    [Parameter]
    public EViewType ViewType { get; set; } = EViewType.List;

    public IMoviesPage Movies { get; set; }

    public string MoviesTitle {
      get {
        return $"{(Movies is null ? 0 : Movies.AvailableMovies)} movies";
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
      Logger?.Log("Initialized async");
      Movies = await MovieService.GetMovies(Filter.NameFilter);
      CurrentPage = 1;
    }

    protected override async Task OnParametersSetAsync() {
      Logger.Log("Parameters set async");
      CurrentPage = 1;
      Movies = await MovieService.GetMovies(Filter.NameFilter, Filter.Days, CurrentPage);
      StateHasChanged();
    }

    private async Task RefreshHome() {
      CurrentPage = 1;
      Movies = await MovieService.GetMovies(Filter.NameFilter, Filter.Days, CurrentPage);
      StateHasChanged();
    }

    private async Task RefreshPrevious() {
      if (CurrentPage > 1) {
        CurrentPage--;
      }
      Movies = await MovieService.GetMovies(Filter.NameFilter, Filter.Days, CurrentPage);
      StateHasChanged();
    }

    private async Task RefreshNext() {
      if (CurrentPage < Movies.AvailablePages) {
        CurrentPage++;
      }
      Movies = await MovieService.GetMovies(Filter.NameFilter, Filter.Days, CurrentPage);
      StateHasChanged();
    }

    private async Task RefreshEnd() {
      CurrentPage = Movies.AvailablePages;
      Movies = await MovieService.GetMovies(Filter.NameFilter, Filter.Days, CurrentPage);
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

    private string _GetSizeForDisplay(long size) {
      const long KB = 1024;
      const long MB = 1024 * KB;
      const long GB = 1024 * MB;
      if (size > GB) {
        double Size = (double)size / (double)GB;
        return $"{Size:N3} GB";
      }
      if (size > MB) {
        double Size = (double)size / (double)MB;
        return $"{Size:N3} MB";
      }
      if (size > KB) {
        double Size = (double)size / (double)KB;
        return $"{Size:N3} KB";
      }
      return size.ToString();
    }
  }
}
