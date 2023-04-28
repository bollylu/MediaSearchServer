namespace MediaSearch.Models;

public class TMediasPage : IMediasPage, IJson {

  #region --- Public properties ------------------------------------------------------------------------------
  public string Name { get; set; } = "";

  public List<IMedia> Medias { get; set; } = new();

  public int Page { get; set; } = 0;

  public int AvailablePages { get; set; } = 0;

  public int AvailableMedias { get; set; } = 0;

  public string Source { get; set; } = "";
  #endregion --- Public properties ---------------------------------------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediasPage() { }
  public TMediasPage(IMediasPage page) : this() {
    Name = page.Name;
    Page = page.Page;
    AvailablePages = page.AvailablePages;
    AvailableMedias = page.AvailableMedias;
    Source = page.Source;
    Medias.AddRange(page.Medias);
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{Name} / page {Page}/{AvailablePages} [{Medias.Count} of {AvailableMedias} medias]");
    return RetVal.ToString();
  }

  public string ToString(bool withDetails) {
    StringBuilder RetVal = new StringBuilder(ToString());
    if (withDetails) {
      foreach (IMedia MediaItem in Medias) {
        RetVal.AppendLine($"# {MediaItem}");
      }
    }
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  #region --- Static instance --------------------------------------------
  public static IMediasPage Empty => _Empty ??= new TMediasPage();
  private static IMediasPage? _Empty;
  #endregion --- Static instance --------------------------------------------
}
