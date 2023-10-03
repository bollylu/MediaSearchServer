using System.Runtime.CompilerServices;

namespace MediaSearch.Models;
public class TMediaInfos : ALoggable, IMediaInfos {

  protected List<IMediaInfo> MediaInfos = new List<IMediaInfo>();
  private readonly ReaderWriterLockSlim _Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

  private readonly IMediaInfo? _Default;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaInfos() { }

  public TMediaInfos(params IMediaInfo[] mediaInfos) {
    foreach (IMediaInfo MediaInfoItem in mediaInfos) {
      MediaInfos.Add(MediaInfoItem);
    }
  }
  public TMediaInfos(IEnumerable<IMediaInfo> mediaInfos) {
    foreach (var MediaInfoItem in mediaInfos) {
      MediaInfos.Add(MediaInfoItem);
    }
  }

  public TMediaInfos(IMediaInfos mediaInfos) {
    foreach (var MediaInfoItem in mediaInfos.GetAll()) {
      MediaInfos.Add(MediaInfoItem);
    }
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    foreach (var Item in MediaInfos) {
      RetVal.AppendIndent($"- {Item}", indent);
    }
    return RetVal.ToString();
  }


  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public IMediaInfo? Get(ELanguage language) {
    try {
      _Lock.EnterReadLock();
      return MediaInfos.FirstOrDefault(i => i.Language == language);
    } finally {
      _Lock.ExitReadLock();
    }
  }

  public void SetTitle(ELanguage language, string title) {
    try {
      _Lock.EnterUpgradeableReadLock();
      int Index = MediaInfos.FindIndex(i => i.Language == language);
      if (Index < 0) {
        return;
      }
      try {
        _Lock.EnterWriteLock();
        MediaInfos[Index].Title = title;
      } finally {
        _Lock.ExitWriteLock();
      }
    } catch (Exception ex) {
      LogErrorBox("Unable to set title", ex);
    } finally {
      _Lock.ExitUpgradeableReadLock();
    }
  }

  public IEnumerable<IMediaInfo> GetAll() {
    try {
      _Lock.EnterReadLock();
      return MediaInfos.AsEnumerable();
    } finally {
      _Lock.ExitReadLock();
    }
  }

  public IMediaInfo? GetDefault() {
    return _Default;
  }

  public bool Add(params IMediaInfo[] mediaInfo) {
    try {
      _Lock.EnterWriteLock();
      MediaInfos.AddRange(mediaInfo);
      return true;
    } catch (Exception ex) {
      LogErrorBox("Unable to add MediaInfo", ex);
      return false;
    } finally {
      _Lock.ExitWriteLock();
    }
  }

  public bool AddRange(IEnumerable<IMediaInfo> mediaInfos) {
    try {
      _Lock.EnterWriteLock();
      MediaInfos.AddRange(mediaInfos);
      return true;
    } catch (Exception ex) {
      LogErrorBox("Unable to add MediaInfos", ex);
      return false;
    } finally {
      _Lock.ExitWriteLock();
    }
  }

  public bool Remove(IMediaInfo mediaInfo) {
    throw new NotImplementedException();
  }

  public void Clear() {
    try {
      _Lock.EnterWriteLock();
      MediaInfos.Clear();
    } finally {
      _Lock.ExitWriteLock();
    }
  }

  public bool Any() {
    try {
      _Lock.EnterReadLock();
      return MediaInfos.Any();
    } finally {
      _Lock.ExitReadLock();
    }
  }

  public bool Any(Func<IMediaInfo, bool> predicate) {
    try {
      _Lock.EnterReadLock();
      return MediaInfos.Any(predicate);
    } finally {
      _Lock.ExitReadLock();
    }
  }

  public bool IsEmpty() => !Any();
}
