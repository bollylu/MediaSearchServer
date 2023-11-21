namespace MediaSearch.Storage;
public class TStorageMemoryMedias : AStorageMemory, IStorageMedias {

  protected readonly List<IMedia> Medias = new List<IMedia>();

  protected readonly Dictionary<IRecord, Dictionary<string, byte[]>> MediaPictures = new Dictionary<IRecord, Dictionary<string, byte[]>>();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TStorageMemoryMedias() : base() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TStorageMemoryMedias>();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{nameof(Medias)} = {Medias.Count} item(s))");
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public override Task Clear() {
    try {
      _lock.EnterWriteLock();
      Medias.Clear();
      return Task.CompletedTask;
    } finally {
      _lock.EnterWriteLock();
    }

  }

  public override ValueTask<bool> Any() {
    try {
      _lock.EnterReadLock();
      return ValueTask.FromResult(Medias.Any());
    } finally {
      _lock.ExitReadLock();
    }
  }

  public override async ValueTask<bool> IsEmpty() {
    return !await Any();
  }

  public IAsyncEnumerable<IMedia> GetMediasAsync() {
    try {
      _lock.EnterReadLock();
      return Medias.ToAsyncEnumerable();
    } finally {
      _lock.ExitReadLock();
    }
  }

  public Task<IMedia?> GetMediaAsync(IRecord movieId) {
    try {
      _lock.EnterReadLock();
      return Task.FromResult(Medias.FirstOrDefault(x => x.Id == movieId.Id));
    } finally {
      _lock.ExitReadLock();
    }
  }

  public async Task<IMediasPage?> GetMediasPageAsync(IFilter filter) {
    await Task.Yield();
    LogDebugBox("Filter", filter);

    IMediasPage RetVal = new TMediasPage() {
      //Source = PhysicalDataPath,
      Page = filter.Page
    };

    try {
      _lock.EnterReadLock();
      IList<IMedia> FilteredMedias = Medias.WithFilter(filter).OrderedBy(filter).ToList();
      RetVal.AvailableMedias = FilteredMedias.Count;
      RetVal.AvailablePages = (RetVal.AvailableMedias / filter.PageSize) + (RetVal.AvailableMedias % filter.PageSize > 0 ? 1 : 0);
      RetVal.Medias.AddRange(FilteredMedias.Skip(filter.PageSize * (filter.Page - 1)).Take(filter.PageSize));
      return RetVal;
    } catch (Exception ex) {
      LogErrorBox("Unable to build a media page", ex);
      return null;
    } finally {
      _lock.ExitReadLock();
    }
  }

  public Task<IMediasPage?> GetMediasLastPageAsync(IFilter filter) {
    throw new NotImplementedException();
  }

  public ValueTask<int> MediasCount() {
    try {
      _lock.EnterReadLock();
      return ValueTask.FromResult(Medias.Count());
    } catch (Exception ex) {
      LogErrorBox("Unable to count medias", ex);
      return ValueTask.FromResult(-1);
    } finally {
      _lock.ExitReadLock();
    }
  }

  public ValueTask<int> MediasCount(IFilter filter) {
    try {
      _lock.EnterReadLock();
      return ValueTask.FromResult(Medias.WithFilter(filter).Count());
    } catch (Exception ex) {
      LogErrorBox("Unable to count medias", ex);
      return ValueTask.FromResult(-1);
    } finally {
      _lock.ExitReadLock();
    }
  }

  public async ValueTask<int> PagesCount(IFilter filter) {
    int FilteredMediasCount = await MediasCount(filter).ConfigureAwait(false);
    return (FilteredMediasCount / filter.PageSize) + (FilteredMediasCount % filter.PageSize > 0 ? 1 : 0);
  }

  public ValueTask<bool> AddMediaAsync(IMedia media) {
    try {
      _lock.EnterWriteLock();
      Medias.Add(media);
      return ValueTask.FromResult(true);
    } finally {
      _lock.ExitWriteLock();
    }
  }

  public ValueTask<bool> UpdateMediaAsync(IMedia media) {
    try {
      _lock.EnterWriteLock();
      int Index = Medias.FindIndex(x => x.Id == media.Id);
      if (Index == -1) {
        LogWarningBox("Unable to locate media for update", media);
        return ValueTask.FromResult(false);
      }
      Medias[Index] = media;
      return ValueTask.FromResult(true);
    } finally {
      _lock.ExitWriteLock();
    }
  }


  public ValueTask<bool> RemoveMediaAsync(IRecord mediaId) {
    try {
      _lock.EnterWriteLock();
      int Index = Medias.FindIndex(x => x.Id == mediaId.Id);
      if (Index == -1) {
        LogWarningBox("Unable to locate media for removal", mediaId);
        return ValueTask.FromResult(false);
      }
      Medias.RemoveAt(Index);
      return ValueTask.FromResult(true);
    } finally {
      _lock.ExitWriteLock();
    }
  }

  public ValueTask<bool> RemoveAllMediasAsync() {
    throw new NotImplementedException();
  }

}
