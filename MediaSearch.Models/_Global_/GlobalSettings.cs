﻿using System.Reflection;

namespace MediaSearch.Models;

public static class GlobalSettings {
  public const int DEBUG_BOX_WIDTH = 100;

  public static IMediaSearchLogger GlobalLogger { get; set; } = new TMediaSearchLoggerConsole();

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

  public static TAbout ExecutingAbout {
    get {
      return _ExecutingAbout ??= new TAbout(AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name == "MediaSearch.Models"));
    }
  }
  private static TAbout? _ExecutingAbout;

  
}
