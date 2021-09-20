
using Microsoft.AspNetCore.Components;

using MovieSearchClient.Services;
using MovieSearchClient.Models;

namespace MovieSearchClient.Pages {
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
