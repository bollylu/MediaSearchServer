namespace MediaSearch.Client.Services;

public class TImageCache : IDisposable, IMediaSearchLoggable<TImageCache> {

  public static int MAX_ITEMS = 250;

  private readonly List<TCachedItem> _CachedImages = new();
  private readonly ReaderWriterLockSlim _LockData = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

  public IMediaSearchLogger<TImageCache> Logger { get; } = GlobalSettings.LoggerPool.GetLogger <TImageCache>();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TImageCache() {
    Logger.LogDebug("Creating cache");
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public byte[] GetImage(string key) {
    try {
      _LockData.EnterReadLock();
      Logger.LogDebug("cache: Looking for image");
      TCachedItem? FoundIt = _CachedImages.FirstOrDefault(c => c.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
      if (FoundIt is null) {
        return Array.Empty<byte>();
      }
      return FoundIt.Data;
    } finally {
      _LockData.ExitReadLock();
    }
  }

  public void AddImage(string key, byte[] image) {
    try {
      _LockData.EnterWriteLock();
      if (_CachedImages.FirstOrDefault(c => c.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)) is not null) {
        return;
      }

      // If cache count already at maximum, remove the oldest image prior to add the new one
      Logger.LogDebugEx($"Cache count : {_CachedImages.Count}");
      if (_CachedImages.Count > MAX_ITEMS) {
        DateTime Latest = DateTime.MaxValue;
        int Index = 0;
        for (int i = 0; i < _CachedImages.Count; i++) {
          if (_CachedImages[i].AddedTime < Latest) {
            Latest = _CachedImages[i].AddedTime;
            Index = i;
          }
        }
        Logger.LogDebug($"Remove from cache [{_CachedImages[Index].Key}]");
        _CachedImages.RemoveAt(Index);
      }

      Logger.LogDebugEx($"Add to cache [{key}]");

      _CachedImages.Add(new TCachedItem() { Key = key, AddedTime = DateTime.Now, Data = image });
    } finally {
      _LockData.ExitWriteLock();
    }
  }

  public void Dispose() {
    _LockData.EnterWriteLock();
    _CachedImages.Clear();
    _LockData.ExitWriteLock();
  }
}

internal record TCachedItem {
  internal string Key { get; init; } = "";
  internal DateTime AddedTime { get; init; }
  internal byte[] Data { get; init; } = Array.Empty<byte>();
}
