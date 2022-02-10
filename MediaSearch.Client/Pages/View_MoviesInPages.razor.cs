using BLTools.Text;

using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Pages {
  public partial class View_MoviesInPages : ComponentBase, ILoggable {

    [Inject]
    public IMovieService MovieService {
      get {
        if (_MovieService is null) {
          Logger.LogFatal("Movie service is missing");
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

    public string Pagination => $"{Filter.Page}/{_AvailablePages}";

    public bool BackDisabled => Filter.Page == 1;
    public bool NextDisabled => Filter.Page == _AvailablePages;

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public View_MoviesInPages() : base() {
      SetLogger(GlobalSettings.GlobalLogger);
    } 
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    #region --- ILoggable --------------------------------------------
    public ILogger Logger { get; set; } = GlobalSettings.GlobalLogger;
    public void SetLogger(ILogger logger) {
      Logger = ALogger.Create(logger);
    }
    #endregion --- ILoggable --------------------------------------------

    private TFilter _OldFilter = new TFilter();
    private int _OldPage = int.MaxValue;

    protected override void OnInitialized() {
      Logger.LogDebugEx("Initialized");
    }

    protected override async Task OnParametersSetAsync() {
      if (_OldPage != Filter.Page || Filter != _OldFilter) {
        Logger.LogDebugEx(Filter.ToString().BoxFixedWidth("New filter", GlobalSettings.DEBUG_BOX_WIDTH));
        _OldPage = Filter.Page;
        _OldFilter = new TFilter(Filter);
        MoviesPage = await MovieService.GetMoviesPage(Filter);
      }
    }

    private async Task RefreshHome() {
      Filter.FirstPage();
      MoviesPage = await MovieService.GetMoviesPage(Filter);
      StateHasChanged();
    }

    private async Task RefreshPrevious() {
      Logger.LogDebugEx("Previous page");
      if (Filter.Page > 1) {
        Filter.PreviousPage();
      }
      MoviesPage = await MovieService.GetMoviesPage(Filter);
      StateHasChanged();
    }

    private async Task RefreshNext() {
      Logger.LogDebugEx("Next page");
      if (Filter.Page < _AvailablePages) {
        Filter.NextPage();
      }
      MoviesPage = await MovieService.GetMoviesPage(Filter);
      StateHasChanged();
    }

    private async Task RefreshEnd() {
      Filter.SetPage(_AvailablePages);
      MoviesPage = await MovieService.GetMoviesPage(Filter);
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
