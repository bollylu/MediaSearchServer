using Microsoft.AspNetCore.Components;

namespace MovieSearchClient.Pages {
  public partial class PageHeader : ComponentBase {

    [Parameter]
    public int VideoCount { get; set; } = 0;
  }
}
