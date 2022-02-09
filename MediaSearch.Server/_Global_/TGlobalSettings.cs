using System.Reflection;

namespace MediaSearch.Server;

public class TGlobalSettings {
  public const int DEBUG_BOX_WIDTH = 100;

  public ISplitArgs AppArgs { get; } = new SplitArgs();

  public ILogger GlobalLogger { get; set; } = new TConsoleLogger();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TGlobalSettings() { }

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

    foreach (Assembly AssemblyItem in AppDomain.CurrentDomain.GetAssemblies().Where(a => (a.GetName()?.Name ?? "").StartsWith("MediaSearch"))) {
      IAbout AssemblyAbout = new TAbout(AssemblyItem);
      await AssemblyAbout.Initialize();
      About.Add(AssemblyAbout);
    }

    await EntryAbout.Initialize().ConfigureAwait(false);

    _IsInitializing = false;
    _IsInitialized = true;
  }

  #endregion --- Constructor(s) ------------------------------------------------------------------------------
  public List<IAbout> About { get; } = new();
  public TAbout EntryAbout => TAbout.Entry;

  public string ListAbout(bool withChangeLog = true) {
    StringBuilder RetVal = new();
    foreach (IAbout AboutItem in About) {
      RetVal.AppendLine(AboutItem.CurrentVersion.ToString().BoxFixedWidth($"{AboutItem.Name} version #", DEBUG_BOX_WIDTH));
      if (withChangeLog) {
        RetVal.AppendLine(AboutItem.ChangeLog.BoxFixedWidth($"Change log {AboutItem.Name}", DEBUG_BOX_WIDTH));
      }
    }
    return RetVal.ToString();
  }
}
