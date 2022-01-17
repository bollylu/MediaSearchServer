using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Pages {
  public partial class PageHeader : ComponentBase {

    [Parameter]
    public string PageTitle { get; set; } = "";

  }
}
