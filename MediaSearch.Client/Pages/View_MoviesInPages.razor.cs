using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Pages {
  public partial class View_MoviesInPages : ComponentBase, ILoggable {

    [Inject]
    public IMovieService MovieService {
      get {
        if (_MovieService is null) {
          Logger?.LogFatal("Movie service is missing");
          throw new ApplicationException("MovieService");
        }
        return _MovieService;
      }
      set {
        _MovieService = value;
      }
    }
    private IMovieService? _MovieService;

    [Parameter]
    public TFilter Filter {
      get {
        return _Filter ??= new TFilter();
      }
      set {
        _Filter = value;
      }
    }
    private TFilter? _Filter;

    [Parameter]
    public EViewType ViewType { get; set; } = EViewType.List;

    public IMoviesPage? MoviesPage { get; set; }
    private int _AvailableMovies => MoviesPage?.AvailableMovies ?? 0;
    private int _AvailablePages => MoviesPage?.AvailablePages ?? 0;

    public string MoviesTitle => $"{(_AvailableMovies)} movies";

    public int CurrentPage { get; set; } = 1;

    public string Pagination => $"{CurrentPage}/{_AvailablePages}";

    public bool BackDisabled => CurrentPage == 1;
    public bool NextDisabled => CurrentPage == _AvailablePages;

    #region --- ILoggable --------------------------------------------
    public ILogger Logger { get; set; } = new TConsoleLogger() { SeverityLimit = ESeverity.Debug };
    public void SetLogger(ILogger logger) {
      Logger = ALogger.Create(logger);
    }
    #endregion --- ILoggable --------------------------------------------

    private TFilter _OldFilter = new TFilter();
    private int _OldPage = int.MaxValue;

    protected override void OnInitialized() {
      Logger.LogDebug("Initialized");
    }

    protected override async Task OnParametersSetAsync() {
      Logger.LogDebug("Parameters set async");
      if (_OldPage != CurrentPage || Filter != _OldFilter) {
        _OldPage = CurrentPage;
        _OldFilter = Filter;
        MoviesPage = await MovieService.GetMoviesPage(Filter, CurrentPage);
      }
    }

    private async Task RefreshHome() {
      CurrentPage = 1;
      MoviesPage = await MovieService.GetMoviesPage(Filter, CurrentPage);
      StateHasChanged();
    }

    private async Task RefreshPrevious() {
      Logger.LogDebug("Previous page");
      if (CurrentPage > 1) {
        CurrentPage--;
      }
      MoviesPage = await MovieService.GetMoviesPage(Filter, CurrentPage);
      StateHasChanged();
    }

    private async Task RefreshNext() {
      Logger.LogDebug("Next page");
      if (CurrentPage < _AvailablePages) {
        CurrentPage++;
      }
      MoviesPage = await MovieService.GetMoviesPage(Filter, CurrentPage);
      StateHasChanged();
    }

    private async Task RefreshEnd() {
      CurrentPage = _AvailablePages;
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


  }
}
