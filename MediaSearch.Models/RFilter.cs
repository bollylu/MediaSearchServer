using System.Drawing;
using System.Text.RegularExpressions;

namespace MediaSearch.Models;

public record RFilter : ILoggable {

  public string Name { get; set; }
  public EFilterKeywords KeywordsSelection { get; set; }
  public int DaysBack {
    get {
      return _DaysBack;
    }
    set {
      _DaysBack = value.WithinLimits(0, int.MaxValue);
    }
  }
  private int _DaysBack;

  public ILogger Logger { get; set; }

  public static RFilter Empty => new RFilter();

  //public string ToBase64() {
  //  StringBuilder RetVal = new StringBuilder();
  //  RetVal.Append($"{nameof(Name)}={Name.ToUrl()}");

  //  if (KeywordsAll) {
  //    RetVal.Append('&');
  //    RetVal.Append($"{nameof(KeywordsAll)}={KeywordsAll}");
  //  }

  //  if (KeywordsAny) {
  //    RetVal.Append('&');
  //    RetVal.Append($"{nameof(KeywordsAny)}={KeywordsAny}");
  //  }

  //  RetVal.Append('&');
  //  RetVal.Append($"daysback={DaysBack}");

  //  return RetVal.ToString();
  //}

  //public RFilter FromBase64(string encodedFilter) {

  //  string DecodedFilter = Encoding.UTF8.GetString(Convert.FromBase64String(encodedFilter));
  //  ISplitArgs Args = new SplitArgs();
  //  Args.Parse(DecodedFilter.Replace('&',' '));

  //  Name = 
  //  KeywordsAll = Args.GetValue(nameof(KeywordsAll), false);
  //  KeywordsAny = Args.GetValue(nameof(KeywordsAny), false);
  //  DaysBack = Args.GetValue(nameof(DaysBack), 0);

  //  return this;
  //}

  //public static RFilter FromUrl(string url) {
  //  RFilter RetVal = new RFilter();
  //  RetVal.ParseUrl(url);
  //  return RetVal;
  //}

  #region --- ToJson --------------------------------------------
  public string ToJson() {

    return ToJson(new JsonWriterOptions());

  }

  public string ToJson(JsonWriterOptions options) {

    using (MemoryStream Utf8JsonStream = new()) {
      using (Utf8JsonWriter Writer = new Utf8JsonWriter(Utf8JsonStream, options)) {

        Writer.WriteStartObject();

        Writer.WriteString(nameof(Name), Name);
        Writer.WriteString(nameof(KeywordsSelection), KeywordsSelection.ToString());
        Writer.WriteNumber(nameof(DaysBack), DaysBack);

        Writer.WriteEndObject();
      }

      return Encoding.UTF8.GetString(Utf8JsonStream.ToArray());
    }
  }
  #endregion --- ToJson --------------------------------------------

  #region --- ParseJson --------------------------------------------
  public RFilter ParseJson(JsonElement source) {
    return ParseJson(source.GetRawText());
  }

  public RFilter ParseJson(string source) {
    #region === Validate parameters ===
    if (string.IsNullOrWhiteSpace(source)) {
      throw new JsonException("Json filter source is null");
    }
    #endregion === Validate parameters ===

    try {
      JsonDocument JsonFilter = JsonDocument.Parse(source);
      JsonElement Root = JsonFilter.RootElement;

      //LogDebugEx(Root.GetRawText().BoxFixedWidth("RawText", 80, TextBox.EStringAlignment.Left));

      Name = Root.GetPropertyEx(nameof(Name)).GetString();
      KeywordsSelection = Enum.Parse<EFilterKeywords>(Root.GetPropertyEx(nameof(KeywordsSelection)).GetString());
      DaysBack = Root.GetPropertyEx(nameof(DaysBack)).GetInt32();

    } catch (Exception ex) {
      Logger?.LogError($"Unable to parse json : {ex.Message}");
    }

    return this;
  }
  #endregion --- ParseJson --------------------------------------------

  #region --- Static FromJson --------------------------------------------
  public static RFilter FromJson(JsonElement source) {
    if (source.ValueKind != JsonValueKind.Object) {
      throw new JsonException("Json movie source is not an object");
    }

    return FromJson(source.GetRawText());
  }

  public static RFilter FromJson(string source) {
    RFilter RetVal = new RFilter();
    return RetVal.ParseJson(source);
  }

  public void SetLogger(ILogger logger) {
    Logger = ALogger.Create(logger);
  }
  #endregion --- Static FromJson --------------------------------------------


}

