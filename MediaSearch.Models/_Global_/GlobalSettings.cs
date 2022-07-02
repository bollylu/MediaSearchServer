using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace MediaSearch.Models;

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
#if DEBUG
    IJson.DefaultJsonSerializerOptions.WriteIndented = true;
#else
    IJson.DefaultJsonSerializerOptions.WriteIndented = false;
#endif
    IJson.DefaultJsonSerializerOptions.NumberHandling = JsonNumberHandling.Strict;
    IJson.DefaultJsonSerializerOptions.DictionaryKeyPolicy = new TIdenticalJsonNamingPolicy();
    IJson.DefaultJsonSerializerOptions.IgnoreReadOnlyFields = true;
    IJson.DefaultJsonSerializerOptions.IgnoreReadOnlyProperties = true;
    IJson.DefaultJsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement);
    IJson.DefaultJsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    IJson.DefaultJsonSerializerOptions.Converters.Add(new TLanguageDictionaryStringConverter());
    IJson.DefaultJsonSerializerOptions.Converters.Add(new TAboutJsonConverter());
    IJson.DefaultJsonSerializerOptions.Converters.Add(new TFilterJsonConverter());
    IJson.DefaultJsonSerializerOptions.Converters.Add(new TMovieJsonConverter());
    IJson.DefaultJsonSerializerOptions.Converters.Add(new TMoviesPageJsonConverter());
    IJson.DefaultJsonSerializerOptions.Converters.Add(new TUserAccountInfoJsonConverter());
    IJson.DefaultJsonSerializerOptions.Converters.Add(new TUserAccountJsonConverter());
    IJson.DefaultJsonSerializerOptions.Converters.Add(new TUserAccountSecretJsonConverter());
    IJson.DefaultJsonSerializerOptions.Converters.Add(new TUserTokenJsonConverter());
    IJson.DefaultJsonSerializerOptions.Converters.Add(new TMovieInfoContentMetaJsonConverter());
    IJson.DefaultJsonSerializerOptions.Converters.Add(new TLanguageTextInfoJsonConverter());
    IJson.DefaultJsonSerializerOptions.Converters.Add(new TLanguageTextInfosJsonConverter());
    IJson.DefaultJsonSerializerOptions.Converters.Add(new TMovieInfoFileMetaJsonConverter());
    IJson.DefaultJsonSerializerOptions.Converters.Add(new TMediaSourceJsonConverter());
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

  public static string AssemblyName => _AssemblyName ??= $"{nameof(MediaSearch)}.{nameof(Models)}";
  private static string? _AssemblyName;

  public static Type? GetType(string typeName) {
    return Type.GetType($"{AssemblyName}.{typeName},{AssemblyName}");
  }
}
