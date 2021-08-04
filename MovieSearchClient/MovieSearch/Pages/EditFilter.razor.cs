
using Microsoft.AspNetCore.Components;

using MovieSearch.Client.Services;
using MovieSearch.Client.Models;

namespace MovieSearch.Pages {
  public partial class EditFilter : ComponentBase {

    public const string SVC_NAME = "filter";

    public SearchFilter Filter { get; set; } = new SearchFilter();


    [Inject]
    public IBusService<string> BusService { get; set; }



    private void ProcessSearch(ChangeEventArgs args) {
      Filter.Value = (string)args.Value;
      BusService.SendMessage(SVC_NAME, Filter.Value);
    }
  }

}
