namespace MediaSearch.Models;
public class TMediaMovieParser : ALoggable {
  private readonly string _RootPath = "";

  public TMediaMovieParser(string rootPath) {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaMovieParser>();
    _RootPath = rootPath;
  }

  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{nameof(_RootPath)} = {_RootPath.WithQuotes()}");
    return RetVal.ToString();
  }

  public IMediaMovie? Parse(IFileInfo source) {
    if (source is null) {
      LogError("Unable to parse : source is null");
      return null;
    }

    string ParseSource = source.FullName.After(_RootPath);
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
