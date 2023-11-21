namespace MediaSearch.Client.Services;

public class XMovieService : IMovieService {

  public string ApiBase { get; } = "";
  public IApiServer ApiServer { get; set; } = new TApiServer();

  public Task<IMediasPage?> GetMoviesPage(IFilter filter) {
    IMediasPage RetVal = new TMediasPage();
    IMediaSource Source1 = new TMediaSourceVirtual() { StoragePath = "Le seigneur des anneaux 1.mvk", Size = 8_001_000 };
    IMediaSource Source2 = new TMediaSourceVirtual() { StoragePath = "Le seigneur des anneaux 2.mvk", Size = 8_002_000 };
    IMediaSource Source3 = new TMediaSourceVirtual() { StoragePath = "Le seigneur des anneaux 3.mvk", Size = 8_003_000 };

    TMovie Movie1 = new TMovie() { Name = "Le seigneur des anneaux", Group = "Fantasy" };
    Movie1.MediaSources.Add(Source1);
    RetVal.Movies.Add(Movie1);

    TMovie Movie2 = new TMovie() { Name = "Le seigneur des anneaux 2", Group = "Fantasy" };
    Movie2.MediaSources.Add(Source2);
    RetVal.Movies.Add(Movie2);

    TMovie Movie3 = new TMovie() { Name = "Le seigneur des anneaux 3", Group = "Fantasy" };
    Movie3.MediaSources.Add(Source3);
    RetVal.Movies.Add(Movie3);

    return Task.FromResult<IMediasPage?>(RetVal);
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
