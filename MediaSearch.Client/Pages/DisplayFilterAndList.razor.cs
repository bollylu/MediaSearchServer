using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Pages {
  public partial class DisplayFilterAndList : ComponentBase, IDisposable {

    [Inject]
    public IBusService<IFilter>? BusServiceFilter { get; set; }
    private TFilter CurrentFilter = new TFilter();

    [Inject]
    public IBusService<string>? BusServiceAction { get; set; }

    public bool IsRefreshRuning { get; private set; } = false;

    protected override void OnInitialized() {
      // subscribe to OnMessage event
      if (BusServiceFilter is not null) {
        BusServiceFilter.OnMessage += _MessageHandler;
      }
      if (BusServiceAction is not null) {
        BusServiceAction.OnMessage += _MessageHandler;
      }
    }

    public void Dispose() {
      // unsubscribe from OnMessage event
      if (BusServiceFilter is not null) {
        BusServiceFilter.OnMessage -= _MessageHandler;
      }
      if (BusServiceAction is not null) {
        BusServiceAction.OnMessage -= _MessageHandler;
      }
    }

    private void _MessageHandler(string source, IFilter data) {
      switch (source) {
        case EditFilter.SVC_NAME: {
            CurrentFilter = new TFilter(data);
            break;
          }
        default: {
            break;
          }
      }
      StateHasChanged();
    }

    private void _MessageHandler(string source, string data) {
      switch (source) {
        case AdminControl.SVC_NAME when data == AdminControl.ACTION_REFRESH_RUNNING: {
            IsRefreshRuning = true;
            break;
          }

        case AdminControl.SVC_NAME when data == AdminControl.ACTION_REFRESH_COMPLETED: {
            IsRefreshRuning = false;
            break;
          }

        default: {
            break;
          }
      }
      StateHasChanged();
    }






  }
}
