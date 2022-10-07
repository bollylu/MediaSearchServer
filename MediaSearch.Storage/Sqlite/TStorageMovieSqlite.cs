namespace MediaSearch.Storage;

public class TStorageMovieSqlite : IStorage, IStorageMovie {

  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TStorageMovieSqlite>();

  public string DbName { get; set; } = "Movie";

  public string DbLocation { get; set; } = ".\\";
  public string DbFullName => Path.Join(DbLocation, $"{DbName}.db");

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TStorageMovieSqlite(string location, string name) {
    DbLocation = location;
    DbName = name;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------


  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{nameof(DbFullName)} = {DbFullName.WithQuotes()}");
    return RetVal.ToString();
  }

  public bool Exists() {
    using (TMovieContext Context = new TMovieContext(DbFullName)) {
      return Context.Database.CanConnect();
    }
  }

  public bool Create() {
    using (TMovieContext Context = new TMovieContext(DbFullName)) {
      return Context.Database.EnsureCreated();
    }
  }

  public bool Remove() {
    using (TMovieContext Context = new TMovieContext(DbFullName)) {
      try {
        Context.Database.EnsureDeleted();
        return true;
      } catch (Exception ex) {
        Logger.LogErrorBox("Unable to remove sqlite database", ex);
        return false;
      }
    }
  }

  public async Task<IMovie?> GetMovieAsync(string movieId) {
    using (TMovieContext Context = new TMovieContext(DbFullName)) {
      return await Context.Movies.FirstAsync(x => x.ID == movieId, CancellationToken.None).ConfigureAwait(false);
    }
  }

  public Task<byte[]?> GetMoviePictureAsync(string movieId, string pictureName, int width, int height) {
    throw new NotImplementedException();
  }

  public async IAsyncEnumerable<IMovie> GetAllMoviesAsync() {
    using (TMovieContext Context = new TMovieContext(DbFullName)) {
      await foreach (IMovie MovieItem in Context.Movies.AsAsyncEnumerable().ConfigureAwait(false)) {
        yield return MovieItem;
      }
    }
  }

  public Task<IMoviesPage?> GetMoviesPageAsync(IFilter filter) {
    throw new NotImplementedException();
  }

  public Task<IMoviesPage?> GetMoviesLastPageAsync(IFilter filter) {
    throw new NotImplementedException();
  }

  public ValueTask<int> MoviesCount(IFilter filter) {
    throw new NotImplementedException();
  }

  public ValueTask<int> PagesCount(IFilter filter) {
    throw new NotImplementedException();
  }

  public async ValueTask<bool> AddMovieAsync(TMovie movie) {
    try {
      using (TMovieContext Context = new TMovieContext(DbFullName)) {
        await Context.Movies.AddAsync(movie);
        await Context.SaveChangesAsync();
        return true;
      }
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to add movie record {movie.ID.WithQuotes()}", ex);
      return false;
    }

  }

  public ValueTask<bool> RemoveMovieAsync(IMovie movie) {
    throw new NotImplementedException();
  }
}
