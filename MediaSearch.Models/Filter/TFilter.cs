namespace MediaSearch.Models;

public class TFilter : AJson<TFilter>, IEquatable<TFilter> {

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
  public string Tags { get; set; } = "";
  /// <summary>
  /// How to use the tags for the search
  /// </summary>
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
  public bool GroupOnly { get; set; } = false;
  /// <summary>
  /// Group to searched for
  /// </summary>
  public string Group { get; set; } = "";
  /// <summary>
  /// Sub-group to searched for
  /// </summary>
  public string SubGroup { get; set; } = "";
  #endregion --- Groups --------------------------------------------

  #region --- Static instance for an empty filter --------------------------------------------
  public static TFilter Empty => _Empty ??= new TFilter();
  private static TFilter _Empty;
  #endregion --- Static instance for an empty filter --------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TFilter() { }
  public TFilter(TFilter filter) {
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
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.Append($"{nameof(Keywords)}={KeywordsSelection}:{Keywords.WithQuotes()}");
    RetVal.Append($", {nameof(Tags)}={TagSelection}:{Tags.WithQuotes()}");
    RetVal.Append($", {nameof(DaysBack)}={DaysBack}");
    RetVal.Append($", OutputDateRange={OutputDateMin}..{OutputDateMax}");
    RetVal.Append($", {nameof(Page)}={Page}");
    RetVal.Append($", {nameof(PageSize)}={PageSize}");
    RetVal.Append($", {nameof(GroupOnly)}={GroupOnly}");
    RetVal.Append($", {nameof(Group)}={Group}");
    RetVal.Append($", {nameof(SubGroup)}={SubGroup}");
    RetVal.Append($", {nameof(SortOrder)}={SortOrder}");
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  #region --- IEquatable<TFilter> --------------------------------------------
  public bool Equals(TFilter other) {
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
      SortOrder == other.SortOrder;
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
      SortOrder.GetHashCode();
  }

  public override bool Equals(object obj) {
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

  #region --- IJson --------------------------------------------
  public override string ToJson(JsonWriterOptions options) {

    using (MemoryStream Utf8JsonStream = new()) {
      using (Utf8JsonWriter Writer = new Utf8JsonWriter(Utf8JsonStream, options)) {

        Writer.WriteStartObject();

        Writer.WriteNumber(nameof(Page), Page);
        Writer.WriteNumber(nameof(PageSize), PageSize);
        Writer.WriteString(nameof(Keywords), Keywords);
        Writer.WriteString(nameof(KeywordsSelection), KeywordsSelection.ToString());
        Writer.WriteString(nameof(Tags), Tags);
        Writer.WriteString(nameof(TagSelection), TagSelection.ToString());
        Writer.WriteNumber(nameof(DaysBack), DaysBack);
        Writer.WriteNumber(nameof(OutputDateMin), OutputDateMin);
        Writer.WriteNumber(nameof(OutputDateMax), OutputDateMax);
        Writer.WriteBoolean(nameof(GroupOnly), GroupOnly);
        Writer.WriteString(nameof(Group), Group);
        Writer.WriteString(nameof(SubGroup), SubGroup);
        Writer.WriteString(nameof(SortOrder), SortOrder.ToString());

        Writer.WriteEndObject();
      }

      return Encoding.UTF8.GetString(Utf8JsonStream.ToArray());
    }
  }

  public override TFilter ParseJson(string source) {
    #region === Validate parameters ===
    if (string.IsNullOrWhiteSpace(source)) {
      throw new JsonException("Json filter source is null");
    }
    #endregion === Validate parameters ===

    try {
      JsonDocument JsonFilter = JsonDocument.Parse(source);
      JsonElement Root = JsonFilter.RootElement;

      //LogDebugEx(Root.GetRawText().BoxFixedWidth("RawText", 80, TextBox.EStringAlignment.Left));

      Page = Root.GetPropertyEx(nameof(Page)).GetInt32();
      int PageSizeFromJson = Root.GetPropertyEx(nameof(PageSize)).GetInt32();
      PageSize = PageSizeFromJson == 0 ? DEFAULT_PAGE_SIZE : PageSizeFromJson;
      Keywords = Root.GetPropertyEx(nameof(Keywords)).GetString();
      KeywordsSelection = Enum.Parse<EFilterType>(Root.GetPropertyEx(nameof(KeywordsSelection)).GetString());
      Tags = Root.GetPropertyEx(nameof(Tags)).GetString();
      TagSelection = Enum.Parse<EFilterType>(Root.GetPropertyEx(nameof(TagSelection)).GetString());
      DaysBack = Root.GetPropertyEx(nameof(DaysBack)).GetInt32();
      OutputDateMin = Root.GetPropertyEx(nameof(OutputDateMin)).GetInt32();
      OutputDateMax = Root.GetPropertyEx(nameof(OutputDateMax)).GetInt32();
      GroupOnly = Root.GetPropertyEx(nameof(GroupOnly)).GetBoolean();
      Group = Root.GetPropertyEx(nameof(Group)).GetString();
      SubGroup = Root.GetPropertyEx(nameof(SubGroup)).GetString();
      SortOrder = Enum.Parse<EFilterSortOrder>(Root.GetPropertyEx(nameof(SortOrder)).GetString());

    } catch (Exception ex) {
      Logger?.LogError($"Unable to parse json : {ex.Message}");
    }

    return this;
  }
  #endregion --- IJson --------------------------------------------

}