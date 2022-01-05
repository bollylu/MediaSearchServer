using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Components {
  public partial class Movie_SmallCard : ComponentBase, ILoggable {

    [Parameter]
    public IMovie Movie {
      get {
        return _Movie ?? new TMovie();
      }
      set {
        _Movie = value;
      }
    }
    private IMovie? _Movie;

    [Parameter]
    public int Page { get; set; }

    private int LastPage = 0;

    public string? PictureData { get; set; }

    private string AltNames {
      get {
        if (Movie.AltNames.IsEmpty()) {
          return string.Empty; ;
        }
        return string.Join("\n", Movie.AltNames.Select(n => $"{n.Key}:{n.Value}"));
      }
    }

    [Inject]
    public IMovieService? MovieService { get; set; }

    public ILogger Logger { get; set; } = new TConsoleLogger() { SeverityLimit = ESeverity.Debug };
    public void SetLogger(ILogger logger) {
      throw new NotImplementedException();
    }

    public string? HeaderText => Movie.Name ?? "";
    public string? BodyTitle => AltNames;
    public string? BodySubTitle => Movie.Group;
    public string? BodyText => string.Join(", ", Movie.Tags);
    public string? FooterLink { get; set; }
    public string? ImageWidth { get; set; }

    private List<CancellationTokenSource> _CancellationTokenSources = new();

    protected override async Task OnParametersSetAsync() {
      Logger.LogDebugEx($"{_CancellationTokenSources.Count} tasks awaiting, Page={Page}, LastPage={LastPage}");
      if (Page != LastPage) {
        LastPage = Page;
        foreach (CancellationTokenSource CancellationtokenSourceItem in _CancellationTokenSources) {
          CancellationtokenSourceItem.Cancel();
        }
        _CancellationTokenSources.Clear();
      }
      IsPictureLoaded = false;
      if (MovieService is not null) {
        CancellationTokenSource Cancellation = new CancellationTokenSource();
        Task GetImageTask = GetImage(Cancellation.Token);
        _CancellationTokenSources.Add(Cancellation);
        await GetImageTask.ConfigureAwait(false);
      }
    }

    protected override async Task OnInitializedAsync() {
      await base.OnInitializedAsync();
    }

    private async Task GetImage(CancellationToken cancelToken) {
      if (MovieService is not null && !IsPictureLoaded) {
        StateHasChanged();
        PictureData = await MovieService.GetPicture64(Movie, cancelToken);
        IsPictureLoaded = true;
        StateHasChanged();
      }
    }

    public bool IsPictureLoaded = false;

  }

}
