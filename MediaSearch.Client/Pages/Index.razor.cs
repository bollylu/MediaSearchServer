using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Pages;

public partial class Index : ComponentBase, ILoggable {

  public ILogger? Logger { get; set; }

  public void SetLogger(ILogger logger) {
    Logger = logger;
  }

  [Inject]
  public IBusService<RFilter> BusService { get; set; }

  [Inject]
  public IMovieService? MovieService { get; set; }

  private RFilter CurrentFilter;
  private string? ServerApi;

  protected override void OnInitialized() {
    // subscribe to OnMessage event
    SetLogger(ALogger.SYSTEM_LOGGER);
    BusService.OnMessage += _MessageHandler;
    ServerApi = MovieService.ApiBase;
  }

  public void Dispose() {
    // unsubscribe from OnMessage event
    BusService.OnMessage -= _MessageHandler;
  }

  private void _MessageHandler(string source, RFilter data) {
    switch (source) {

      case EditFilter.SVC_NAME: {
          Logger?.Log($"Filter is now [{data}]");
          CurrentFilter = data;
          StateHasChanged();
        }
        break;

      default: {
          Logger?.Log($"Unknown source : {source}");
          StateHasChanged();
          break;
        }
    }
  }
}