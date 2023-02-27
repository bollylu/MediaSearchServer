namespace MediaSearch.Storage;
public abstract class AStorageMemory : AStorage {

  protected readonly List<IMedia> Medias = new List<IMedia>();

  protected readonly Dictionary<IIdString, Dictionary<string, byte[]>> MediaPictures = new Dictionary<IIdString, Dictionary<string, byte[]>>();

  protected readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

  #region --- IStorage --------------------------------------------
  public override ValueTask<bool> Exists() {
    return ValueTask.FromResult(true);
  }

  public override ValueTask<bool> Create() {
    Medias.Clear();
    return ValueTask.FromResult(true);
  }

  public override ValueTask<bool> Remove() {
    Medias.Clear();
    return ValueTask.FromResult(true);
  }
  #endregion --- IStorage --------------------------------------------

  protected bool AddPicture(IIdString mediaId, string pictureName, byte[] pictureContent) {
    const string ERROR_MSG_UNABLE_TO_ADD_PICTURE = "Unable to add picture";

    try {
      _lock.EnterUpgradeableReadLock();
      if (!Medias.Any(x => x.Id == mediaId.Id)) {
        LogError($"{ERROR_MSG_UNABLE_TO_ADD_PICTURE} : Picture {pictureName.WithQuotes()} cannot be associated with unknown mediaId {mediaId.Id.WithQuotes()}");
        return false;
      }
      if (!MediaPictures.ContainsKey(mediaId.Id)) {
        MediaPictures.Add(mediaId.Id, new Dictionary<string, byte[]>());
      }
      try {
        _lock.EnterWriteLock();
        if (MediaPictures[mediaId.Id].ContainsKey(pictureName)) {
          LogError($"{ERROR_MSG_UNABLE_TO_ADD_PICTURE} : Picture {pictureName.WithQuotes()} already exists");
          return false;
        }
        MediaPictures[mediaId.Id].Add(pictureName, pictureContent);
        return true;
      } catch (Exception ex) {
        LogErrorBox($"{ERROR_MSG_UNABLE_TO_ADD_PICTURE} {pictureName}", ex);
        return false;
      } finally {
        _lock.ExitWriteLock();
      }
    } finally {
      _lock.ExitUpgradeableReadLock();
    }
  }

  protected bool RemovePicture(IIdString mediaId, string pictureName) {
    try {
      _lock.EnterUpgradeableReadLock();
      if (MediaPictures.ContainsKey(mediaId.Id)) {
        try {
          _lock.EnterWriteLock();
          MediaPictures[mediaId.Id].Remove(pictureName);
          return true;
        } catch (Exception ex) {
          LogErrorBox($"Unable to remove picture {pictureName}", ex);
          return false;
        } finally {
          _lock.ExitWriteLock();
        }
      }
      return false;
    } finally {
      _lock.ExitUpgradeableReadLock();
    }
  }

  protected byte[]? GetPicture(IIdString mediaId, string pictureName) {
    try {
      _lock.EnterReadLock();
      return MediaPictures[mediaId.Id][pictureName];
    } catch (Exception ex) {
      LogErrorBox($"Unable to retrieve picture {pictureName}", ex);
      return null;
    } finally {
      _lock.ExitReadLock();
    }
  }

  protected IDictionary<string, byte[]> GetAllPictures(IIdString mediaId) {
    try {
      _lock.EnterReadLock();
      return MediaPictures[mediaId.Id];
    } catch (Exception ex) {
      LogErrorBox($"Unable to retrieve pictures for {mediaId}", ex);
      return new Dictionary<string, byte[]>();
    } finally {
      _lock.ExitReadLock();
    }
  }

  protected int GetPicturesCount(IIdString mediaId) {
    try {
      _lock.EnterReadLock();
      return MediaPictures[mediaId.Id].Count;
    } catch (Exception ex) {
      LogErrorBox($"Unable to retrieve pictures for {mediaId}", ex);
      return 0;
    } finally {
      _lock.ExitReadLock();
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
}
