namespace MediaSearch.Models;

public class TAbout : ALoggable, IAbout, IJson<TAbout> {

  private readonly Assembly? _Assembly;

  public string Name {
    get {
      return string.IsNullOrWhiteSpace(_Name) ? _Name = _Assembly?.GetName().Name ?? "" : _Name;
    }
    set {
      _Name = value;
    }
  }
  private string? _Name;

  public string Description {
    get {
      return string.IsNullOrWhiteSpace(_Description) ? _Description = _Assembly?.GetName().FullName ?? "" : _Description;
    }
    set {
      _Description = value;
    }
  }
  private string? _Description;

  public string VersionSource { get; init; } = "_global_.version.txt";
  public Version CurrentVersion {
    get {
      return _CurrentVersion ??= new Version(0, 0, 0);
    }
    set {
      _CurrentVersion = value;
    }
  }
  private Version? _CurrentVersion;

  public string ChangeLogSource { get; init; } = "_global_.changelog.txt";
  public string ChangeLog {
    get {
      return _ChangeLog ??= "";
    }
    set {
      _ChangeLog = value;
    }
  }
  private string? _ChangeLog;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TAbout() { }

  public TAbout(Assembly? assembly) {
    _Assembly = assembly;
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
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override string ToString() {
    StringBuilder RetVal = new();
    RetVal.AppendLine($"{nameof(Name)} : {Name}");
    RetVal.AppendLine($"{nameof(Description)} : {Description}");
    RetVal.AppendLine($"{nameof(CurrentVersion)} : {CurrentVersion}");
    RetVal.AppendLine($"{nameof(ChangeLog)} : {ChangeLog}");
    return RetVal.ToString();
  }
  #region --- IJson --------------------------------------------
  public static JsonSerializerOptions DefaultJsonSerializerOptions {
    get {
      lock (_DefaultJsonSerializerOptionsLock) {
        if (_DefaultJsonSerializerOptions is null) {
          _DefaultJsonSerializerOptions = new JsonSerializerOptions() {
            WriteIndented = true,
            NumberHandling = JsonNumberHandling.Strict
          };
          _DefaultJsonSerializerOptions.Converters.Add(new TAboutJsonConverter());
        }
        return _DefaultJsonSerializerOptions;
      }
    }
    set {
      lock (_DefaultJsonSerializerOptionsLock) {
        _DefaultJsonSerializerOptions = value;
      }
    }
  }
  private static JsonSerializerOptions? _DefaultJsonSerializerOptions;
  private static readonly object _DefaultJsonSerializerOptionsLock = new object();

  public string ToJson() {
    return JsonSerializer.Serialize(this, DefaultJsonSerializerOptions);
  }

  public string ToJson(JsonSerializerOptions options) {
    return JsonSerializer.Serialize(this, options);
  }

  public TAbout ParseJson(string source) {
    return ParseJson(source, DefaultJsonSerializerOptions);
  }

  public TAbout ParseJson(string source, JsonSerializerOptions options) {
    #region === Validate parameters ===
    if (string.IsNullOrWhiteSpace(source)) {
      throw new JsonException("Json filter source is null");
    }
    #endregion === Validate parameters ===

    TAbout? Deserialized = JsonSerializer.Deserialize<TAbout>(source, options);
    if (Deserialized is null) {
      string Error = $"Unable to deserialize json string \"{source}\"";
      LogError(Error);
      throw new JsonException(Error);
    }

    CurrentVersion = Deserialized.CurrentVersion;
    ChangeLog = Deserialized.ChangeLog;

    return this;
  }

  #region --- Static Deserializer --------------------------------------------

  public static TAbout? FromJson(string source) {
    if (string.IsNullOrWhiteSpace(source)) {
      throw new ArgumentNullException(nameof(source));
    }
    return JsonSerializer.Deserialize<TAbout>(source, DefaultJsonSerializerOptions);
  }

  public static TAbout? FromJson(string source, JsonSerializerOptions options) {
    if (string.IsNullOrWhiteSpace(source)) {
      throw new ArgumentNullException(nameof(source));
    }
    return JsonSerializer.Deserialize<TAbout>(source, options);
  }


  #endregion --- Static Deserializer --------------------------------------------
  #endregion --- IJson --------------------------------------------

  #region --- I/O --------------------------------------------
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
    if (_Assembly is null) {
      throw new InvalidOperationException("Unable to read version : assembly is null");
    }

    if (source is null) {
      Logger?.LogError("Unable to read version : source is null");
      return;
    }
    #endregion === Validate parameters ===

    try {
      string? ResourceName = _GetResourceNameCaseInsensitive(_Assembly, source);
      if (ResourceName is null) {
        return;
      }
      using (Stream? VersionStream = _Assembly.GetManifestResourceStream(ResourceName)) {
        if (VersionStream is null) {
          return;
        }
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
    if (_Assembly is null) {
      throw new InvalidOperationException("Unable to read changelog : assembly is null");
    }

    if (source is null) {
      Logger?.LogError("Unable to read change log : source is null");
      return;
    }
    #endregion === Validate parameters ===

    try {
      string? ResourceName = _GetResourceNameCaseInsensitive(_Assembly, source);
      if (ResourceName is null) {
        return;
      }
      using (Stream? ChangeLogStream = _Assembly.GetManifestResourceStream(ResourceName)) {
        if (ChangeLogStream is null) {
          return;
        }
        using (TextReader Reader = new StreamReader(ChangeLogStream)) {
          ChangeLog = await Reader.ReadToEndAsync();
        }
      }
    } catch (Exception ex) {
      Logger?.LogError($"Unable to read changelog : {ex.Message}");
      CurrentVersion = new Version(0, 0);
    }
  }

  private static string? _GetResourceNameCaseInsensitive(Assembly assembly, string resourceName) {
    if (assembly is null) {
      throw new InvalidOperationException("Unable to search for resource : assembly is null");
    }
    if (string.IsNullOrWhiteSpace(resourceName)) {
      return null;
    }
    string FullResourceName = $"{assembly.GetName().Name}.{resourceName}".ToLowerInvariant();
    string? RetVal = assembly.GetManifestResourceNames().FirstOrDefault(x => x.ToLowerInvariant() == FullResourceName);
    return RetVal;
  }
  #endregion --- I/O --------------------------------------------

  #region --- Static instances --------------------------------------------
  [JsonIgnore]
  public static TAbout Empty => _Empty ??= new TAbout();
  private static TAbout? _Empty;

  [JsonIgnore]
  public static TAbout Entry => _Entry ??= new TAbout(Assembly.GetEntryAssembly());
  private static TAbout? _Entry;

  [JsonIgnore]
  public static TAbout Executing => _Executing ??= new TAbout(Assembly.GetExecutingAssembly());
  private static TAbout? _Executing;

  [JsonIgnore]
  public static TAbout Calling => _Calling ??= new TAbout(Assembly.GetCallingAssembly());
  private static TAbout? _Calling;
  #endregion --- Static instances --------------------------------------------
}
