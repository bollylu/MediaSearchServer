using System.Reflection;

namespace MediaSearch.Server;

public static class GlobalSettings {
  public const int DEBUG_BOX_WIDTH = 100;

  public static ISplitArgs AppArgs { get; } = new SplitArgs();

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

    foreach (Assembly AssemblyItem in AppDomain.CurrentDomain.GetAssemblies().Where(a => (a.GetName()?.Name ?? "").StartsWith("MediaSearch"))) {
      IAbout AssemblyAbout = new TAbout(AssemblyItem);
      await AssemblyAbout.Initialize();
      Abouts.Add(AssemblyAbout);
    }

    await EntryAbout.Initialize().ConfigureAwait(false);

    _IsInitializing = false;
    _IsInitialized = true;
  }

  #endregion --- Constructor(s) ------------------------------------------------------------------------------
  
  public static TAbout EntryAbout => TAbout.Entry;

  public static TAbout ExecutingAbout {
    get {
      return _ExecutingAbout ??= new TAbout(AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name == "MediaSearch.Server"));
    }
  }
  private static TAbout? _ExecutingAbout;

  public static List<IAbout> Abouts { get; } = new();
  public static string ListAbout(bool withChangeLog = true) {
    StringBuilder RetVal = new();
    foreach (IAbout AboutItem in Abouts) {
      RetVal.AppendLine(AboutItem.CurrentVersion.ToString().BoxFixedWidth($"{AboutItem.Name} version #", DEBUG_BOX_WIDTH));
      if (withChangeLog) {
        RetVal.AppendLine(AboutItem.ChangeLog.BoxFixedWidth($"Change log {AboutItem.Name}", DEBUG_BOX_WIDTH));
      }
    }
    return RetVal.ToString();
  }
}
