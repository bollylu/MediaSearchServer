using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Pages {
  public partial class PageHeader : ComponentBase, IDisposable {

    [Parameter]
    public string PageTitle { get; set; } = "";

    [Parameter]
    public bool WithSpinner { get; set; } = false;

    [Inject]
    public IBusService<IFilter>? BusServiceFilter { get; set; }

    [Inject]
    public IBusService<string>? BusServiceAction { get; set; }

    private string RemoteIpText => $"{GlobalSettings.Account?.RemoteIp.MapToIPv4().ToString() ?? "000.000.000.000"}";
    private string Username => $"{GlobalSettings.Account?.Name ?? "(anonymous)"}";
    private string UserConnectionDisplay => $"{Username} / {RemoteIpText}";

    private string Server => GlobalSettings.ApiServer?.BaseAddress?.ToString() ?? "(no server)";
    private string ServerRequestId => GlobalSettings.ApiServer?.RequestId.ToString() ?? "";
    private string ServerConnectionDisplay => $"{Server} #{ServerRequestId}";

    protected override void OnInitialized() {
      base.OnInitialized();
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
      StateHasChanged();
    }
    private void _MessageHandler(string source, string data) {
      StateHasChanged();
    }
  }
}
