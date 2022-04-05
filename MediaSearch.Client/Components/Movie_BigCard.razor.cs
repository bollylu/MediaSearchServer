using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace MediaSearch.Client.Components {

  public partial class Movie_BigCard : ComponentBase, IMediaSearchLoggable<Movie_BigCard> {

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
    public string? ImageSource { get; set; }

    private string AltNames {
      get {
        if (Movie.Titles.IsEmpty()) {
          return string.Empty;
          ;
        }
        return string.Join("\n", Movie.Titles.Select(n => $"{n.Key}:{n.Value}"));
      }
    }

    [Inject]
    public IMovieService? MovieService { get; set; }

    public IMediaSearchLogger<Movie_BigCard> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<Movie_BigCard>();

    private CancellationTokenSource Cancellation = new CancellationTokenSource();

    public string? HeaderText => Movie.Name ?? "";
    public string? BodyTitle => AltNames;
    public string? BodySubTitle => Movie.Group;
    public string? BodyText => string.Join(", ", Movie.Tags);
    public string? FooterLink { get; set; }
    public string? ImageWidth { get; set; }


    protected override async Task OnParametersSetAsync() {
      Cancellation.Cancel();
      IsImageLoaded = false;
      if (MovieService is not null) {
        Cancellation = new CancellationTokenSource();
        await GetImage(Cancellation.Token);
      }
      StateHasChanged();
    }

    protected override async Task OnInitializedAsync() {
      await base.OnInitializedAsync();
    }

    protected override Task OnAfterRenderAsync(bool firstRender) {
      if (!firstRender) {
        Logger.LogDebug($"NOT first render for {Movie.Name}");
        Logger.LogDebug($"IsImageLoaded : {IsImageLoaded}");
        return base.OnAfterRenderAsync(firstRender);
      } else {
        Logger.LogDebug($"First render for {Movie.Name}");
        return base.OnAfterRenderAsync(firstRender);
      }
    }

    private async Task GetImage(CancellationToken cancelToken) {
      if (MovieService is not null && !IsImageLoaded) {
        StateHasChanged();
        ImageSource = await MovieService.GetPicture64(Movie, cancelToken);
        IsImageLoaded = true;
        StateHasChanged();
      }
    }

    public bool IsImageLoaded = false;

  }

}
