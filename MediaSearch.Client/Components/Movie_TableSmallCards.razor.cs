﻿using Microsoft.AspNetCore.Components;

namespace MediaSearch.Client.Components;

public partial class Movie_TableSmallCards : ComponentBase, ILoggable {

  #region --- ILoggable --------------------------------------------
  public ILogger? Logger { get; set; }

  public void SetLogger(ILogger logger) {
    Logger = ALogger.Create(logger);
  }
  #endregion --- ILoggable --------------------------------------------

  //[Inject]
  //public IMovieService MovieService { get; set; }

  [Parameter]
  public IMoviesPage MoviesPage {
    get {
      return _MoviesPage ??= new TMoviesPage();
    }
    set {
      _MoviesPage = value;
    }
  }
  private IMoviesPage? _MoviesPage;

}