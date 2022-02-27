using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Components;

public partial class Movie_TableSimple : ComponentBase, IMediaSearchLoggable<Movie_TableSimple> {

  #region --- ILoggable --------------------------------------------
  public IMediaSearchLogger<Movie_TableSimple> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<Movie_TableSimple>();
  #endregion --- ILoggable --------------------------------------------

  [Parameter]
  public IMoviesPage? MoviesPage { get; set; }
}
