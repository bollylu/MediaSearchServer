
using Microsoft.AspNetCore.Components;

using MovieSearchClientServices;
using MovieSearchClientModels;

namespace BlazorClient.Pages;

public partial class EditFilter : ComponentBase {

  public const string SVC_NAME = "filter";

  public SearchFilter Filter { get; set; } = new SearchFilter();

  [Inject]
  public IBusService<SearchFilter>? BusService { get; set; }

  protected override void OnInitialized() {
    base.OnInitialized();
    Filter.Days = 0;
    _NotifyMessage(Filter);
  }

  private void _ProcessSearch() {
      _NotifyMessage(Filter);
  }
  private void _NotifyMessage(SearchFilter filter) {
    if (filter is null) {
      return;
    }
    BusService?.SendMessage(SVC_NAME, filter);
  }
}

