using System.IO;

using BLTools.Diagnostic;

namespace MediaSearch.Models;
public class TMediaStreams : ALoggable, IMediaStreams {

  [DoNotDump]
  protected List<IMediaStream> MediaStreamItems = new();
  [DoNotDump]
  private readonly ReaderWriterLockSlim _Lock = new ReaderWriterLockSlim();

  [DoNotDump]
  public IEnumerable<TMediaStreamVideo> MediaStreamsVideo => MediaStreamItems.OfType<TMediaStreamVideo>();
  [DoNotDump]
  public IEnumerable<TMediaStreamAudio> MediaStreamsAudio => MediaStreamItems.OfType<TMediaStreamAudio>();
  [DoNotDump]
  public IEnumerable<TMediaStreamSubTitle> MediaStreamsSubTitle => MediaStreamItems.OfType<TMediaStreamSubTitle>();
  [DoNotDump]
  public IEnumerable<TMediaStreamUnknown> MediaStreamsUnknown => MediaStreamItems.OfType<TMediaStreamUnknown>();
  [DoNotDump]
  public IEnumerable<TMediaStreamData> MediaStreamsData => MediaStreamItems.OfType<TMediaStreamData>();

  public TMediaStreams() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaStreams>();
  }

  public bool Add(params IMediaStream[] mediaStreams) {
    try {
      _Lock.EnterWriteLock();
      MediaStreamItems.AddRange(mediaStreams);
      return true;
    } catch (Exception ex) {
      LogErrorBox($"Unable to add {mediaStreams.Count()} items", ex);
      return false;
    } finally {
      _Lock.ExitWriteLock();
    }
  }

  public bool AddRange(IEnumerable<IMediaStream> mediaStreams) {
    try {
      _Lock.EnterWriteLock();
      MediaStreamItems.AddRange(mediaStreams);
      return true;
    } catch (Exception ex) {
      LogErrorBox($"Unable to add {mediaStreams.Count()} items", ex);
      return false;
    } finally {
      _Lock.ExitWriteLock();
    }
  }

  public IMediaStream? Get(int streamId) {
    try {
      _Lock.EnterReadLock();
      return MediaStreamItems.FirstOrDefault(s => s.Index == streamId);
    } catch (Exception ex) {
      LogErrorBox($"Unable to get stream #{streamId}", ex);
      return null;
    } finally {
      _Lock.ExitReadLock();
    }
  }

  public IEnumerable<IMediaStream> GetAll() {
    try {
      _Lock.EnterReadLock();
      return MediaStreamItems.AsEnumerable();
    } catch (Exception ex) {
      LogErrorBox($"Unable to get streams", ex);
      return Enumerable.Empty<IMediaStream>();
    } finally {
      _Lock.ExitReadLock();
    }
  }

  public bool Remove(IMediaStream mediaStream) {
    throw new NotImplementedException();
  }

  public void Clear() {
    try {
      _Lock.EnterWriteLock();
      MediaStreamItems.Clear();
    } finally {
      _Lock.ExitWriteLock();
    }
  }

  public bool IsEmpty() {
    return !Any();
  }

  public bool Any() {
    try {
      _Lock.EnterReadLock();
      return MediaStreamItems.Any();
    } finally {
      _Lock.ExitReadLock();
    }
  }

  public bool Any(Predicate<IMediaStream> predicate) {
    try {
      _Lock.EnterReadLock();
      return MediaStreamItems.Any(x => predicate(x));
    } finally {
      _Lock.ExitReadLock();
    }
  }
}
