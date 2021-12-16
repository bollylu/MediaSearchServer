
using Microsoft.AspNetCore.Components;

using MovieSearchClientServices;
using MovieSearchClientModels;

namespace BlazorClient.Pages;

public partial class EditFilter : ComponentBase {

  public const string SVC_NAME = "filter";

  public SearchFilter Filter { get; set; } = new SearchFilter();

  [Inject]
  public IBusService<string>? BusService { get; set; }

  protected override void OnInitialized() {
    base.OnInitialized();
    _NotifyMessage(Filter.Value);
  }

  private void _ProcessSearch(ChangeEventArgs args) {
    if (args is not null) {
      Filter.Value = (string?)args.Value;
      _NotifyMessage(Filter.Value);
    }
  }

  private void _NotifyMessage(string? message) {
    if (message is null) {
      return;
    }
    BusService?.SendMessage(SVC_NAME, message);
  }
}

