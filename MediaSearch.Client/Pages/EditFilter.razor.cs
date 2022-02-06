using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Pages;

public partial class EditFilter : ComponentBase {

  public const string SVC_NAME = "filter";

  public TFilter Filter { get; set; } = new TFilter();

  public List<string> _Groups { get; } = new List<string>();

  [Inject]
  public IMovieService? MovieService { get; set; }

  [Inject]
  public IBusService<IFilter>? BusService { get; set; }

  protected override async Task OnInitializedAsync() {
    await base.OnInitializedAsync();
    if (MovieService is not null) {
      using (CancellationTokenSource cts = new CancellationTokenSource(5000)) {
        _Groups.AddRange(await MovieService.GetGroups(cts.Token));
      }
    }
    Filter.KeywordsSelection = EFilterType.All;
    Filter.TagSelection = EFilterType.All;
    Filter.GroupFilter = EFilterGroup.All;
    _NotifyMessage(Filter);
  }

  private void _ProcessSearch() {
    Filter.Page = 1;
    _NotifyMessage(Filter);
  }

  private void _NotifyMessage(IFilter filter) {
    if (filter is null) {
      return;
    }
    BusService?.SendMessage(SVC_NAME, filter);
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
  }

  private void GroupMembershipSelectionChanged(ChangeEventArgs e) {
    if (e.Value is not string[] GroupMembershipSelection) {
      return;
    }
    Filter.GroupMemberships.Clear();
    Filter.GroupMemberships.AddRange(GroupMembershipSelection);
  }
}

