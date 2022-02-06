namespace MediaSearch.Client.Services;

public interface IMovieService {

  /// <summary>
  /// The base address for the api server connection
  /// </summary>
  string ApiBase { get; }

  IApiServer ApiServer { get; set; }

  /// <summary>
  /// Test the api server for existance
  /// </summary>
  /// <returns>true if the server is available, false otherwise</returns>
  Task<bool> ProbeApi();

  //string RootPath { get; }

  #region --- Movies --------------------------------------------
  //List<string> ExcludedExtensions { get; }
  Task<IMoviesPage?> GetMoviesPage(IFilter filter);
  Task<byte[]> GetPicture(string pathname, CancellationToken cancelToken, int w = 128, int h = 160);
  Task<string> GetPicture64(IMovie movie, CancellationToken cancelToken);
  #endregion --- Movies --------------------------------------------

  Task StartRefresh();
  Task<int> GetRefreshStatus();

  Task<IList<string>> GetGroups(CancellationToken cancelToken);
  Task<IList<string>> GetSubGroups(string group, CancellationToken cancelToken);

  //Task<IMovieStats> GetStats(CancellationToken token);
}
