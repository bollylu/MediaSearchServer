
namespace MediaSearch.Database;

public static class GlobalSettings {
  public const int DEBUG_BOX_WIDTH = 110;

  public static ILogger GlobalLogger { get; set; } = new TConsoleLogger();

  public static TLoggerPool LoggerPool { get; } = new();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  static GlobalSettings() {
    Initialize().Wait();
  }

  private static bool _IsInitialized = false;
  private static bool _IsInitializing = false;
  public static async Task Initialize() {
    if (_IsInitialized) {
      return;
    }
    if (_IsInitializing) {
      return;
    }
    _IsInitializing = true;

    IJson.DefaultJsonSerializerOptions.Converters.Add(new TTableHeaderJsonConverter());
    IJson.DefaultJsonSerializerOptions.Converters.Add(new TMediaSourceJsonConverter());

    await ExecutingAbout.Initialize().ConfigureAwait(false);

    _IsInitializing = false;
    _IsInitialized = true;
  }

  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public static TAbout ExecutingAbout {
    get {
      return _ExecutingAbout ??= new TAbout(AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name == "MediaSearch.Database"));
    }
  }
  private static TAbout? _ExecutingAbout;

  public static string AssemblyName => _AssemblyName ??= $"{nameof(MediaSearch)}.{nameof(Database)}";
  private static string? _AssemblyName;

  public static Type? GetType(string typeName) {
    return Type.GetType($"{AssemblyName}.{typeName},{AssemblyName}");
  }
}
