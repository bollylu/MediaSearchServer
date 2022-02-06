using Microsoft.AspNetCore.Components;

using System;
using System.Diagnostics;

namespace MediaSearch.Client.Pages;

public partial class AdminControl : ComponentBase {

  [Inject]
  public IMovieService? MovieService { get; set; }

  public int Progress { get; set; } = 0;
  public bool Completed { get; set; } = false;
  public string Message { get; set; } = "";

  private const int REFRESH_COMPLETED = -1;
  private const int DELAY_BETWEEN_REFRESH_IN_MS = 1000;

  private bool IsRefreshRunning = true;

  protected override async Task OnInitializedAsync() {
   IsRefreshRunning = await CheckRefreshRunning();
    if (IsRefreshRunning) {
      _ = Task.Run(() => RefreshProgress());
    }
  }

  private async Task RefreshData() {
    if (MovieService is null) {
      return;
    }

    if (!await CheckRefreshRunning() ) {
      IsRefreshRunning = true;
      await MovieService.StartRefresh();
      Completed = false;
      await RefreshProgress();
    }
  }

  private async Task<bool> CheckRefreshRunning() {
    if (MovieService is null) {
      return false;
    }

    return await MovieService.GetRefreshStatus() != REFRESH_COMPLETED;
  }

  private async Task RefreshProgress() {
    while (!Completed) {
      int Status = await _GetRefreshStatus();
      if (Status == REFRESH_COMPLETED) {
        Completed = true;
        Message = "Refresh completed successfully.";
        IsRefreshRunning = false;
        StateHasChanged();
      } else {
        Progress = Status;
        Message = $"Refresh in progress : found {Progress} movies";
        StateHasChanged();
        await Task.Delay(DELAY_BETWEEN_REFRESH_IN_MS);
      }
    }
  }

  private async Task<int> _GetRefreshStatus() {
    if (MovieService is null) {
      return REFRESH_COMPLETED;
    }

    return await MovieService.GetRefreshStatus();
  }
}
