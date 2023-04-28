﻿using MediaSearch.Models;

namespace MediaSearch.Models2;

public class TFilter : IFilter, IJson, IEquatable<TFilter> {

  public const int DEFAULT_PAGE_SIZE = 20;
  public const int DEFAULT_OUTPUT_DATE_MIN = 1900;
  public static int DEFAULT_OUTPUT_DATE_MAX = DateTime.Now.Year + 1;

  /// <summary>
  /// The language used in this filter
  /// </summary>
  public ELanguage Language { get; init; }

  #region --- Pagination --------------------------------------------
  /// <summary>
  /// The number of the page to request. Must be positive.
  /// </summary>
  [JsonPropertyName(nameof(Page))]
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
  [JsonPropertyName(nameof(PageSize))]
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
  public IMultiItemsSelection Keywords { get; init; } = new TMultiItemsSelection();
  #endregion --- Keywords in movie name --------------------------------------------

  #region --- Tags --------------------------------------------
  public IMultiItemsSelection Tags { get; init; } = new TMultiItemsSelection();
  #endregion --- Tags --------------------------------------------

  /// <summary>
  /// When selecting a movie, how many days in the past of it addition to the library should we look
  /// </summary>
  [JsonPropertyName(nameof(DaysBack))]
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
      if (value > OutputDateMax) {
        value = OutputDateMax;
      }
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

  [JsonConverter(typeof(JsonStringEnumConverter))]
  [JsonPropertyName(nameof(SortOrder))]
  public EFilterSortOrder SortOrder { get; set; } = EFilterSortOrder.Name;

  #region --- Groups --------------------------------------------
  [JsonPropertyName(nameof(GroupOnly))]
  public bool GroupOnly { get; set; } = false;

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
    Keywords = new TMultiItemsSelection(filter.Keywords);
    Tags = new TMultiItemsSelection(filter.Tags);
    DaysBack = filter.DaysBack;
    OutputDateMin = filter.OutputDateMin;
    OutputDateMax = filter.OutputDateMax;
    GroupOnly = filter.GroupOnly;
    SortOrder = filter.SortOrder;
    GroupFilter = filter.GroupFilter;
    GroupMemberships.AddRange(filter.GroupMemberships);
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent = 2) {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendIndent($"- {nameof(Keywords)}", indent);
    RetVal.AppendIndent($"  {Keywords}", indent + 2);
    RetVal.AppendIndent($"- {nameof(Tags)}", indent);
    RetVal.AppendIndent($"  {Tags}", indent + 2);
    RetVal.AppendIndent($"- {nameof(DaysBack)}={DaysBack}", indent);
    RetVal.AppendIndent($"- OutputDateRange={OutputDateMin}..{OutputDateMax}", indent);
    RetVal.AppendIndent($"- {nameof(Page)}={Page}", indent);
    RetVal.AppendIndent($"- {nameof(PageSize)}={PageSize}", indent);
    RetVal.AppendIndent($"- {nameof(GroupOnly)}={GroupOnly}", indent);
    RetVal.AppendIndent($"- {nameof(SortOrder)}={SortOrder}", indent);
    if (GroupMemberships.Any()) {
      RetVal.AppendIndent($"- {nameof(GroupMemberships)}=[{GroupMemberships.Select(x => x.WithQuotes()).CombineToString()}]", indent);
    }
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
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
      Keywords == other.Keywords &&
      Tags == other.Tags &&
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
      Keywords.GetHashCode() |
      Tags.GetHashCode() |
      OutputDateMin.GetHashCode() |
      OutputDateMax.GetHashCode() |
      GroupOnly.GetHashCode() |
      SortOrder.GetHashCode() |
      GroupMemberships.GetHashCode();
  }

  public override bool Equals(object? obj) {
    if (obj is TFilter Other) {
      return Equals(Other);
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
    Keywords.Clear();
    Tags.Clear();
    GroupOnly = false;
    OutputDateMin = DEFAULT_OUTPUT_DATE_MIN;
    OutputDateMax = DEFAULT_OUTPUT_DATE_MAX;
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