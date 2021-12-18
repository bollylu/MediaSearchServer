using MovieSearchClientServices;
using Microsoft.AspNetCore.Components;

namespace BlazorClient.Pages;

public partial class Refresh : ComponentBase {

  [Inject]
  public IBusService<string> BusService { get; set; }
  [Inject]
  public IMovieService MovieService { get; set; }
  [Inject]
  public NavigationManager NavManager { get; set; }

  public int Progress { get; set; } = 0;
  public bool Completed { get; set; } = false;

  protected override async Task OnInitializedAsync() {
    Completed = false;
    Progress = 0;
    await MovieService.StartRefresh();
    BusService.SendMessage(nameof(Refresh), "REFRESH");
    StateHasChanged();
    await RefreshProgress();
    NavManager.NavigateTo("/");
  }

  private async Task RefreshProgress() {
    while (!Completed) {
      int Status = await MovieService.GetRefreshStatus();
      if (Status == -1) {
        Completed = true;
      } else {
        Progress = Status; 
        StateHasChanged();
        await Task.Delay(1000);
      }
    }
  }

}
