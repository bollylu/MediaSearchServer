namespace MediaSearch.Models;

public class TMoviesPage : IMoviesPage, IJson<TMoviesPage> {

  #region --- Public properties ------------------------------------------------------------------------------
  public string Name { get; set; } = "";

  public List<IMovie> Movies { get; set; } = new();

  public int Page { get; set; } = 0;

  public int AvailablePages { get; set; } = 0;

  public int AvailableMovies { get; set; } = 0;

  public string Source { get; set; } = "";
  #endregion --- Public properties ---------------------------------------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMoviesPage() {}
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
  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{Name} / page {Page}/{AvailablePages} [{Movies.Count} of {AvailableMovies} movies]");
    return RetVal.ToString();
  }

  public string ToString(bool withDetails) {
    StringBuilder RetVal = new StringBuilder(ToString());
    if (withDetails) {
      foreach (IMovie MovieItem in Movies) {
        RetVal.AppendLine($"# {MovieItem}");
      }
    }
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  #region --- Static instance --------------------------------------------
  public static IMoviesPage Empty => _Empty ??= new TMoviesPage();
  private static IMoviesPage? _Empty;
  #endregion --- Static instance --------------------------------------------
}
