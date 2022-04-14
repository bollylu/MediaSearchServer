using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace MediaSearch.Client.Services;

public static class GlobalSettings {
  public const int DEBUG_BOX_WIDTH = 110;
  public const int HTTP_TIMEOUT_IN_MS = 1000000;

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
      return _ExecutingAbout ??= new TAbout(AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name == "MediaSearch.Client.Services"));
    }
  }
  private static TAbout? _ExecutingAbout;

  public static JsonSerializerOptions DefaultJsonSerializerOptions {
    get {
      lock (_DefaultJsonSerializerOptionsLock) {
        if (_DefaultJsonSerializerOptions is null) {
          _DefaultJsonSerializerOptions = new JsonSerializerOptions() {
#if DEBUG
            WriteIndented = true,
#else
            WriteIndented = false,
#endif
            NumberHandling = JsonNumberHandling.Strict,
            DictionaryKeyPolicy = new TIdenticalJsonNamingPolicy(),
            IgnoreReadOnlyFields = true,
            IgnoreReadOnlyProperties = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement)
          };
          _DefaultJsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TAboutJsonConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TDateOnlyJsonConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TFilterJsonConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TIPAddressJsonConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TMovieJsonConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TMoviesPageJsonConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TUserAccountJsonConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TUserAccountSecretJsonConverter());
          _DefaultJsonSerializerOptions.Converters.Add(new TUserTokenJsonConverter());
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
}
