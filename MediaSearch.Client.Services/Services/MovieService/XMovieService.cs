namespace MediaSearch.Client.Services;

public class XMovieService : IMovieService {

  public string ApiBase { get; } = "";
  public IApiServer ApiServer { get; set; } = new TApiServer();

  public Task<IMoviesPage?> GetMoviesPage(IFilter filter) {
    IMoviesPage? RetVal = new TMoviesPage();
    RetVal.Movies.Add(new TMovie("Le seigneur des anneaux") { Group = "Fantasy", StoragePath = "Le seigneur des anneaux 1.mkv", Size = 8_000_000 });
    RetVal.Movies.Add(new TMovie("Le seigneur des anneaux 2") { Group = "Fantasy", StoragePath = "Le seigneur des anneaux 2.mvk", Size = 8_001_000 });
    RetVal.Movies.Add(new TMovie("Le seigneur des anneaux 3") { Group = "Fantasy", StoragePath = "Le seigneur des anneaux 3.mvk", Size = 8_002_000 });
    return Task.FromResult<IMoviesPage?>(RetVal);
  }

  public Task<byte[]> GetPicture(string pathname, CancellationToken cancelToken, int w, int h) {
    throw new NotImplementedException();
  }

  public Task<string> GetPicture64(IMovie movie, CancellationToken cancelToken) {
    throw new NotImplementedException();
  }

  public Task<int> GetRefreshStatus() {
    return Task.FromResult(-1);
  }

  public Task<bool> ProbeApi() {
    return Task.FromResult(true);
  }

  public Task StartRefresh() {
    return Task.CompletedTask;
  }

  public Task<IList<string>> GetGroups(CancellationToken cancelToken) {
    throw new NotImplementedException();
  }

  public Task<IList<string>> GetSubGroups(string group, CancellationToken cancelToken) {
    throw new NotImplementedException();
  }

}
