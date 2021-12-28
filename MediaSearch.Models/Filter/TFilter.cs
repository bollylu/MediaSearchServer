using System.Drawing;
using System.Text.RegularExpressions;

namespace MediaSearch.Models;

public class TFilter : AJson<TFilter> {

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

  public static TFilter Empty => _Empty ??= new TFilter();
  private static TFilter _Empty;

  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.Append($"Keywords={Name.WithQuotes()}");
    RetVal.Append($", KeywordsSelection={KeywordsSelection}");
    RetVal.Append($", DaysBack={DaysBack}");
    return RetVal.ToString();
  }

  #region --- IJson --------------------------------------------
  public override string ToJson(JsonWriterOptions options) {

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

      Name = Root.GetPropertyEx(nameof(Name)).GetString();
      KeywordsSelection = Enum.Parse<EFilterKeywords>(Root.GetPropertyEx(nameof(KeywordsSelection)).GetString());
      DaysBack = Root.GetPropertyEx(nameof(DaysBack)).GetInt32();

    } catch (Exception ex) {
      Logger?.LogError($"Unable to parse json : {ex.Message}");
    }

    return this;
  } 
  #endregion --- IJson --------------------------------------------

}

