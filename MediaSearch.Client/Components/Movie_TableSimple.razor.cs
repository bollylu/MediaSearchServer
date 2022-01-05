using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Components;

public partial class Movie_TableSimple : ComponentBase, ILoggable {
  
  #region --- ILoggable --------------------------------------------
  public ILogger? Logger { get; set; }

  public void SetLogger(ILogger logger) {
    Logger = ALogger.Create(logger);
  } 
  #endregion --- ILoggable --------------------------------------------

  [Parameter]
  public IMoviesPage? MoviesPage { get; set; }
}
