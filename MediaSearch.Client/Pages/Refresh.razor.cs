using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Pages;

public partial class Refresh : ComponentBase {

  //[Inject]
  //public IBusService<string>? BusService { get; set; }

  [Inject]
  public IMovieService? MovieService { get; set; }

  [Inject]
  public NavigationManager? NavManager { get; set; }

  public int Progress { get; set; } = 0;
  public bool Completed { get; set; } = false;
  public string Message { get; set; } = "";

  private const int REFRESH_COMPLETED = -1;
  private const int DELAY_BETWEEN_REFRESH_IN_MS = 1000;

  protected override async Task OnInitializedAsync() {
    int Status = await _GetRefreshStatus().ConfigureAwait(false);

    if (Status == REFRESH_COMPLETED && MovieService is not null) {
      await MovieService.StartRefresh();
    } else {
      Message = "There is already a refresh in progress, be patient !";
    }

    _ = Task.Run(() => RefreshProgress()).ConfigureAwait(false);

  }

  private async Task RefreshProgress() {
    while (!Completed) {
      int Status = await _GetRefreshStatus().ConfigureAwait(false);
      if (Status == REFRESH_COMPLETED) {
        Completed = true;
      } else {
        Progress = Status;
        StateHasChanged();
        await Task.Delay(DELAY_BETWEEN_REFRESH_IN_MS).ConfigureAwait(false);
      }
    }

    if (NavManager is not null) {
      NavManager.NavigateTo("/");
    }
  }

  private async Task<int> _GetRefreshStatus() {
    if (MovieService is null) {
      return REFRESH_COMPLETED;
    }

    return await MovieService.GetRefreshStatus().ConfigureAwait(false);
  }
}
