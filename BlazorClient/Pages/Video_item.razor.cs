using BLTools;

using Microsoft.AspNetCore.Components;

using MovieSearchModels;


namespace BlazorClient.Pages {
  public partial class Video_item : ComponentBase {


    [Parameter]
    public IMovie? Movie { get; set; }

    protected override void OnParametersSet() {
      base.OnParametersSet();
    }

  }

}
