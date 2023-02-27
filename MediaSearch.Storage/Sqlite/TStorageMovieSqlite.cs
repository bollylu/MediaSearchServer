namespace MediaSearch.Storage;

public class TStorageMovieSqlite : AStorage, IStorageMovie {

  public string DbName { get; set; } = "Movie";

  public string DbLocation { get; set; } = ".\\";
  public string DbFullName => Path.Join(DbLocation, $"{DbName}.db");

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TStorageMovieSqlite(string location, string name) {
    DbLocation = location;
    DbName = name;
    Logger = GlobalSettings.LoggerPool.GetLogger<TStorageMovieSqlite>();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{nameof(DbFullName)} = {DbFullName.WithQuotes()}");
    return RetVal.ToString();
  }

  #endregion --- Converters -------------------------------------------------------------------------------------

  #region --- AStorage --------------------------------------------
  public override ValueTask<bool> Exists() {
    using (TMovieContext Context = new TMovieContext(DbFullName)) {
      return ValueTask.FromResult(Context.Database.CanConnect());
    }
  }

  public override ValueTask<bool> Create() {
    using (TMovieContext Context = new TMovieContext(DbFullName)) {
      return ValueTask.FromResult(Context.Database.EnsureCreated());
    }
  }

  public override ValueTask<bool> Remove() {
    using (TMovieContext Context = new TMovieContext(DbFullName)) {
      try {
        Context.Database.EnsureDeleted();
        return ValueTask.FromResult(true);
      } catch (Exception ex) {
        Logger.LogErrorBox("Unable to remove sqlite database", ex);
        return ValueTask.FromResult(false);
      }
    }
  }
  public override ValueTask<bool> Any() {
    throw new NotImplementedException();
  }

  public override async ValueTask<bool> IsEmpty() {
    return !await Any();
  }

  public override Task Clear() {
    throw new NotImplementedException();
  }
  #endregion --- AStorage --------------------------------------------

  #region --- Movies --------------------------------------------
  public async Task<IMovie?> GetMovieAsync(string movieId) {
    using (TMovieContext Context = new TMovieContext(DbFullName)) {
      return await Context.Movies.FirstAsync(x => x.Id == movieId, CancellationToken.None).ConfigureAwait(false);
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

  public ValueTask<int> MoviesCount() {
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
      Logger.LogErrorBox($"Unable to add movie record {movie.Id.WithQuotes()}", ex);
      return false;
    }

  }

  public ValueTask<bool> RemoveMovieAsync(IRecord movie) {
    throw new NotImplementedException();
  }

  public ValueTask<bool> RemoveAllMoviesAsync() {
    throw new NotImplementedException();
  }

  public ValueTask<bool> AddMovieAsync(IMovie movie) {
    throw new NotImplementedException();
  }

  public ValueTask<bool> UpdateMovieAsync(IRecord movie) {
    throw new NotImplementedException();
  }
  public Task<IMovie?> GetMovieAsync(IRecord movieId) {
    throw new NotImplementedException();
  }
  #endregion --- Movies --------------------------------------------

  public ValueTask<bool> AddMoviePictureAsync(IRecord movie, string pictureName, byte[] picture) {
    throw new NotImplementedException();
  }

  public ValueTask<bool> RemovePictureAsync(IRecord movie, string pictureName) {
    throw new NotImplementedException();
  }



  public Task<byte[]?> GetMoviePictureAsync(IRecord movie, string pictureName, int width, int height) {
    throw new NotImplementedException();
  }

  public Task<IDictionary<string, byte[]>> GetMoviePicturesAsync(IRecord movie) {
    throw new NotImplementedException();
  }

  public ValueTask<int> GetMoviePictureCountAsync(IRecord movie) {
    throw new NotImplementedException();
  }


  #region --- Groups --------------------------------------------
  public Task<IGroup?> GetGroupsAsync(IRecord id) {
    throw new NotImplementedException();
  }

  public IAsyncEnumerable<IGroup> GetGroupsListAsync(IRecord id) {
    throw new NotImplementedException();
  }
  #endregion --- Groups --------------------------------------------
}
