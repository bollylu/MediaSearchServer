﻿using MediaSearch.Models2.Support.About;

namespace MediaSearch.Storage;

public static class GlobalSettings {
  public const int DEBUG_BOX_WIDTH = 110;

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

    await ExecutingAbout.Initialize().ConfigureAwait(false);

    _IsInitializing = false;
    _IsInitialized = true;
  }

  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- About --------------------------------------------
  public static TAbout ExecutingAbout {
    get {
      return _ExecutingAbout ??= new TAbout(AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name == "MediaSearch.Storage2"));
    }
  }
  private static TAbout? _ExecutingAbout;

  public static TAbout CallingAbout => TAbout.Calling;
  #endregion --- About --------------------------------------------

}
