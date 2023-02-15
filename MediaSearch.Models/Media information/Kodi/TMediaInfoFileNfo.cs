namespace MediaSearch.Models;

public class TMediaInfoFileNfo : ALoggable, IMediaInfoFile, IToXml {

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

  public string StoragePath { get; set; } = "";
  public string StorageName { get; set; } = "";

  public TMediaInfoFileNfo() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaInfoFileNfo>();
  }

  #region --- IFileMediaInfo --------------------------------------------
  public IMediaInfoContent Content { get; } = new TMediaInfoContentNfo();

  public string Country { get; set; } = "";

  public string Name => Path.Combine(StoragePath, StorageName);

  public Task<bool> Export(string newFilename) {
    throw new NotImplementedException();
  }

  public Task<bool> Read() {
    throw new NotImplementedException();
  }

  public Task<bool> Write() {
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



  public Task<bool> Exists() {
    throw new NotImplementedException();
  }


  IMediaInfoContent? IMediaInfoFile.Content { get; set; }

  public ILanguageDictionary<string> GetTitles() {
    throw new NotImplementedException();
  }

  public ILanguageDictionary<string> GetDescription() {
    throw new NotImplementedException();
  }


  #endregion --- IToXml --------------------------------------------
}
