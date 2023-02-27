namespace MediaSearch.Models;

public class TMediaInfoFileNfo : ALoggable, IMediaInfoFile, IToXml {

  #region --- XML constants ----------------------------------------------------------------------------------
  public const string XML_THIS_ELEMENT = "movie";
  public const string XML_ELEMENT_TITLE = "title";
  public const string XML_ELEMENT_ORIGINAL_TITLE = "originaltitle";
  public const string XML_ELEMENT_DESCRIPTION = "plot";
  public const string XML_ELEMENT_KODI_ID = "id";
  public const string XML_ELEMENT_UNIQUE_ID = "uniqueid";
  public const string XML_ELEMENT_GENRE = "genre";
  public const string XML_ELEMENT_TAGS = "TAG";
  public const string XML_ELEMENT_COUNTRY = "country";
  public const string XML_ELEMENT_PREMIERED = "premiered";
  #endregion --- XML constants -------------------------------------------------------------------------------

  public string StoragePath { get; set; } = "";
  public string StorageName { get; set; } = "";

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaInfoFileNfo() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaInfoFileNfo>();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- IFileMediaInfo --------------------------------------------
  public IMediaInfoContent Content { get; } = new TMediaInfoContentNfo();

  public string Country { get; set; } = "";

  public string Name => Path.Combine(StoragePath, StorageName);

  public ValueTask<bool> ExportAsync(string newFilename, CancellationToken token) {
    throw new NotImplementedException();
  }

  public ValueTask<bool> ExistsAsync(CancellationToken token) {
    throw new NotImplementedException();
  }
  public ValueTask<bool> ReadAsync(CancellationToken token) {
    throw new NotImplementedException();
  }

  public ValueTask<bool> WriteAsync(CancellationToken token) {
    throw new NotImplementedException();
  }

  public bool Exists() {
    throw new NotImplementedException();
  }
  public bool Read() {
    throw new NotImplementedException();
  }

  public bool Write() {
    throw new NotImplementedException();
  }
  #endregion --- IFileMediaInfo --------------------------------------------

  #region --- IToXml --------------------------------------------
  public XElement ToXml() {
    throw new NotImplementedException();
  }

  public void FromXml(XElement source) {
    if (source is null) {
      Logger.LogError("Unable to convert from Xml : source is null");
      return;
    }

    Country = source.SafeReadElementValue(XML_ELEMENT_COUNTRY, "FR");
    switch (Country) {

      case "FR": {
          Content.Titles.Add(ELanguage.French, source.SafeReadElementValue(XML_ELEMENT_TITLE, ""));
          break;
        }

      default:
      case "US": {
          Content.Titles.Add(ELanguage.English, source.SafeReadElementValue(XML_ELEMENT_TITLE, ""));
          break;
        }

    }

  }
  #endregion --- IToXml --------------------------------------------

  IMediaInfoContent? IMediaInfoFile.Content { get; set; }

  public ILanguageDictionary<string> GetTitles() {
    throw new NotImplementedException();
  }

  public ILanguageDictionary<string> GetDescription() {
    throw new NotImplementedException();
  }



}
