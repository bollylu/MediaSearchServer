using MediaSearch.Models.Support;

namespace MediaSearch.Models;
public class TMediaMovieParser : ALoggable, IMediaMovieParser {
  private readonly string _RootPath = "";
  private readonly bool ForWindows = true;
  private readonly bool ForLinux = false;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaMovieParser(string rootPath, bool forWindows = true) {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaMovieParser>();
    ForWindows = forWindows;
    ForLinux = !ForWindows;
    _RootPath = rootPath.NormalizePath(ForWindows).TrimStart('.');
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{nameof(_RootPath)} = {_RootPath.WithQuotes()}");
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public async Task<IMediaMovie?> ParseFile(string source) {
    if (source.IsEmpty()) {
      Logger.LogError("Unable to parse : source is missing");
      return null;
    }

    LogBox(nameof(source), source.ToString());

    if (!File.Exists(source)) {
      LogError($"Unable to parse : {source.WithQuotes()} is missing or access is denied");
      return null;
    }

    LogDebug($"{nameof(_RootPath)} = {_RootPath.WithQuotes()}");
    string FullName = Path.GetFullPath(source);
    LogDebug($"{nameof(FullName)} = {FullName.WithQuotes()}");

    string FullnameAfterRootPath = FullName.After(_RootPath);
    LogDebug($"{nameof(FullnameAfterRootPath)} = {FullnameAfterRootPath.WithQuotes()}");

    string Year = FullnameAfterRootPath.AfterLast('(').Before(')');
    _ = int.TryParse(Year, out int ConvertedYear);

    IPropertiesFinder Finder = new TFFProbe(FullName);
    await Finder.Init();

    TMediaInfo MediaInfo = new TMediaInfo() {
      Title = FullnameAfterRootPath.AfterLast(Path.DirectorySeparatorChar).BeforeLast(" ("),
      CreationDate = new DateOnly(ConvertedYear, 1, 1)
    };

    TMediaSourceVirtual MediaSource = new TMediaSourceVirtual() {
      FileName = FullnameAfterRootPath.AfterLast(Path.DirectorySeparatorChar).BeforeLast('.'),
      FileExtension = FullnameAfterRootPath.AfterLast("."),
      StoragePath = FullnameAfterRootPath.BeforeLast(Path.DirectorySeparatorChar),
      StorageRoot = _RootPath,
      CreationDate = new DateOnly(ConvertedYear, 1, 1),
      Size = (new FileInfo(FullName)).Length,
      DateAdded = DateOnly.FromDateTime(DateTime.Today)
    };
    MediaSource.Languages.Clear();
    foreach (TStreamAudioProperties StreamPropertyItem in Finder.MediaProperties.AudioProperties) {
      MediaSource.Languages.Add(ELanguageConverter.FromAudioStreamValue(StreamPropertyItem.Language));
    }

    if (MediaSource.Languages.Count > 1) {
      MediaSource.Languages.SetPrincipal(ELanguage.French);
    }

    foreach (var StreamPropertyItem in Finder.MediaProperties.StreamProperties) {
      MediaSource.Properties.AddProperty(
        new TMediaSourceProperty() {
          Name = StreamPropertyItem.Name,
          Value = StreamPropertyItem
        });
    }

    TMediaMovie RetVal = new TMediaMovie();
    RetVal.MediaSources.Add(MediaSource);

    return RetVal;
  }
}