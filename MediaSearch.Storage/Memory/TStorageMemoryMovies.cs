namespace MediaSearch.Storage;
public class TStorageMemoryMovies : AStorageMemory, IStorageMovie {

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TStorageMemoryMovies() : base() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TStorageMemoryMovies>();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public async Task<IMovie?> GetMovieAsync(string movieId) {
    await Task.Yield();
    try {
      _lock.EnterReadLock();
      return Medias.OfType<IMovie>().FirstOrDefault(x => x.Id == movieId);
    } finally {
      _lock.ExitReadLock();
    }
  }

  public Task<byte[]?> GetMoviePictureAsync(string movieId, string pictureName, int width, int height) {
    throw new NotImplementedException();
  }

  public async IAsyncEnumerable<IMovie> GetAllMoviesAsync() {
    await Task.Yield();
    try {
      _lock.EnterReadLock();
      foreach (IMovie MovieItem in Medias.OfType<IMovie>()) {
        yield return MovieItem;
      }
    } finally {
      _lock.ExitReadLock();
    }
  }

  public async Task<IMoviesPage?> GetMoviesPageAsync(IFilter filter) {
    await Task.Yield();
    LogDebugBox("Filter", filter);

    TMoviesPage RetVal = new TMoviesPage() {
      Source = PhysicalDataPath,
      Page = filter.Page
    };

    try {
      _lock.EnterReadLock();
      IList<IMovie> FilteredMovies = Medias.OfType<IMovie>().WithFilter(filter).OrderedBy(filter).ToList();
      RetVal.AvailableMovies = FilteredMovies.Count;
      RetVal.AvailablePages = (RetVal.AvailableMovies / filter.PageSize) + (RetVal.AvailableMovies % filter.PageSize > 0 ? 1 : 0);
      RetVal.Movies.AddRange(FilteredMovies.Skip(filter.PageSize * (filter.Page - 1)).Take(filter.PageSize));
      return RetVal;
    } catch (Exception ex) {
      LogErrorBox("Unable to build a movie page", ex);
      return null;
    } finally {
      _lock.ExitReadLock();
    }
  }

  public Task<IMoviesPage?> GetMoviesLastPageAsync(IFilter filter) {
    throw new NotImplementedException();
  }

  public ValueTask<int> MoviesCount(IFilter filter) {
    return ValueTask.FromResult(Medias.OfType<IMovie>().WithFilter(filter).Count());
  }

  public async ValueTask<int> PagesCount(IFilter filter) {
    int FilteredMoviesCount = await MoviesCount(filter).ConfigureAwait(false);
    return (FilteredMoviesCount / filter.PageSize) + (FilteredMoviesCount % filter.PageSize > 0 ? 1 : 0);
  }

  public ValueTask<bool> AddMovieAsync(IMovie movie) {
    try {
      _lock.EnterWriteLock();
      Medias.Add(movie);
      return ValueTask.FromResult(true);
    } finally {
      _lock.ExitWriteLock();
    }
  }

  public ValueTask<bool> RemoveMovieAsync(IMovie movie) {
    try {
      _lock.EnterWriteLock();
      Medias.Remove(movie);
      return ValueTask.FromResult(true);
    } finally {
      _lock.ExitWriteLock();
    }
  }
}
