using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Pages;

public partial class EditFilter : ComponentBase {

  public const string SVC_NAME = "filter";

  public TFilter Filter { get; set; } = new TFilter();

  [Inject]
  public IBusService<TFilter>? BusService { get; set; }

  protected override void OnInitialized() {
    base.OnInitialized();
    _NotifyMessage(Filter);
  }

  private void _ProcessSearch() {
      _NotifyMessage(Filter);
  }

  private void _NotifyMessage(TFilter filter) {
    if (filter is null) {
      return;
    }
    BusService?.SendMessage(SVC_NAME, filter);
  }
}

