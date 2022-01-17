using System.Reflection;

namespace MediaSearch.Models;

public class TAbout : AJson<TAbout> {
  public string VersionSource { get; init; }
  public Version CurrentVersion {
    get {
      return _CurrentVersion ?? new Version(0, 0, 0);
    }
    set {
      _CurrentVersion = value;
    }
  }
  private Version _CurrentVersion;

  public string ChangeLogSource { get; init; }
  public string ChangeLog {
    get {
      return _ChangeLog ?? "";
    }
    set {
      _ChangeLog = value;
    }
  }
  private string _ChangeLog;

  public TAbout() { }
  public TAbout(string versionSource, string changeLogSource) {
    VersionSource = versionSource;
    ChangeLogSource = changeLogSource;
  }

  private bool _IsInitialized = false;
  private bool _IsInitializing = false;

  public async Task Initialize() {
    if (_IsInitialized) {
      return;
    }
    if (_IsInitializing) {
      return;
    }
    _IsInitializing = true;

    if (VersionSource is not null) {
      await ReadVersion(VersionSource);
    }

    if (ChangeLogSource is not null) {
      await ReadChangeLog(ChangeLogSource);
    }

    _IsInitializing = false;
    _IsInitialized = true;
  }

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
      //LogDebugEx(Root.GetRawText().BoxFixedWidth("RawText", 120, TextBox.EStringAlignment.Left));
      CurrentVersion = Version.Parse(Root.GetPropertyEx(nameof(CurrentVersion)).GetString());
      ChangeLog = Root.GetPropertyEx(nameof(ChangeLog)).GetString();
    } catch (Exception ex) {
      Logger?.LogError($"Unable to parse json : {ex.Message}");
    }
    return this;
  }

  public static TAbout Parse(string source) {
    TAbout RetVal = new TAbout();
    RetVal.ParseJson(source);
    return RetVal;
  }
  #endregion --- IJson --------------------------------------------

  public async Task ReadVersion(Stream source) {
    #region === Validate parameters ===
    if (source is null) {
      Logger?.LogError("Unable to read version : source is null");
      return;
    }
    #endregion === Validate parameters ===

    try {
      using (TextReader Reader = new StreamReader(stream: source, leaveOpen: true)) {
        CurrentVersion = Version.Parse(await Reader.ReadToEndAsync());
      }
    } catch (Exception ex) {
      Logger?.LogError($"Unable to read version : {ex.Message}");
      CurrentVersion = new Version(0, 0, 0);
    }

  }

  public async Task ReadVersion(string source) {
    #region === Validate parameters ===
    if (source is null) {
      Logger?.LogError("Unable to read version : source is null");
      return;
    }
    #endregion === Validate parameters ===

    try {
      Assembly Asm = Assembly.GetEntryAssembly();
      using (Stream VersionStream = Asm.GetManifestResourceStream(source)) {
        using (TextReader Reader = new StreamReader(VersionStream)) {
          CurrentVersion = Version.Parse(await Reader.ReadToEndAsync());
        }
      }
    } catch (Exception ex) {
      Logger?.LogError($"Unable to read version : {ex.Message}");
      CurrentVersion = new Version(0, 0);
    }

  }

  public async Task ReadChangeLog(Stream source) {
    #region === Validate parameters ===
    if (source is null) {
      Logger?.LogError("Unable to read change log : source is null");
      return;
    }
    #endregion === Validate parameters ===

    using (TextReader Reader = new StreamReader(stream: source, leaveOpen: true)) {
      ChangeLog = await Reader.ReadToEndAsync();
    }
  }

  public async Task ReadChangeLog(string source) {
    #region === Validate parameters ===
    if (source is null) {
      Logger?.LogError("Unable to read change log : source is null");
      return;
    }
    #endregion === Validate parameters ===

    Assembly Asm = Assembly.GetEntryAssembly();
    using (Stream ChangeLogStream = Asm.GetManifestResourceStream(source)) {
      using (TextReader Reader = new StreamReader(ChangeLogStream)) {
        ChangeLog = await Reader.ReadToEndAsync();
      }
    }
  }
}
