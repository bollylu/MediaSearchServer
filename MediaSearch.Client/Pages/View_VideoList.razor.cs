using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Pages {
  public partial class View_VideoList : ComponentBase, ILoggable {

    [Inject]
    public IMovieService MovieService { get; set; }

    [Parameter]
    public TFilter Filter { get; set; }

    [Parameter]
    public EViewType ViewType { get; set; } = EViewType.List;

    public IMoviesPage MoviesPage { get; set; }

    public string MoviesTitle {
      get {
        return $"{(MoviesPage is null ? 0 : MoviesPage.AvailableMovies)} movies";
      }
    }

    public int CurrentPage { get; set; }

    public string Pagination => $"{CurrentPage}/{MoviesPage.AvailablePages}";

    public bool BackDisabled => CurrentPage == 1;
    public bool NextDisabled => CurrentPage == MoviesPage.AvailablePages;

    #region --- ILoggable --------------------------------------------
    public ILogger Logger { get; set; }
    public void SetLogger(ILogger logger) {
      Logger = ALogger.Create(logger);
    }
    #endregion --- ILoggable --------------------------------------------

    protected override async Task OnInitializedAsync() {
      SetLogger(new TConsoleLogger());
      Logger?.Log("Initialized async");
      MoviesPage = await MovieService.GetMoviesPage(Filter);
      Logger?.Log(MoviesPage.ToString(true));
      CurrentPage = 1;
    }

    protected override async Task OnParametersSetAsync() {
      Logger.Log("Parameters set async");
      CurrentPage = 1;
      MoviesPage = await MovieService.GetMoviesPage(Filter, CurrentPage);
      StateHasChanged();
    }

    private async Task RefreshHome() {
      CurrentPage = 1;
      MoviesPage = await MovieService.GetMoviesPage(Filter, CurrentPage);
      StateHasChanged();
    }

    private async Task RefreshPrevious() {
      if (CurrentPage > 1) {
        CurrentPage--;
      }
      MoviesPage = await MovieService.GetMoviesPage(Filter, CurrentPage);
      StateHasChanged();
    }

    private async Task RefreshNext() {
      if (CurrentPage < MoviesPage.AvailablePages) {
        CurrentPage++;
      }
      MoviesPage = await MovieService.GetMoviesPage(Filter, CurrentPage);
      StateHasChanged();
    }

    private async Task RefreshEnd() {
      CurrentPage = MoviesPage.AvailablePages;
      MoviesPage = await MovieService.GetMoviesPage(Filter, CurrentPage);
      StateHasChanged();
    }

    private void ViewList() {
      ViewType = EViewType.List;
      StateHasChanged();
    }

    private void ViewCard() {
      ViewType = EViewType.SmallCard;
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
