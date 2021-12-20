using Microsoft.AspNetCore.Components;

namespace BlazorClient.Pages {
  public partial class PageHeader : ComponentBase {

    public Version Version { get; set; } = new Version(0,0,4);
  }
}
