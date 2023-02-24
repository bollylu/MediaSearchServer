namespace MediaSearch.Storage;
public partial class TStorageMemoryMovies : AStorageMemory, IStorageMovie {

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TStorageMemoryMovies() : base() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TStorageMemoryMovies>();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{nameof(PhysicalDataPath)} = {PhysicalDataPath.WithQuotes()}");
    RetVal.AppendLine($"{nameof(Medias)} = {Medias.Count} item(s))");
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public IAsyncEnumerable<IMovie> GetAllMoviesAsync() {
    try {
      _lock.EnterReadLock();
      return Medias.OfType<IMovie>().ToAsyncEnumerable();
    } finally {
      _lock.ExitReadLock();
    }
  }

  public Task<IMovie?> GetMovieAsync(string movieId) {
    try {
      _lock.EnterReadLock();
      return Task.FromResult(Medias.OfType<IMovie>().FirstOrDefault(x => x.Id == movieId));
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

  public ValueTask<int> MoviesCount() {
    try {
      _lock.EnterReadLock();
      return ValueTask.FromResult(Medias.OfType<IMovie>().Count());
    } catch (Exception ex) {
      LogErrorBox("Unable to count movies", ex);
      return ValueTask.FromResult(-1);
    } finally {
      _lock.ExitReadLock();
    }
  }

  public ValueTask<int> MoviesCount(IFilter filter) {
    try {
      _lock.EnterReadLock();
      return ValueTask.FromResult(Medias.OfType<IMovie>().WithFilter(filter).Count());
    } catch (Exception ex) {
      LogErrorBox("Unable to count movies", ex);
      return ValueTask.FromResult(-1);
    } finally {
      _lock.ExitReadLock();
    }
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

  public async ValueTask<bool> RemoveMovieAsync(IMovie movie) {
    return await RemoveMovieAsync(movie.Id);
  }

  public ValueTask<bool> UpdateMovieAsync(IMovie movie) {
    try {
      _lock.EnterWriteLock();
      int Index = Medias.FindIndex(x => x.Id == movie.Id);
      if (Index == -1) {
        LogWarningBox("Unable to locate movie for update", movie);
        return ValueTask.FromResult(false);
      }
      Medias[Index] = movie;
      return ValueTask.FromResult(true);
    } finally {
      _lock.ExitWriteLock();
    }
  }


  public ValueTask<bool> RemoveMovieAsync(string movieId) {
    try {
      _lock.EnterWriteLock();
      int Index = Medias.FindIndex(x => x.Id == movieId);
      if (Index == -1) {
        LogWarningBox("Unable to locate movie for removal", movieId);
        return ValueTask.FromResult(false);
      }
      Medias.RemoveAt(Index);
      return ValueTask.FromResult(true);
    } finally {
      _lock.ExitWriteLock();
    }
  }

  public ValueTask<bool> RemoveAllMoviesAsync() {
    throw new NotImplementedException();
  }
}
