namespace MediaSearch.Models;

public class TFilter : AJson<TFilter>, IEquatable<TFilter> {

  public const int DEFAULT_PAGE_SIZE = 20;

  /// <summary>
  /// The number of the page to request. Must be positive.
  /// </summary>
  public int Page { get; set; } = 1;

  /// <summary>
  /// The maximum number of items on the page. Must be positive.
  /// </summary>
  public int PageSize { get; set; } = DEFAULT_PAGE_SIZE;

  /// <summary>
  /// Keywords to use for the search in the Movie name
  /// </summary>
  public string Keywords { get; set; } = "";
  /// <summary>
  /// How to use the keywords for the search
  /// </summary>
  public EFilterType KeywordsSelection { get; set; }

  /// <summary>
  /// Tags to be searched for
  /// </summary>
  public string Tags { get; set; } = "";
  /// <summary>
  /// How to use the tags for the search
  /// </summary>
  public EFilterType TagSelection { get; set; }

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
  private int _DaysBack;

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
  private int _OutputDateMin = 0;

  /// <summary>
  /// When selecting a movie, the maximum (included) of the range for the output date
  /// </summary>
  public int OutputDateMax {
    get {
      return _OutputDateMax;
    }
    set {
      _OutputDateMax = value.WithinLimits(OutputDateMin, int.MaxValue);
    }
  }
  private int _OutputDateMax = DateTime.Now.Year + 1;

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
      Tags == other.Tags;
  }
  #endregion --- IEquatable<TFilter> --------------------------------------------

  #region --- Equalilty comparison --------------------------------------------
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
      OutputDateMax.GetHashCode();
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
  #endregion --- Equalilty comparison --------------------------------------------

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
      PageSize = Root.GetPropertyEx(nameof(PageSize)).GetInt32();
      Keywords = Root.GetPropertyEx(nameof(Keywords)).GetString();
      KeywordsSelection = Enum.Parse<EFilterType>(Root.GetPropertyEx(nameof(KeywordsSelection)).GetString());
      Tags = Root.GetPropertyEx(nameof(Tags)).GetString();
      TagSelection = Enum.Parse<EFilterType>(Root.GetPropertyEx(nameof(TagSelection)).GetString());
      DaysBack = Root.GetPropertyEx(nameof(DaysBack)).GetInt32();
      OutputDateMin = Root.GetPropertyEx(nameof(OutputDateMin)).GetInt32();
      OutputDateMax = Root.GetPropertyEx(nameof(OutputDateMax)).GetInt32();

    } catch (Exception ex) {
      Logger?.LogError($"Unable to parse json : {ex.Message}");
    }

    return this;
  }
  #endregion --- IJson --------------------------------------------

}