using System.Reflection;

namespace MediaSearch.Client.Services;

public static class GlobalSettings {
  public const int DEBUG_BOX_WIDTH = 100;
  public const int HTTP_TIMEOUT_IN_MS = 1000000;

  public static ILogger GlobalLogger { get; set; } = new TConsoleLogger();

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

    await ExecutingAbout.Initialize().ConfigureAwait(false);

    _IsInitializing = false;
    _IsInitialized = true;
  }

  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public static TAbout ExecutingAbout {
    get {
      return _ExecutingAbout ??= new TAbout(AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name == "MediaSearch.Client.Services"));
    }
  }
  private static TAbout? _ExecutingAbout;
}
