using BLTools.Diagnostic;

namespace MediaSearch.Models;
public class TMediaSources : ALoggable, IMediaSources {

  [DoNotDump]
  protected List<IMediaSource> MediaSources = new List<IMediaSource>();
  [DoNotDump]
  private readonly ReaderWriterLockSlim _Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSources() : base() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaSources>();
  }
  public TMediaSources(params IMediaSource[] mediaSources) : this() {
    foreach (IMediaSource MediaSourceItem in mediaSources) {
      MediaSources.Add(MediaSourceItem);
    }
  }
  public TMediaSources(IMediaSources mediaSources) : this() {
    foreach (IMediaSource MediaSourceItem in mediaSources.GetAll()) {
      MediaSources.Add(MediaSourceItem);
    }
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    foreach (var Item in MediaSources) {
      RetVal.AppendIndent($"- {Item}", indent);
    }
    return RetVal.ToString();
  }


  public override string ToString() {
    return ToString(0);
  }

  public string Dump() {
    StringBuilder RetVal = new StringBuilder();
    foreach (var Item in MediaSources) {
      RetVal.AppendLine(Item.Dump());
    }
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  #region --- IMediaSources --------------------------------------------
  public IMediaSource? GetDefault() {
    if (MediaSources.IsEmpty()) {
      return null;
    }

    return MediaSources.FirstOrDefault(s => s.Languages.GetPrincipal() == ELanguage.French);
  }

  public IMediaSource? Get(ELanguage language) {
    return MediaSources.FirstOrDefault(m => m.Languages.Contains(language));
  }

  public IEnumerable<IMediaSource> GetAll() {
    try {
      _Lock.EnterReadLock();
      return MediaSources.AsEnumerable();
    } finally {
      _Lock.ExitReadLock();
    }
  }

  public bool Add(params IMediaSource[] mediaSource) {
    try {
      _Lock.EnterWriteLock();
      MediaSources.AddRange(mediaSource);
      return true;
    } catch (Exception ex) {
      LogErrorBox("Unable to add MediaSource", ex);
      return false;
    } finally {
      _Lock.ExitWriteLock();
    }
  }

  public bool AddRange(IEnumerable<IMediaSource> mediaSources) {
    try {
      _Lock.EnterWriteLock();
      MediaSources.AddRange(mediaSources);
      return true;
    } catch (Exception ex) {
      LogErrorBox("Unable to add MediaSources", ex);
      return false;
    } finally {
      _Lock.ExitWriteLock();
    }
  }

  public bool Remove(IMediaSource mediaSource) {
    try {
      MediaSources.Remove(mediaSource);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox("Unable to remove media source", ex);
      return false;
    }
  }

  public void Clear() {
    try {
      _Lock.EnterWriteLock();
      MediaSources.Clear();
    } finally {
      _Lock.ExitWriteLock();
    }
  }

  public void SetDefault(IMediaSource mediaSource) {
    throw new NotImplementedException();
  }

  public bool Any() {
    try {
      _Lock.EnterReadLock();
      return MediaSources.Any();
    } finally {
      _Lock.ExitReadLock();
    }
  }

  public bool Any(Predicate<IMediaSource> predicate) {
    try {
      _Lock.EnterReadLock();
      return MediaSources.Any(x => predicate(x));
    } finally {
      _Lock.ExitReadLock();
    }
  }

  public bool IsEmpty() => !Any();
  #endregion --- IMediaSources --------------------------------------------
}
