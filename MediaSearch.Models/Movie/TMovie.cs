using BLTools.Encryption;

namespace MediaSearch.Models;

public class TMovie : AMedia, IMovie, IJson<TMovie> {

  #region --- Public properties ------------------------------------------------------------------------------
  public EMovieExtension Extension =>
    FileExtension.ToLowerInvariant() switch {
      "avi" => EMovieExtension.AVI,
      "mkv" => EMovieExtension.MKV,
      "mp4" => EMovieExtension.MP4,
      "iso" => EMovieExtension.ISO,
      _ => EMovieExtension.Unknown,
    };

  public string SubGroup { get; set; } = "";

  public int OutputYear { get; set; }

  public IMovieInfoContent MovieInfoContent { get; } = new TMovieInfoContentMeta();
  public TMovieInfoContentMeta MovieInfoContentMeta => MovieInfoContent as TMovieInfoContentMeta ?? new TMovieInfoContentMeta();
  #endregion --- Public properties ---------------------------------------------------------------------------

  #region --- Picture --------------------------------------------
  public static byte[] PictureMissing {
    get {
      return _PictureMissingBytes ??= Support.GetPicture("missing", ".jpg");
    }
  }

  private static byte[]? _PictureMissingBytes;
  #endregion --- Picture --------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMovie() : base() { }

  public TMovie(IMovie movie) : base(movie) {
    Size = movie.Size;
    OutputYear = movie.OutputYear;
    Group = movie.Group;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  protected override string _BuildId() {
    return $"{Name}{OutputYear}".HashToBase64();
  }

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString() {
    StringBuilder RetVal = new StringBuilder(base.ToString());
    RetVal.AppendLine($"{nameof(Extension)} = {Extension}");
    if (IsGroupMember) {
      RetVal.AppendLine($"{nameof(Group)} = {Group}");
      RetVal.AppendLine($", {nameof(SubGroup)} = {SubGroup}");
    }
    RetVal.AppendLine($"{nameof(Size)} = {Size}");
    RetVal.AppendLine($"{nameof(OutputYear)} = {OutputYear}");

    return RetVal.ToString();
  }
  #endregion --- Converters ----------------------------------------------------------------------------------

  public static async Task<IMovie?> Parse(string moviePath, string rootStoragePath, ILogger logger) {
    return await Parse(new TFileInfo(moviePath), rootStoragePath, logger);
  }

  public static async Task<IMovie?> Parse(IFileInfo fileInfo, string rootStoragePath, ILogger logger) {

    char FOLDER_SEPARATOR = Path.DirectorySeparatorChar;

    // Standardize directory separator
    string ProcessedFileItem = fileInfo.FullName.NormalizePath();

    TMovie RetVal = new TMovie() { Name = ProcessedFileItem.AfterLast(FOLDER_SEPARATOR).BeforeLast(" (") };

    RetVal.StorageRoot = rootStoragePath.NormalizePath();
    RetVal.StoragePath = ProcessedFileItem.BeforeLast(FOLDER_SEPARATOR).After(RetVal.StorageRoot, System.StringComparison.InvariantCultureIgnoreCase);

    RetVal.FileName = fileInfo.Name;
    RetVal.FileExtension = RetVal.FileName.AfterLast('.').ToLowerInvariant();

    string[] Tags = RetVal.StoragePath.BeforeLast(FOLDER_SEPARATOR).Split(FOLDER_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
    string[] GroupTags = Tags.Where(t => t.EndsWith(" #")).ToArray();
    switch (GroupTags.Length) {
      case 0:
        RetVal.Group = "";
        RetVal.SubGroup = "";
        break;
      case 1:
        RetVal.Group = GroupTags[0];
        RetVal.SubGroup = "";
        break;
      case 2:
        RetVal.Group = GroupTags[0];
        RetVal.SubGroup = GroupTags[1];
        break;
      default:
        RetVal.Group = GroupTags[0];
        RetVal.SubGroup = GroupTags[1];
        logger.LogWarningBox($"Too much groups in path name", string.Join(", ", Tags));
        break;
    }

    foreach (string TagItem in Tags) {
      RetVal.Tags.Add(TagItem);
    }

    try {
      RetVal.OutputYear = int.Parse(RetVal.FileName.AfterLast('(').BeforeLast(')'));
    } catch (FormatException ex) {
      logger.LogErrorBox("Unable to find output year for {item.FullName}", ex);
      RetVal.OutputYear = 0;
    }

    RetVal.Size = fileInfo.Length;

    IMediaInfoFile DataFile = new TMovieInfoFileMeta(Path.Join(RetVal.StorageRoot, RetVal.StoragePath));
    if (await DataFile.ExistsAsync(CancellationToken.None)) {
      logger.LogDebugExBox("Found datafile", DataFile);
      if (!await DataFile.ReadAsync(CancellationToken.None)) {
        logger.LogWarning($"Unable to read datafile {DataFile.StorageName}");
      }
    }

    string CoverName = Path.Join(RetVal.StorageRoot, RetVal.StoragePath);

    return RetVal;

  }

}
