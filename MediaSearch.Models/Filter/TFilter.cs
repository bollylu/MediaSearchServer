namespace MediaSearch.Models;

public class TFilter : IFilter, IEquatable<TFilter> {

  public const int DEFAULT_PAGE_SIZE = 20;
  public const int DEFAULT_OUTPUT_DATE_MIN = 1900;
  public static int DEFAULT_OUTPUT_DATE_MAX = DateTime.Now.Year + 1;

  #region --- Pagination --------------------------------------------
  /// <summary>
  /// The number of the page to request. Must be positive.
  /// </summary>
  public int Page {
    get {
      return _Page;
    }
    set {
      _Page = value.WithinLimits(1, int.MaxValue);
    }
  }
  private int _Page = 1;

  /// <summary>
  /// The maximum number of items on the page. Must be positive.
  /// </summary>
  public int PageSize {
    get {
      return _PageSize;
    }
    set {
      if (value == 0) {
        _PageSize = DEFAULT_PAGE_SIZE;
      } else {
        _PageSize = value.WithinLimits(1, int.MaxValue);
      }
    }
  }
  private int _PageSize = DEFAULT_PAGE_SIZE;
  #endregion --- Pagination --------------------------------------------

  #region --- Keywords in movie name --------------------------------------------
  /// <summary>
  /// Keywords to use for the search in the Movie name
  /// </summary>
  public string Keywords { get; set; } = "";
  /// <summary>
  /// How to use the keywords for the search
  /// </summary>
  public EFilterType KeywordsSelection { get; set; }
  #endregion --- Keywords in movie name --------------------------------------------

  #region --- Tags --------------------------------------------
  /// <summary>
  /// Tags to be searched for
  /// </summary>
  [JsonPropertyName(nameof(Tags))]
  public string Tags { get; set; } = "";
  /// <summary>
  /// How to use the tags for the search
  /// </summary>
  [JsonConverter(typeof(JsonStringEnumConverter))]
  [JsonPropertyName(nameof(TagSelection))]
  public EFilterType TagSelection { get; set; }
  #endregion --- Tags --------------------------------------------

  /// <summary>
  /// When selecting a movie, how many days in the past of it addition to the library should we look
  /// </summary>
  public int DaysBack {
    get {
      return _DaysBack;
    }
    set {
      _DaysBack = value.WithinLimits(0, int.MaxValue);
    }
  }
  private int _DaysBack = 0;

  #region --- Output date --------------------------------------------
  /// <summary>
  /// When selecting a movie, the minimum (included) of the range for the output date
  /// </summary>
  [JsonPropertyName(nameof(OutputDateMin))]
  public int OutputDateMin {
    get {
      return _OutputDateMin;
    }
    set {
      _OutputDateMin = value.WithinLimits(0, int.MaxValue);
    }
  }
  private int _OutputDateMin = DEFAULT_OUTPUT_DATE_MIN;

  /// <summary>
  /// When selecting a movie, the maximum (included) of the range for the output date
  /// </summary>
  [JsonPropertyName(nameof(OutputDateMax))]
  public int OutputDateMax {
    get {
      return _OutputDateMax;
    }
    set {
      if (value == 0) {
        _OutputDateMax = DateTime.Now.Year + 1;
      } else {
        _OutputDateMax = value.WithinLimits(OutputDateMin, int.MaxValue);
      }
    }
  }
  private int _OutputDateMax = DateTime.Now.Year + 1;

  #endregion --- Output date --------------------------------------------

  public EFilterSortOrder SortOrder { get; set; } = EFilterSortOrder.Name;

  #region --- Groups --------------------------------------------
  [JsonPropertyName(nameof(GroupOnly))]
  public bool GroupOnly { get; set; } = false;

  /// <summary>
  /// Group to searched for
  /// </summary>
  [JsonPropertyName(nameof(Group))]
  public string Group { get; set; } = "";

  /// <summary>
  /// Sub-group to searched for
  /// </summary>
  [JsonPropertyName(nameof(SubGroup))]
  public string SubGroup { get; set; } = "";

  [JsonPropertyName(nameof(GroupMemberships))]
  public List<string> GroupMemberships { get; set; } = new();

  [JsonConverter(typeof(JsonStringEnumConverter))]
  [JsonPropertyName(nameof(GroupFilter))]
  public EFilterGroup GroupFilter { get; set; } = EFilterGroup.All;
  #endregion --- Groups --------------------------------------------

  #region --- Static instance for an empty filter --------------------------------------------
  public static TFilter Empty => _Empty ??= new TFilter();
  private static TFilter? _Empty;
  #endregion --- Static instance for an empty filter --------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TFilter() { }
  public TFilter(IFilter filter) : this() {
    Page = filter.Page;
    PageSize = filter.PageSize;
    Keywords = filter.Keywords;
    KeywordsSelection = filter.KeywordsSelection;
    Tags = filter.Tags;
    TagSelection = filter.TagSelection;
    DaysBack = filter.DaysBack;
    OutputDateMin = filter.OutputDateMin;
    OutputDateMax = filter.OutputDateMax;
    Group = filter.Group;
    SubGroup = filter.SubGroup;
    GroupOnly = filter.GroupOnly;
    SortOrder = filter.SortOrder;
    GroupFilter = filter.GroupFilter;
    GroupMemberships.AddRange(filter.GroupMemberships);
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString() {
    return ToString(0);
  }

  public string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"- {nameof(Keywords)} = {KeywordsSelection}:{Keywords.WithQuotes()}");
    RetVal.AppendLine($"- {nameof(Tags)} = {TagSelection}:{Tags.WithQuotes()}");
    RetVal.AppendLine($"- {nameof(DaysBack)}={DaysBack}");
    RetVal.AppendLine($"- OutputDateRange = {OutputDateMin}..{OutputDateMax}");
    RetVal.AppendLine($"- {nameof(Page)} = {Page}");
    RetVal.AppendLine($"- {nameof(PageSize)} = {PageSize}");
    RetVal.AppendLine($"- {nameof(GroupOnly)} = {GroupOnly}");
    RetVal.AppendLine($"- {nameof(Group)} = {Group.WithQuotes()}");
    RetVal.AppendLine($"- {nameof(SubGroup)} = {SubGroup.WithQuotes()}");
    RetVal.AppendLine($"- {nameof(SortOrder)} = {SortOrder}");
    if (GroupMemberships.Any()) {
      RetVal.AppendLine($"- {nameof(GroupMemberships)}");
      foreach (string GroupMembershipItem in GroupMemberships) {
        RetVal.AppendIndent($"- {GroupMembershipItem}", indent + 2);
      }
    }
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  #region --- IEquatable<TFilter> --------------------------------------------
  public bool Equals(TFilter? other) {
    if (other is null) {
      return false;
    }

    return
      Page == other.Page &&
      PageSize == other.PageSize &&
      DaysBack == other.DaysBack &&
      OutputDateMin == other.OutputDateMin &&
      OutputDateMax == other.OutputDateMax &&
      KeywordsSelection == other.KeywordsSelection &&
      Keywords == other.Keywords &&
      TagSelection == other.TagSelection &&
      Tags == other.Tags &&
      Group == other.Group &&
      SubGroup == other.SubGroup &&
      GroupOnly == other.GroupOnly &&
      SortOrder == other.SortOrder &&
      GroupMemberships.SequenceEqual(other.GroupMemberships);
  }
  #endregion --- IEquatable<TFilter> --------------------------------------------

  #region --- Equality comparison --------------------------------------------
  public override int GetHashCode() {
    return
      Page.GetHashCode() |
      PageSize.GetHashCode() |
      DaysBack.GetHashCode() |
      KeywordsSelection.GetHashCode() |
      Keywords.GetHashCode() |
      Tags.GetHashCode() |
      TagSelection.GetHashCode() |
      OutputDateMin.GetHashCode() |
      OutputDateMax.GetHashCode() |
      GroupOnly.GetHashCode() |
      Group.GetHashCode() |
      SubGroup.GetHashCode() |
      SortOrder.GetHashCode() |
      GroupMemberships.GetHashCode();
  }

  public override bool Equals(object? obj) {
    if (obj is TFilter Other) {
      return this.Equals(Other);
    }
    return false;
  }

  public static bool operator ==(TFilter a, TFilter b) {
    return a.Equals(b);
  }

  public static bool operator !=(TFilter a, TFilter b) {
    return !a.Equals(b);
  }
  #endregion --- Equality comparison --------------------------------------------

  public void Clear() {
    FirstPage();
    Keywords = "";
    KeywordsSelection = EFilterType.All;
    Tags = "";
    TagSelection = EFilterType.All;
    Group = "";
    SubGroup = "";
    GroupOnly = false;
    OutputDateMin = TFilter.DEFAULT_OUTPUT_DATE_MIN;
    OutputDateMax = TFilter.DEFAULT_OUTPUT_DATE_MAX;
    DaysBack = 0;
    GroupFilter = EFilterGroup.All;
    GroupMemberships.Clear();
  }
  public void NextPage() {
    Page++;
  }
  public void PreviousPage() {
    Page--;
  }
  public void FirstPage() {
    Page = 1;
  }
  public void SetPage(int pageNumber) {
    Page = pageNumber.WithinLimits(1, int.MaxValue);
  }

}