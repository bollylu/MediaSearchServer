using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Pages {
  public partial class DisplayFilterAndList {

    [Inject]
    public IBusService<RFilter> BusService { get; set; }

    private RFilter CurrentFilter = new RFilter();

    protected override void OnInitialized() {
      // subscribe to OnMessage event
      BusService.OnMessage += _MessageHandler;
    }

    public void Dispose() {
      // unsubscribe from OnMessage event
      BusService.OnMessage -= _MessageHandler;
    }

    private void _MessageHandler(string source, RFilter data) {
      switch (source) {

        case EditFilter.SVC_NAME: {
            CurrentFilter = data;
            StateHasChanged();
          }
          break;

        default: {
            StateHasChanged();
            break;
          }
      }
    }
  }
}
