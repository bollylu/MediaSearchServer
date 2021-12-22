namespace MediaSearch.Client.Services;

public interface IMovieService {

  /// <summary>
  /// The base address for the api server connection
  /// </summary>
  string ApiBase { get; }

  /// <summary>
  /// Test the api server for existance
  /// </summary>
  /// <returns>true if the server is available, false otherwise</returns>
  Task<bool> ProbeApi();

  //string RootPath { get; }

  #region --- Movies --------------------------------------------
  //List<string> ExcludedExtensions { get; }
  Task<IMoviesPage> GetMovies(RFilter filter, int startPage = 1, int pageSize = 20);
  Task<byte[]> GetPicture(string pathname, int w = 128, int h = 160);
  Task<string> GetPicture64(IMovie movie);
  #endregion --- Movies --------------------------------------------

  Task StartRefresh();
  Task<int> GetRefreshStatus();
}
