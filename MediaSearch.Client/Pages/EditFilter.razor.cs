using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Pages;

public partial class EditFilter : ComponentBase, IDisposable {

  public const string SVC_NAME = "filter";

  public TFilter Filter { get; set; } = new TFilter();

  public List<string> Groups { get; } = new List<string>();

  [Inject]
  public IMovieService? MovieService { get; set; }

  [Inject]
  public IBusService<IFilter>? BusServiceFilter { get; set; }
  
  [Inject]
  public IBusService<string>? BusServiceAction { get; set; }

  protected override async Task OnInitializedAsync() {
    await base.OnInitializedAsync();
    Filter.KeywordsSelection = EFilterType.All;
    Filter.TagSelection = EFilterType.All;
    Filter.GroupFilter = EFilterGroup.All;
    

    if (BusServiceAction is not null) {
      BusServiceAction.OnMessage += _MessageHandler;
    }

    if (MovieService is not null) {
      using (CancellationTokenSource cts = new CancellationTokenSource(5000)) {
        Groups.Clear();
        Groups.AddRange(await MovieService.GetGroups(cts.Token));
      }
    }

    _NotifyMessage(Filter);
  }

  public void Dispose() {
    // unsubscribe from OnMessage event
    if (BusServiceAction is not null) {
      BusServiceAction.OnMessage -= _MessageHandler;
    }
  }

  private void _ProcessSearch() {
    Filter.Page = 1;
    _NotifyMessage(Filter);
  }

  private void _NotifyMessage(IFilter filter) {
    if (filter is null) {
      return;
    }
    BusServiceFilter?.SendMessage(SVC_NAME, filter);
  }

  private void ClearKeywords() {
    Filter.Keywords = "";
    Filter.KeywordsSelection = EFilterType.All;
  }

  private bool ClearKeywordsDisabled => string.IsNullOrEmpty(Filter.Keywords);

  private void ClearTags() {
    Filter.Tags = "";
    Filter.TagSelection = EFilterType.All;
  }

  private bool ClearTagsDisabled => string.IsNullOrEmpty(Filter.Tags);

  private void ClearOutputDate() {
    Filter.OutputDateMin = TFilter.DEFAULT_OUTPUT_DATE_MIN;
    Filter.OutputDateMax = TFilter.DEFAULT_OUTPUT_DATE_MAX;
  }

  private bool ClearOutputDateDisabled => Filter.OutputDateMin == TFilter.DEFAULT_OUTPUT_DATE_MIN && Filter.OutputDateMax == TFilter.DEFAULT_OUTPUT_DATE_MAX;

  private void ClearGroup() {
    Filter.Group = "";
    Filter.GroupOnly = false;
    Filter.GroupMemberships.Clear();
    Filter.GroupFilter = EFilterGroup.All;
  }

  private bool ClearGroupDisabled => string.IsNullOrEmpty(Filter.Group) && Filter.GroupOnly == false;

  private void ClearFilter() {
    Filter.Clear();
    _NotifyMessage(Filter);
  }

  private async void _MessageHandler(string source, string data) {
    switch (source) {

      case AdminControl.SVC_NAME when data == AdminControl.ACTION_REFRESH_COMPLETED: {
          if (MovieService is not null) {
            using (CancellationTokenSource cts = new CancellationTokenSource(5000)) {
              Groups.Clear();
              Groups.AddRange(await MovieService.GetGroups(cts.Token));
            }
          }
          StateHasChanged();
          break;
        }

      default: {
          break;
        }
    }
    StateHasChanged();
  }
}

