using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Pages {
  public partial class DisplayFilterAndList {

    [Inject]
    public IBusService<IFilter>? BusService { get; set; }

    private TFilter CurrentFilter = new TFilter();

    protected override void OnInitialized() {
      // subscribe to OnMessage event
      if (BusService is not null) {
        BusService.OnMessage += _MessageHandler;
      }
    }

    public void Dispose() {
      // unsubscribe from OnMessage event
      if (BusService is not null) {
        BusService.OnMessage -= _MessageHandler;
      }
    }

    private void _MessageHandler(string source, IFilter data) {
      switch (source) {

        case EditFilter.SVC_NAME: {
            CurrentFilter = new TFilter(data);
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
