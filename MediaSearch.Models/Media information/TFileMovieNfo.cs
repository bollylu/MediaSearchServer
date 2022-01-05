namespace MediaSearch.Models.Media_information;

public class TFileMovieNfo : ALoggable, IFileMediaInfo, IToXml {

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

  #region --- IFileMediaInfo --------------------------------------------
  public Dictionary<ELanguage, string> Titles { get; } = new();
  public Dictionary<ELanguage, string> Descriptions { get; } = new();
  public int Size { get; } = -1;
  public string Country { get; set; }

  #region --- IName --------------------------------------------
  public string Name { get; set; }
  public string Description { get; set; }
  #endregion --- IName --------------------------------------------

  public bool Export(string newFilename) {
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
      LogError("Unable to convert from Xml : source is null");
      return;
    }

    Country = source.SafeReadElementValue(XML_ELEMENT_COUNTRY, "FR");
    switch (Country) {
      
      case "FR": {
          Titles.Add(ELanguage.French, source.SafeReadElementValue(XML_ELEMENT_TITLE, ""));
          break;
        }

      default:
      case "US": {
          Titles.Add(ELanguage.English, source.SafeReadElementValue(XML_ELEMENT_TITLE, ""));
          break;
        }

    }




  }
  #endregion --- IToXml --------------------------------------------
}
