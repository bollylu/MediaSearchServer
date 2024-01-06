using BLTools.Diagnostic;

namespace MediaSearch.Storage;
public abstract class AStorageMemory : ALoggable, IStorage {

  public string Name { get; set; } = string.Empty;

  [DoNotDump]
  protected readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

  #region --- IStorage --------------------------------------------
  public ValueTask<bool> Exists() {
    return ValueTask.FromResult(true);
  }

  public async ValueTask<bool> Create() {
    await Clear();
    return true;
  }

  public async ValueTask<bool> Remove() {
    await Clear();
    return true;
  }

  public abstract Task Clear();
  public abstract ValueTask<bool> Any();
  public abstract ValueTask<bool> IsEmpty();

  #endregion --- IStorage --------------------------------------------

  protected AStorageMemory() { }
  protected AStorageMemory(ILogger logger) {
    Logger = logger;
  }

  //protected bool AddPicture(IRecord mediaId, string pictureName, byte[] pictureContent) {
  //  const string ERROR_MSG_UNABLE_TO_ADD_PICTURE = "Unable to add picture";

  //  try {
  //    _lock.EnterUpgradeableReadLock();
  //    if (!Medias.Any(x => x.Id == mediaId.Id)) {
  //      LogError($"{ERROR_MSG_UNABLE_TO_ADD_PICTURE} : Picture {pictureName.WithQuotes()} cannot be associated with unknown mediaId {mediaId.Id.WithQuotes()}");
  //      return false;
  //    }
  //    if (!MediaPictures.ContainsKey(mediaId)) {
  //      MediaPictures.Add(mediaId, new Dictionary<string, byte[]>());
  //    }
  //    try {
  //      _lock.EnterWriteLock();
  //      if (MediaPictures[mediaId].ContainsKey(pictureName)) {
  //        LogError($"{ERROR_MSG_UNABLE_TO_ADD_PICTURE} : Picture {pictureName.WithQuotes()} already exists");
  //        return false;
  //      }
  //      MediaPictures[mediaId].Add(pictureName, pictureContent);
  //      return true;
  //    } catch (Exception ex) {
  //      LogErrorBox($"{ERROR_MSG_UNABLE_TO_ADD_PICTURE} {pictureName}", ex);
  //      return false;
  //    } finally {
  //      _lock.ExitWriteLock();
  //    }
  //  } finally {
  //    _lock.ExitUpgradeableReadLock();
  //  }
  //}

  //protected bool RemovePicture(IRecord mediaId, string pictureName) {
  //  try {
  //    _lock.EnterUpgradeableReadLock();
  //    if (MediaPictures.ContainsKey(mediaId)) {
  //      try {
  //        _lock.EnterWriteLock();
  //        MediaPictures[mediaId].Remove(pictureName);
  //        return true;
  //      } catch (Exception ex) {
  //        LogErrorBox($"Unable to remove picture {pictureName}", ex);
  //        return false;
  //      } finally {
  //        _lock.ExitWriteLock();
  //      }
  //    }
  //    return false;
  //  } finally {
  //    _lock.ExitUpgradeableReadLock();
  //  }
  //}

  //protected byte[]? GetPicture(IRecord mediaId, string pictureName) {
  //  try {
  //    _lock.EnterReadLock();
  //    return MediaPictures[mediaId][pictureName];
  //  } catch (Exception ex) {
  //    LogErrorBox($"Unable to retrieve picture {pictureName}", ex);
  //    return null;
  //  } finally {
  //    _lock.ExitReadLock();
  //  }
  //}

  //protected IDictionary<string, byte[]> GetAllPictures(IRecord mediaId) {
  //  try {
  //    _lock.EnterReadLock();
  //    return MediaPictures[mediaId];
  //  } catch (Exception ex) {
  //    LogErrorBox($"Unable to retrieve pictures for {mediaId}", ex);
  //    return new Dictionary<string, byte[]>();
  //  } finally {
  //    _lock.ExitReadLock();
  //  }
  //}

  //protected int GetPicturesCount(IRecord mediaId) {
  //  try {
  //    _lock.EnterReadLock();
  //    return MediaPictures[mediaId].Count;
  //  } catch (Exception ex) {
  //    LogErrorBox($"Unable to retrieve pictures for {mediaId}", ex);
  //    return 0;
  //  } finally {
  //    _lock.ExitReadLock();
  //  }
  //}

  //public override ValueTask<bool> Any() {
  //  try {
  //    _lock.EnterReadLock();
  //    return ValueTask.FromResult(Medias.Any());
  //  } finally {
  //    _lock.ExitReadLock();
  //  }
  //}

  //public override async ValueTask<bool> IsEmpty() {
  //  return !await Any();
  //}
}
