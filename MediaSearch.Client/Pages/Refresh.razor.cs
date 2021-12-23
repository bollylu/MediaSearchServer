using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Pages;

public partial class Refresh : ComponentBase {

  [Inject]
  public IBusService<string> BusService { get; set; }

  [Inject]
  public IMovieService MovieService { get; set; }

  [Inject]
  public NavigationManager NavManager { get; set; }

  public int Progress { get; set; } = 0;
  public bool Completed { get; set; } = false;
  public string Message { get; set; } = "";

  private const int REFRESH_COMPLETED = -1;
  private const int DELAY_BETWEEN_REFRESH_IN_MS = 1000;

  protected override async Task OnInitializedAsync() {
    int Status = await MovieService.GetRefreshStatus();
    if (Status == REFRESH_COMPLETED) {
      await MovieService.StartRefresh();
      await RefreshProgress();
    } else {
      Message = "There is already a refresh in progress, be patient !";
      await RefreshProgress();
    }
    NavManager.NavigateTo("/");
  }

  private async Task RefreshProgress() {
    while (!Completed) {
      int Status = await MovieService.GetRefreshStatus();
      if (Status == REFRESH_COMPLETED) {
        Completed = true;
      } else {
        Progress = Status;
        StateHasChanged();
        await Task.Delay(DELAY_BETWEEN_REFRESH_IN_MS);
      }
    }
  }

}
