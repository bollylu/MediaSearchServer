using System.Reflection;

namespace MediaSearch.Models;

public static class GlobalSettings {
  public const int DEBUG_BOX_WIDTH = 100;

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

    IJson.AddJsonConverter(new TAboutJsonConverter());
    IJson.AddJsonConverter(new TDateOnlyJsonConverter());
    IJson.AddJsonConverter(new TFilterJsonConverter());
    IJson.AddJsonConverter(new TIPAddressJsonConverter());
    IJson.AddJsonConverter(new TMovieJsonConverter());
    IJson.AddJsonConverter(new TMoviesPageJsonConverter());
    IJson.AddJsonConverter(new TUserAccountJsonConverter());
    IJson.AddJsonConverter(new TUserAccountSecretJsonConverter());
    IJson.AddJsonConverter(new TUserTokenJsonConverter());

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
