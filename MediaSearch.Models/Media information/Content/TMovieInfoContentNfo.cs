namespace MediaSearch.Models;

public class TMovieInfoContentNfo : IMediaInfoContent, 
                                    IToXml, 
                                    IMediaSearchLoggable<TMovieInfoContentNfo> {

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
  public const string XML_ELEMENT_OUTLINE = "outline";
  public const string XML_ELEMENT_CREATION_YEAR = "year"; 
  #endregion --- XML constants -------------------------------------------------------------------------------

  public IMediaSearchLogger<TMovieInfoContentNfo> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMovieInfoContentNfo>();

  public string Title { get; set; } = "";
  public string OriginalTitle { get; set; } = "";
  public string SortTitle { get; set; } = "";
  public string Description { get; set; } = "";
  public string Country { get; set; } = "";
  public int CreationYear { get; set; } = -1;

  public List<string> Genres { get; } = new();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMovieInfoContentNfo() { }
  public TMovieInfoContentNfo(IMovie movie) {

  } 
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    string IndentSpace = new string(' ', indent);
    RetVal.AppendLine($"{IndentSpace}{nameof(Title)} = {Title.WithQuotes()}");
    RetVal.AppendLine($"{IndentSpace}{nameof(Description)} = {Description.WithQuotes()}");
    RetVal.AppendLine($"{IndentSpace}{nameof(Country)} = {Country.WithQuotes()}");
    RetVal.AppendLine($"{IndentSpace}{nameof(CreationYear)} = {CreationYear}");
    RetVal.AppendLine($"{IndentSpace}{nameof(Genres)} ({Genres.Count})");
    foreach (string TagItem in Genres) {
      RetVal.AppendLine($"{IndentSpace}  {TagItem.WithQuotes()}");
    }
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }

  public IMovie GetMovie() {
    IMovie RetVal = new TMovie() {
      Description = Description,
      Name = Title,
      CreationYear = CreationYear,
    };
    RetVal.Tags.AddRange(Genres);
    return RetVal;
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  #region --- IToXml --------------------------------------------

  public XElement ToXml() {
    XElement RetVal = new XElement(XML_THIS_ELEMENT);
    RetVal.SetElementValue(XML_ELEMENT_ORIGINAL_TITLE, Title);
    RetVal.SetElementValue(XML_ELEMENT_TITLE, Title);
    RetVal.SetElementValue(XML_ELEMENT_COUNTRY, Country);
    RetVal.SetElementValue(XML_ELEMENT_CREATION_YEAR, CreationYear);
    RetVal.SetElementValue(XML_ELEMENT_DESCRIPTION, Description);
    foreach (string GenreItem in Genres) {
      RetVal.Add(new XElement(XML_ELEMENT_GENRE, GenreItem));
    }
    return RetVal;
  }

  public void FromXml(XElement source) {
    if (source is null) {
      Logger.LogError("Unable to convert from Xml : source is null");
      return;
    }

    CreationYear = source.SafeReadElementValue(XML_ELEMENT_CREATION_YEAR, 0);

    Country = source.SafeReadElementValue(XML_ELEMENT_COUNTRY, "FR");
    Title = source.SafeReadElementValue(XML_ELEMENT_TITLE, "");
    Description = source.SafeReadElementValue(XML_ELEMENT_DESCRIPTION, "");

    Genres.Clear();
    foreach (XElement GenreItem in source.Elements(XML_ELEMENT_GENRE)) {
      string Genre = GenreItem.Value;
      if (Genre != "") {
        Genres.Add(Genre);
      }
    }
  }
  #endregion --- IToXml --------------------------------------------
}
