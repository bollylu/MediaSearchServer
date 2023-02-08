namespace MediaSearch.Client.Services;

public class TImageCache : ALoggable, IDisposable {

  public static int MAX_ITEMS = 250;

  private readonly List<TCachedItem> _CachedImages = new();
  private readonly ReaderWriterLockSlim _LockData = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TImageCache() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TImageCache>();
    Logger.IfDebugMessageEx("Creating cache", "");
    Logger.SeverityLimit = ESeverity.Debug;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public byte[] GetImage(string key) {
    try {
      _LockData.EnterReadLock();
      Logger.IfDebugMessageEx("Looking for image", key);
      TCachedItem? FoundIt = _CachedImages.FirstOrDefault(c => c.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
      if (FoundIt is null) {
        Logger.IfDebugMessageEx("Image not found", key);
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
      Logger.IfDebugMessageEx($"Cache count", _CachedImages.Count);
      if (_CachedImages.Count > MAX_ITEMS) {
        DateTime Latest = DateTime.MaxValue;
        int Index = 0;
        for (int i = 0; i < _CachedImages.Count; i++) {
          if (_CachedImages[i].AddedTime < Latest) {
            Latest = _CachedImages[i].AddedTime;
            Index = i;
          }
        }
        Logger.IfDebugMessageEx("Remove from cache", _CachedImages[Index].Key);
        _CachedImages.RemoveAt(Index);
      }

      Logger.IfDebugMessageEx("Add to cache", key);

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
