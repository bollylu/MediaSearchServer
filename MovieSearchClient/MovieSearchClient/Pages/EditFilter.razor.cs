
using Microsoft.AspNetCore.Components;

using MovieSearchClientServices;
using MovieSearchClientModels;

namespace MovieSearchClient.Pages {
  public partial class EditFilter : ComponentBase {

    public const string SVC_NAME = "filter";

    public SearchFilter Filter { get; set; } = new SearchFilter();

    [Inject]
    public IBusService<string> BusService { get; set; }

    protected override void OnInitialized() {
      base.OnInitialized();
      BusService.SendMessage(SVC_NAME, Filter.Value);
    }
    private void ProcessSearch(ChangeEventArgs args) {
      Filter.Value = (string)args.Value;
      BusService.SendMessage(SVC_NAME, Filter.Value);
    }
  }

}
