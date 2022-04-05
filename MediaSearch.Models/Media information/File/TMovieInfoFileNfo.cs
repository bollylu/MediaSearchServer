namespace MediaSearch.Models;

public class TMovieInfoFileNfo : IMediaInfoFile, IMediaSearchLoggable<TMovieInfoFileNfo> {

  public const int TIMEOUT_IN_MS = 5000;

  public IMediaSearchLogger<TMovieInfoFileNfo> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMovieInfoFileNfo>();

  #region --- IName --------------------------------------------
  public string Name { get; set; } = "";
  public string Description { get; set; } = "";
  #endregion --- IName --------------------------------------------

  #region --- Storage info --------------------------------------------
  public string StoragePath { get; set; } = "";
  public string StorageName { get; set; } = "";
  public string FullStorageName => Path.Join(StoragePath.NormalizePath(), StorageName);
  #endregion --- Storage info --------------------------------------------

  public IMediaInfoHeader Header => throw new NotImplementedException();

  #region --- Content --------------------------------------------
  public IMediaInfoContent Content {
    get {
      return _Content ??= new TMovieInfoContentNfo();
    }
    set {
      _Content = value;
    }
  }
  private IMediaInfoContent? _Content;
  public TMovieInfoContentNfo NfoContent => (TMovieInfoContentNfo)Content;

  private XElement? XmlContent;
  #endregion --- Content --------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMovieInfoFileNfo() { }
  public TMovieInfoFileNfo(string storagePath, string storageName) {
    StoragePath = storagePath;
    StorageName = storageName;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{nameof(Name)} = {Name.WithQuotes()}");
    RetVal.AppendLine($"{nameof(Description)} = {Description.WithQuotes()}");
    RetVal.AppendLine($"{nameof(StoragePath)} = {StoragePath.WithQuotes()}");
    RetVal.AppendLine($"{nameof(StorageName)} = {StorageName.WithQuotes()}");
    RetVal.AppendLine($"{nameof(FullStorageName)} = {FullStorageName.WithQuotes()}");
    RetVal.AppendLine($"{nameof(Content)} ({Content.GetType().Name})");
    RetVal.AppendLine($"{Content.ToString(2)}");
    return RetVal.ToString();
  } 
  #endregion --- Converters -------------------------------------------------------------------------------------

  #region --- IFileMediaInfo --------------------------------------------
  public bool Exists() {
    if (string.IsNullOrWhiteSpace(FullStorageName)) {
      return false;
    }
    return File.Exists(FullStorageName);
  }

  public bool Remove() {
    if (string.IsNullOrWhiteSpace(FullStorageName)) {
      return false;
    }
    try {
      File.Delete(FullStorageName);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to remove {FullStorageName}", ex);
      return false;
    }
  }

  public bool Read() {
    try {
      XDocument Document = XDocument.Load(FullStorageName);
      XElement Root = Document.Root ?? new XElement(TMovieInfoContentNfo.XML_THIS_ELEMENT);
      NfoContent.FromXml(Root);
      Logger.IfDebugMessageEx($"Read content of {FullStorageName}", Content);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to read {GetType().Name} : {FullStorageName}", ex);
      return false;
    }
  }

  public async Task<bool> ReadAsync(CancellationToken token) {
    try {
      XDocument Document;
      using (Stream InputStream = new FileStream(FullStorageName, new FileStreamOptions() {
                                                                    Access = FileAccess.Read,
                                                                    Mode = FileMode.Open,
                                                                    Share = FileShare.Read
                                                                  })) {
        Document = await XDocument.LoadAsync(InputStream, LoadOptions.None, token);
      }
      XElement Root = Document.Root ?? new XElement(TMovieInfoContentNfo.XML_THIS_ELEMENT);
      NfoContent?.FromXml(Root);
      Logger.IfDebugMessageEx($"Read content of {FullStorageName}", Content);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to read {GetType().Name} : {FullStorageName}", ex);
      return false;
    }
  }

  public bool Write() {
    try {
      Logger.IfDebugMessageEx($"Writing content to {FullStorageName}", XmlContent);
      XmlContent = NfoContent?.ToXml();
      XDocument Document = new XDocument();
      Document.Add(XmlContent);
      Document.Save(FullStorageName);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to write {GetType().Name} : {FullStorageName}", ex);
      return false;
    }
  }

  public async Task<bool> WriteAsync(CancellationToken token) {
    try {
      Logger.IfDebugMessageEx($"Writing content to {FullStorageName}", XmlContent);
      XmlContent = NfoContent?.ToXml();
      XDocument Document = new XDocument();
      Document.Add(XmlContent);
      using (Stream OutputStream = new FileStream(FullStorageName, new FileStreamOptions() {
        Access = FileAccess.Write,
        Mode = FileMode.Create,
        Share = FileShare.Read
      })) {
        await Document.SaveAsync(OutputStream, SaveOptions.None, token);
      }
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to write {GetType().Name} : {FullStorageName}", ex);
      return false;
    }
  }

  public bool Export(string newFilename) {
    throw new NotImplementedException();
  }

  public async Task<bool> ExportAsync(string newFilename, CancellationToken token) {
    await Task.Yield();
    throw new NotImplementedException();
  }
  #endregion --- IFileMediaInfo --------------------------------------------

}
