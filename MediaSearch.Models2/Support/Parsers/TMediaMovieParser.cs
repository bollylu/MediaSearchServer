namespace MediaSearch.Models;
public class TMediaMovieParser : ILoggable {
  private readonly string _RootPath = "";
  private readonly bool ForWindows = true;
  private readonly bool ForLinux = false;

  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TMediaMovieParser>();

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

  public IMediaMovie? Parse(IFileInfo source) {
    if (source is null) {
      Logger.LogError("Unable to parse : source is null");
      return null;
    }

    BLTools.Diagnostic.TraceInfo.Dump(source);
    Logger.Log($"{nameof(_RootPath)} = {_RootPath.WithQuotes()}");
    Logger.Log($"{nameof(source.FullName)} = {source.FullName.WithQuotes()}");

    string ParseSource = source.FullName.After(_RootPath);
    Logger.Log($"{nameof(ParseSource)} = {ParseSource.WithQuotes()}");

    string Year = ParseSource.AfterLast('(').Before(')');
    _ = int.TryParse(Year, out int ConvertedYear);

    TMediaMovie RetVal = new TMediaMovie() {
      MediaInfos = new TMediaInfos(
        new TMediaInfo() {
          Title = ParseSource.AfterLast(Path.DirectorySeparatorChar).BeforeLast(" ("),
          CreationDate = new DateOnly(ConvertedYear, 1, 1)
        }
      ),
      MediaSources = new TMediaSources(
        new TMediaSourceVirtual() {
          FileName = ParseSource.AfterLast(Path.DirectorySeparatorChar).BeforeLast('.'),
          FileExtension = ParseSource.AfterLast("."),
          StoragePath = ParseSource.BeforeLast(Path.DirectorySeparatorChar),
          StorageRoot = _RootPath,
          CreationDate = new DateOnly(ConvertedYear, 1, 1),
          Size = source.Length,
          DateAdded = DateOnly.FromDateTime(DateTime.Today)
        }
      )
    };

    return RetVal;
  }
}
