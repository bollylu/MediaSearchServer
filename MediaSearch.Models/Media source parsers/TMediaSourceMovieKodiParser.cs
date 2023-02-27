﻿namespace MediaSearch.Models;
public class TMediaSourceMovieKodiParser : IMediaSourceParser, ILoggable {

  public static char FOLDER_SEPARATOR = Path.DirectorySeparatorChar;

  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TMediaSourceMovieKodiParser>();

  public IMedia? ParseRow(IFileInfo item, string rootStoragePath) {
    // Standardize directory separator
    string ProcessedFileItem = item.FullName.NormalizePath();

    TMovie RetVal = new TMovie();

    RetVal.Titles.Add(ELanguage.Unknown, ProcessedFileItem.AfterLast(FOLDER_SEPARATOR).BeforeLast(" ("));

    RetVal.StorageRoot = rootStoragePath.NormalizePath();
    RetVal.StoragePath = ProcessedFileItem.BeforeLast(FOLDER_SEPARATOR).After(RetVal.StorageRoot, System.StringComparison.InvariantCultureIgnoreCase);

    RetVal.FileName = item.Name;
    RetVal.FileExtension = RetVal.FileName.AfterLast('.').ToLowerInvariant();

    IEnumerable<string> Tags = RetVal.StoragePath
                                     .BeforeLast(FOLDER_SEPARATOR)
                                     .Split(FOLDER_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);

    foreach (string TagItem in Tags.Where(t => !t.EndsWith(" #"))) {
      RetVal.Tags.Add(TagItem.TrimStart('[').TrimEnd(']'));
    }

    IEnumerable<string> GroupTags = Tags.Where(t => t.EndsWith(" #"));
    RetVal.Group = string.Join("/", GroupTags);

    try {
      RetVal.CreationDate = new DateOnly(int.Parse(RetVal.FileName.AfterLast('(').BeforeLast(')')), 1, 1);
    } catch (FormatException ex) {
      Logger.LogWarningBox($"Unable to find output year", $"{ex.Message}\n{item.FullName}");
      RetVal.CreationDate = DateOnly.MinValue;
    }

    RetVal.Size = item.Length;

    IMediaInfoFile DataFile = new TMovieInfoFileMeta(Path.Join(RetVal.StorageRoot, RetVal.StoragePath));
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

    RetVal.Titles.Add(ELanguage.Unknown, ProcessedFileItem.AfterLast(FOLDER_SEPARATOR).BeforeLast(" ("));

    RetVal.StorageRoot = rootStoragePath.NormalizePath();
    RetVal.StoragePath = ProcessedFileItem.BeforeLast(FOLDER_SEPARATOR).After(RetVal.StorageRoot, System.StringComparison.InvariantCultureIgnoreCase);

    RetVal.FileName = item.Name;
    RetVal.FileExtension = RetVal.FileName.AfterLast('.').ToLowerInvariant();

    IEnumerable<string> Tags = RetVal.StoragePath
                                     .BeforeLast(FOLDER_SEPARATOR)
                                     .Split(FOLDER_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);

    foreach (string TagItem in Tags.Where(t => !t.EndsWith(" #"))) {
      RetVal.Tags.Add(TagItem.TrimStart('[').TrimEnd(']'));
    }

    IEnumerable<string> GroupTags = Tags.Where(t => t.EndsWith(" #"));
    RetVal.Group = string.Join("/", GroupTags);

    try {
      RetVal.CreationDate = new DateOnly(int.Parse(RetVal.FileName.AfterLast('(').BeforeLast(')')), 1, 1);
    } catch (FormatException ex) {
      Logger.LogWarningBox($"Unable to find output year", $"{ex.Message}\n{item.FullName}");
      RetVal.CreationDate = DateOnly.MinValue;
    }

    RetVal.Size = item.Length;

    IMediaInfoFile DataFile = new TMovieInfoFileMeta(Path.Join(RetVal.StorageRoot, RetVal.StoragePath));
    if (await DataFile.ExistsAsync(token)) {
      Logger.LogDebugExBox("Found datafile", DataFile);
      await DataFile.ReadAsync(token);
      //RetVal.MovieInfoContentMeta.Duplicate(DataFile.Content);
    }

    return RetVal;
  }
}