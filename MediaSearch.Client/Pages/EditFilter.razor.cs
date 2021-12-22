using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Pages;

public partial class EditFilter : ComponentBase {

  public const string SVC_NAME = "filter";

  public RFilter Filter { get; set; } = new RFilter();

  [Inject]
  public IBusService<RFilter>? BusService { get; set; }

  protected override void OnInitialized() {
    base.OnInitialized();
    _NotifyMessage(Filter);
  }

  private void _ProcessSearch() {
      _NotifyMessage(Filter);
  }

  private void _NotifyMessage(RFilter filter) {
    if (filter is null) {
      return;
    }
    BusService?.SendMessage(SVC_NAME, filter);
  }
}

