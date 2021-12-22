using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Pages {
  public partial class Video_item : ComponentBase {


    [Parameter]
    public IMovie? Movie { get; set; }

    public int SizeInMb => Movie is null ? 0 : (int)(Movie.Size / 1024 / 1024);

    protected override void OnParametersSet() {
      base.OnParametersSet();
    }

  }

}
