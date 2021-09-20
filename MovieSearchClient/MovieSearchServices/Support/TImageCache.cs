using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieSearchClient.Services {
  public class TImageCache {

    public static int MAX_ITEMS = 50;

    private List<CachedItem> _CachedImages = new();
    private readonly SemaphoreSlim _LockData = new SemaphoreSlim(1, 1);

    public TImageCache() {
      Console.WriteLine("Creating cache");
    }

    public byte[] GetImage(string key) {
      try {
        _LockData.Wait();
        Console.WriteLine("cache: Looking for image");
        CachedItem FoundIt = _CachedImages.FirstOrDefault(c => c.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
        if (FoundIt == null) {
          return null;
        }
        return FoundIt.Data;
      } finally {
        _LockData.Release();
      }
    }

    public void AddImage(string key, byte[] image) {
      try {
        _LockData.Wait();
        if (_CachedImages.FirstOrDefault(c => c.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)) != null) {
          return;
        }

        Console.WriteLine($"Cache count : {_CachedImages.Count}");
        if (_CachedImages.Count > MAX_ITEMS) {
          DateTime Latest = DateTime.MaxValue;
          int Index = 0;
          for (int i = 0; i < _CachedImages.Count; i++) {
            if (_CachedImages[i].AddedTime < Latest) {
              Latest = _CachedImages[i].AddedTime;
              Index = i;
            }
          }
          Console.WriteLine($"Remove from cache [{_CachedImages[Index].Key}]");
          _CachedImages.RemoveAt(Index);
        }

        Console.WriteLine($"Add to cache [{key}]");

        _CachedImages.Add(new CachedItem() { Key = key, AddedTime = DateTime.Now, Data = image });
      } finally {
        _LockData.Release();
      }
    }

  }

  internal class CachedItem {
    internal string Key { get; init; }
    internal DateTime AddedTime { get; init; }
    internal byte[] Data { get; init; }
  }
}
