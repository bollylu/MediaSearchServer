namespace MediaSearch.Models;

public class TMoviesPage : IMoviesPage, IJson<TMoviesPage> {

  public const int DEFAULT_PAGE_SIZE = 20;

  #region --- Public properties ------------------------------------------------------------------------------
  public string Name { get; set; } = "";

  public List<IMovie> Movies { get; set; } = new();

  public int Page { get; set; } = 0;

  public int AvailablePages { get; set; } = 0;

  public int AvailableMovies { get; set; } = 0;

  public string Source { get; set; } = "";
  #endregion --- Public properties ---------------------------------------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMoviesPage() { }
  public TMoviesPage(IMoviesPage page) : this() {
    if (page is null) {
      return;
    }
    Name = page.Name;
    Page = page.Page;
    AvailablePages = page.AvailablePages;
    AvailableMovies = page.AvailableMovies;
    Source = page.Source;
    Movies.AddRange(page.Movies);
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    string IndentSpace = new string(' ', indent);
    RetVal.AppendLine($"{IndentSpace}{nameof(Name)} : {Name.WithQuotes()}");
    RetVal.AppendLine($"{IndentSpace}{nameof(Source)} : {Source.WithQuotes()}");
    RetVal.AppendLine($"{IndentSpace}{nameof(Page)} : {Page}");
    RetVal.AppendLine($"{IndentSpace}{nameof(AvailablePages)} : {AvailablePages}");
    RetVal.AppendLine($"{IndentSpace}{nameof(AvailableMovies)} : {AvailableMovies}");
    RetVal.AppendLine($"{IndentSpace}{nameof(Movies)} ({Movies.Count})");
    if (Movies.Any()) {
      foreach (IMovie MovieItem in Movies) {
        RetVal.AppendLine($"{IndentSpace}---");
        RetVal.AppendLine($"{IndentSpace}{MovieItem.ToString(2)}");
        RetVal.AppendLine($"{IndentSpace}---");
      }
    } else {
      RetVal.AppendLine($"{IndentSpace} - Movies is empty");
    }
    return RetVal.ToString();
  }


  public override string ToString() {
    return ToString(0);
  }

  //public string ToString(bool withDetails, int indent = 0) {
  //  StringBuilder RetVal = new StringBuilder(ToString(indent));
  //  string IndentSpace = new string(' ', indent);
  //  if (withDetails) {
  //    foreach (IMovie MovieItem in Movies) {
  //      RetVal.AppendLine($"{IndentSpace}-{MovieItem.ToString(indent)}");
  //    }
  //  }
  //  return RetVal.ToString();
  //}
  #endregion --- Converters -------------------------------------------------------------------------------------

  #region --- Static instance --------------------------------------------
  public static IMoviesPage Empty => _Empty ??= new TMoviesPage();
  private static IMoviesPage? _Empty;
  #endregion --- Static instance --------------------------------------------
}
