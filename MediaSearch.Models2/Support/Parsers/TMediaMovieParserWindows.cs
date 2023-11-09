using System.Collections.Concurrent;

using MediaSearch.Models.Support;

namespace MediaSearch.Models;
public class TMediaMovieParserWindows : AMediaMovieParser {
  private const char DIRECTORY_SEPARATOR = '\\';

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaMovieParserWindows(string rootPath = ".") : base() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaMovieParserWindows>();
    _RootPath = Path.GetFullPath(rootPath.NormalizePath(true)).TrimEnd(DIRECTORY_SEPARATOR);
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{nameof(_RootPath)} = {_RootPath.WithQuotes()}");
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public override async Task<IMediaMovie?> ParseFile(string source, CancellationToken token) {
    LastParseCount++;

    #region === Validate parameters ===
    if (source.IsEmpty()) {
      LastErrorCount++;
      LogError("Unable to parse : source is missing");
      return null;
    }

    LogDebugBox(nameof(source), source.ToString());

    if (!File.Exists(source)) {
      LastErrorCount++;
      LogError($"Unable to parse : {source.WithQuotes()} is missing or access is denied");
      return null;
    }
    #endregion === Validate parameters ===

    NotifyParseFileStarting(source);

    if (token.IsCancellationRequested) {
      return null;
    }

    try {
      LogDebug($"{nameof(_RootPath)} = {_RootPath.WithQuotes()}");
      string FullName = Path.GetFullPath(source);
      LogDebug($"{nameof(FullName)} = {FullName.WithQuotes()}");

      string FullnameAfterRootPath = FullName.After(_RootPath).After(DIRECTORY_SEPARATOR);
      LogDebug($"{nameof(FullnameAfterRootPath)} = {FullnameAfterRootPath.WithQuotes()}");

      string Year = FullnameAfterRootPath.AfterLast('(').Before(')');
      _ = int.TryParse(Year, out int ConvertedYear);
      LogDebug($"Year = {ConvertedYear}");

      if (token.IsCancellationRequested) {
        return null;
      }

      IMediaSourceStreamsFinder Finder = new TFFProbe(FullName);
      await Finder.Init();

      if (token.IsCancellationRequested) {
        return null;
      }

      #region --- MediaSource --------------------------------------------
      IMediaSourceVirtual MediaSource = new TMediaSourceVirtual() {
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
          ELanguage NewLanguage = HLanguageConverter.FromAudioStreamValue(MediaStreamAudio.Language);
          if (!MediaSource.Languages.TryAdd(NewLanguage)) {
            LogDebug($"Duplicate language : {NewLanguage}");
            MediaSource.Languages.Add(NewLanguage);
          }
        }
      }

      if (MediaSource.Languages.Count > 1) {
        MediaSource.Languages.SetPrincipal(MediaSource.Languages.First());
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
      IMediaMovie RetVal = new TMediaMovie();
      RetVal.MediaInfos.Add(MediaInfo);
      RetVal.MediaSources.Add(MediaSource);

      NotifyParseFileCompleted(source);
      LastSuccessCount++;
      return RetVal;
      #endregion --- MediaMovie -----------------------------------------

    } catch (Exception ex) {
      LastErrorCount++;
      NotifyParseFileCompleted($"Error parsing {source}");
      LogErrorBox($"Error parsing {source}", ex);
      return null;
    }

  }

}