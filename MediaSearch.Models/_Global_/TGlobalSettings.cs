﻿using System.Reflection;

namespace MediaSearch.Models;

public static class GlobalSettings {
  public const int DEBUG_BOX_WIDTH = 100;

    public static ILogger GlobalLogger { get; set; } = new TConsoleLogger();

  #region --- Constructor(s) ---------------------------------------------------------------------------------

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
  public static TAbout ExecutingAbout => TAbout.Executing;
  
}
