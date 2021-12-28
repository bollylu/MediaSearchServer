namespace MediaSearch.Models;

public class TAbout : AJson<TAbout> {
  public Version CurrentVersion { get; set; }
  public string ChangeLog { get; set; }

  #region --- IJson --------------------------------------------
  public override string ToJson(JsonWriterOptions options) {

    using (MemoryStream Utf8JsonStream = new()) {
      using (Utf8JsonWriter Writer = new Utf8JsonWriter(Utf8JsonStream, options)) {

        Writer.WriteStartObject();
        Writer.WriteString(nameof(CurrentVersion), CurrentVersion.ToString());
        Writer.WriteString(nameof(ChangeLog), ChangeLog);

        Writer.WriteEndObject();
      }

      return Encoding.UTF8.GetString(Utf8JsonStream.ToArray());
    }
  }

  public override TAbout ParseJson(string source) {
    #region === Validate parameters ===
    if (string.IsNullOrWhiteSpace(source)) {
      throw new JsonException("Json filter source is null");
    }
    #endregion === Validate parameters ===

    try {
      JsonDocument JsonFilter = JsonDocument.Parse(source);
      JsonElement Root = JsonFilter.RootElement;

      //LogDebugEx(Root.GetRawText().BoxFixedWidth("RawText", 80, TextBox.EStringAlignment.Left));

      CurrentVersion = Version.Parse(Root.GetPropertyEx(nameof(CurrentVersion)).GetString());
      ChangeLog = Root.GetPropertyEx(nameof(ChangeLog)).GetString();

    } catch (Exception ex) {
      Logger?.LogError($"Unable to parse json : {ex.Message}");
    }

    return this;
  }
  #endregion --- IJson --------------------------------------------

  public void ReadChangeLog(string source) {
    if (string.IsNullOrWhiteSpace(source)) {
      Logger?.LogError("Unable to read change log : source is null or invalid");
      return;
    }
    
  }
}
