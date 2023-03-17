namespace MediaSearch.Models;
public class TMediaSourceMovieKodiParser : ALoggable, IMediaSourceParser {

  public static char FOLDER_SEPARATOR = Path.DirectorySeparatorChar;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSourceMovieKodiParser() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaSourceMovieKodiParser>();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public IMedia? ParseRow(IFileInfo item, string rootStoragePath) {
    // Standardize directory separator
    string ProcessedFileItem = item.FullName.NormalizePath();

    TMovie RetVal = new TMovie();
    TMediaSourceMovie Source = new TMediaSourceMovie();

    RetVal.Titles.Add(ELanguage.Unknown, ProcessedFileItem.AfterLast(FOLDER_SEPARATOR).BeforeLast(" ("));

    Source.StorageRoot = rootStoragePath.NormalizePath();
    Source.StoragePath = ProcessedFileItem.BeforeLast(FOLDER_SEPARATOR).After(Source.StorageRoot, System.StringComparison.InvariantCultureIgnoreCase);

    Source.FileName = item.Name;
    Source.FileExtension = Source.FileName.AfterLast('.').ToLowerInvariant();

    IEnumerable<string> Tags = Source.StoragePath
                                     .BeforeLast(FOLDER_SEPARATOR)
                                     .Split(FOLDER_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);

    foreach (string TagItem in Tags.Where(t => !t.EndsWith(" #"))) {
      RetVal.Tags.Add(TagItem.TrimStart('[').TrimEnd(']'));
    }

    IEnumerable<string> GroupTags = Tags.Where(t => t.EndsWith(" #"));
    RetVal.Group = string.Join("/", GroupTags);

    try {
      RetVal.CreationDate = new DateOnly(int.Parse(Source.FileName.AfterLast('(').BeforeLast(')')), 1, 1);
    } catch (FormatException ex) {
      Logger.LogWarningBox($"Unable to find output year", $"{ex.Message}\n{item.FullName}");
      RetVal.CreationDate = DateOnly.MinValue;
    }

    Source.Size = item.Length;

    IMediaInfoFile DataFile = new TMovieInfoFileMeta(Path.Join(Source.StorageRoot, Source.StoragePath));
    if (DataFile.Exists()) {
      Logger.LogDebugExBox("Found datafile", DataFile);
      DataFile.Read();
      //RetVal.MovieInfoContentMeta.Duplicate(DataFile.Content);
    }

    return RetVal;
  }

  public async Task<IMedia?> ParseRowAsync(IFileInfo item, string rootStoragePath, CancellationToken token) {
    // Standardize directory separator
    string ProcessedFileItem = item.FullName.NormalizePath();

    TMovie RetVal = new TMovie();
    TMediaSourceMovie Source = new TMediaSourceMovie();

    RetVal.Titles.Add(ELanguage.Unknown, ProcessedFileItem.AfterLast(FOLDER_SEPARATOR).BeforeLast(" ("));

    Source.StorageRoot = rootStoragePath.NormalizePath();
    Source.StoragePath = ProcessedFileItem.BeforeLast(FOLDER_SEPARATOR).After(Source.StorageRoot, System.StringComparison.InvariantCultureIgnoreCase);

    Source.FileName = item.Name;
    Source.FileExtension = Source.FileName.AfterLast('.').ToLowerInvariant();

    IEnumerable<string> Tags = Source.StoragePath
                                     .BeforeLast(FOLDER_SEPARATOR)
                                     .Split(FOLDER_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);

    foreach (string TagItem in Tags.Where(t => !t.EndsWith(" #"))) {
      RetVal.Tags.Add(TagItem.TrimStart('[').TrimEnd(']'));
    }

    IEnumerable<string> GroupTags = Tags.Where(t => t.EndsWith(" #"));
    RetVal.Group = string.Join("/", GroupTags);

    try {
      RetVal.CreationDate = new DateOnly(int.Parse(Source.FileName.AfterLast('(').BeforeLast(')')), 1, 1);
    } catch (FormatException ex) {
      Logger.LogWarningBox($"Unable to find output year", $"{ex.Message}\n{item.FullName}");
      RetVal.CreationDate = DateOnly.MinValue;
    }

    Source.Size = item.Length;

    IMediaInfoFile DataFile = new TMovieInfoFileMeta(Path.Join(Source.StorageRoot, Source.StoragePath));
    if (await DataFile.ExistsAsync(token)) {
      Logger.LogDebugExBox("Found datafile", DataFile);
      await DataFile.ReadAsync(token);
      //RetVal.MovieInfoContentMeta.Duplicate(DataFile.Content);
    }

    return RetVal;
  }
}
