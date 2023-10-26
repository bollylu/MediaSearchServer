using MediaSearch.Models.Support;

namespace MediaSearch.Models;
public class TMediaMovieParserLinux : ALoggable, IMediaMovieParser {
  private const char DIRECTORY_SEPARATOR = '/';

  private readonly string _RootPath = "";

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaMovieParserLinux(string rootPath = ".") {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaMovieParserLinux>();
    _RootPath = rootPath.NormalizePath(false).TrimEnd(DIRECTORY_SEPARATOR);
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
    #region === Validate parameters ===
    if (source.IsEmpty()) {
      Logger.LogError("Unable to parse : source is missing");
      return null;
    }

    LogBox(nameof(source), source.ToString());

    if (!File.Exists(source)) {
      LogError($"Unable to parse : {source.WithQuotes()} is missing or access is denied");
      return null;
    }
    #endregion === Validate parameters ===

    LogDebug($"{nameof(_RootPath)} = {_RootPath.WithQuotes()}");
    string FullName = Path.GetFullPath(source);
    LogDebug($"{nameof(FullName)} = {FullName.WithQuotes()}");

    string FullnameAfterRootPath = FullName.After(_RootPath).After(DIRECTORY_SEPARATOR);
    LogDebug($"{nameof(FullnameAfterRootPath)} = {FullnameAfterRootPath.WithQuotes()}");

    string Year = FullnameAfterRootPath.AfterLast('(').Before(')');
    _ = int.TryParse(Year, out int ConvertedYear);
    LogDebug($"Year = {ConvertedYear}");

    IMediaSourceStreamsFinder Finder = new TFFProbe(FullName);
    await Finder.Init();

    #region --- MediaSource --------------------------------------------
    TMediaSourceVirtual MediaSource = new TMediaSourceVirtual() {
      FileName = FullnameAfterRootPath.AfterLast(DIRECTORY_SEPARATOR).BeforeLast('.'),
      FileExtension = FullnameAfterRootPath.AfterLast("."),
      StoragePath = FullnameAfterRootPath.BeforeLast(DIRECTORY_SEPARATOR),
      StorageRoot = _RootPath,
      CreationDate = new DateOnly(ConvertedYear, 1, 1),
      Size = (new FileInfo(FullName)).Length,
      DateAdded = DateOnly.FromDateTime(DateTime.Today)
    };

    MediaSource.Languages.Clear();

    foreach (var MediaStreamItem in Finder.MediaSourceStreams.GetAll()) {
      MediaSource.MediaStreams.Add(MediaStreamItem);
      if (MediaStreamItem is TMediaStreamAudio MediaStreamAudio) {
        MediaSource.Languages.Add(HLanguageConverter.FromAudioStreamValue(MediaStreamAudio.Language));
      }
    }

    if (MediaSource.Languages.Count > 1) {
      MediaSource.Languages.SetPrincipal(ELanguage.French);
    }
    #endregion --- MediaSource -----------------------------------------

    #region --- MediaInfos --------------------------------------------
    IMediaInfo MediaInfo = new TMediaInfo() {
      CreationDate = new DateOnly(ConvertedYear, 1, 1)
    };

    MediaInfo.Name = FullnameAfterRootPath.BeforeLast(" (").AfterLast(DIRECTORY_SEPARATOR);
    foreach (string GroupItem in MediaSource.StoragePath.Split(DIRECTORY_SEPARATOR)) {
      MediaInfo.Groups.Add(GroupItem);
    }
    #endregion --- MediaInfos -----------------------------------------

    #region --- MediaMovie --------------------------------------------
    TMediaMovie RetVal = new TMediaMovie();
    RetVal.MediaInfos.Add(MediaInfo);
    RetVal.MediaSources.Add(MediaSource);
    #endregion --- MediaMovie -----------------------------------------

    return RetVal;
  }

  public async Task<IMediaMovie?> ParseFolder(string source) {
    #region === Validate parameters ===
    if (source.IsEmpty()) {
      Logger.LogError("Unable to parse : source is missing");
      return null;
    }

    LogBox(nameof(source), source.ToString());

    if (!File.Exists(source)) {
      LogError($"Unable to parse : {source.WithQuotes()} is missing or access is denied");
      return null;
    }
    #endregion === Validate parameters ===

    LogDebug($"{nameof(_RootPath)} = {_RootPath.WithQuotes()}");
    string FullName = Path.GetFullPath(source);
    LogDebug($"{nameof(FullName)} = {FullName.WithQuotes()}");

    string FullnameAfterRootPath = FullName.After(_RootPath).After(DIRECTORY_SEPARATOR);
    LogDebug($"{nameof(FullnameAfterRootPath)} = {FullnameAfterRootPath.WithQuotes()}");

    string Year = FullnameAfterRootPath.AfterLast('(').Before(')');
    _ = int.TryParse(Year, out int ConvertedYear);
    LogDebug($"Year = {ConvertedYear}");

    IMediaSourceStreamsFinder Finder = new TFFProbe(FullName);
    await Finder.Init();

    #region --- MediaSource --------------------------------------------
    TMediaSourceVirtual MediaSource = new TMediaSourceVirtual() {
      FileName = FullnameAfterRootPath.AfterLast(DIRECTORY_SEPARATOR).BeforeLast('.'),
      FileExtension = FullnameAfterRootPath.AfterLast("."),
      StoragePath = FullnameAfterRootPath.BeforeLast(DIRECTORY_SEPARATOR),
      StorageRoot = _RootPath,
      CreationDate = new DateOnly(ConvertedYear, 1, 1),
      Size = (new FileInfo(FullName)).Length,
      DateAdded = DateOnly.FromDateTime(DateTime.Today)
    };

    MediaSource.Languages.Clear();

    foreach (var MediaStreamItem in Finder.MediaSourceStreams.GetAll()) {
      MediaSource.MediaStreams.Add(MediaStreamItem);
      if (MediaStreamItem is TMediaStreamAudio MediaStreamAudio) {
        MediaSource.Languages.Add(HLanguageConverter.FromAudioStreamValue(MediaStreamAudio.Language));
      }
    }

    if (MediaSource.Languages.Count > 1) {
      MediaSource.Languages.SetPrincipal(ELanguage.French);
    }
    #endregion --- MediaSource -----------------------------------------

    #region --- MediaInfos --------------------------------------------
    IMediaInfo MediaInfo = new TMediaInfo() {
      CreationDate = new DateOnly(ConvertedYear, 1, 1)
    };

    MediaInfo.Name = FullnameAfterRootPath.BeforeLast(" (").AfterLast(DIRECTORY_SEPARATOR);
    foreach (string GroupItem in MediaSource.StoragePath.Split(DIRECTORY_SEPARATOR)) {
      MediaInfo.Groups.Add(GroupItem);
    }
    #endregion --- MediaInfos -----------------------------------------

    #region --- MediaMovie --------------------------------------------
    TMediaMovie RetVal = new TMediaMovie();
    RetVal.MediaInfos.Add(MediaInfo);
    RetVal.MediaSources.Add(MediaSource);
    #endregion --- MediaMovie -----------------------------------------

    return RetVal;
  }
}