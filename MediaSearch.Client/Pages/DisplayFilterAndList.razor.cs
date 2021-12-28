using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Pages {
  public partial class DisplayFilterAndList {

    [Inject]
    public IBusService<TFilter> BusService { get; set; }

    private TFilter CurrentFilter = new TFilter();

    protected override void OnInitialized() {
      // subscribe to OnMessage event
      BusService.OnMessage += _MessageHandler;
    }

    public void Dispose() {
      // unsubscribe from OnMessage event
      BusService.OnMessage -= _MessageHandler;
    }

    private void _MessageHandler(string source, TFilter data) {
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
