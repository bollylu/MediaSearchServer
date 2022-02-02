namespace MediaSearch.Client.Services;

public class TImageCache : ALoggable, IDisposable {

  public static int MAX_ITEMS = 250;

  private List<TCachedItem> _CachedImages = new();
  private readonly SemaphoreSlim _LockData = new SemaphoreSlim(1, 1);

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TImageCache() {
    Logger = new TConsoleLogger();
    LogDebug("Creating cache");
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public byte[] GetImage(string key) {
    try {
      _LockData.Wait();
      LogDebug("cache: Looking for image");
      TCachedItem? FoundIt = _CachedImages.FirstOrDefault(c => c.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
      if (FoundIt is null) {
        return Array.Empty<byte>();
      }
      return FoundIt.Data;
    } finally {
      _LockData.Release();
    }
  }

  public void AddImage(string key, byte[] image) {
    try {
      _LockData.Wait();
      if (_CachedImages.FirstOrDefault(c => c.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)) is not null) {
        return;
      }

      LogDebug($"Cache count : {_CachedImages.Count}");
      if (_CachedImages.Count > MAX_ITEMS) {
        DateTime Latest = DateTime.MaxValue;
        int Index = 0;
        for (int i = 0; i < _CachedImages.Count; i++) {
          if (_CachedImages[i].AddedTime < Latest) {
            Latest = _CachedImages[i].AddedTime;
            Index = i;
          }
        }
        LogDebug($"Remove from cache [{_CachedImages[Index].Key}]");
        _CachedImages.RemoveAt(Index);
      }

      LogDebug($"Add to cache [{key}]");

      _CachedImages.Add(new TCachedItem() { Key = key, AddedTime = DateTime.Now, Data = image });
    } finally {
      _LockData.Release();
    }
  }

  public void Dispose() {

  }
}

internal record TCachedItem {
  internal string Key { get; init; } = "";
  internal DateTime AddedTime { get; init; }
  internal byte[] Data { get; init; } = Array.Empty<byte>();
}
