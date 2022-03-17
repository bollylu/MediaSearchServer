using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Pages;

public partial class Settings : ComponentBase {
  [Inject]
  public NavigationManager? NavigationManager { get; set; }

  protected override void OnInitialized() {
    if (GlobalSettings.Account is null) {
      NavigationManager?.NavigateTo($"/login?{Login.PARAM_ORIGIN}={Login.ORIGIN_SETTINGS}", false, true);
      return;
    }
  }
}
