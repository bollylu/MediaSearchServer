﻿using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace MediaSearch.Client;

public static class GlobalSettings {
  public const int DEBUG_BOX_WIDTH = 110;

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
      return _ExecutingAbout ??= new TAbout(AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name == "MediaSearch.Client"));
    }
  }
  private static TAbout? _ExecutingAbout;

  public static IUserAccountInfo? Account {get; set; }

  public static IApiServer? ApiServer { get; set; }
}
