namespace MediaSearch.Models;
public class TMediaSerieSeason :
  AMedia,
  IMediaSerieSeason {

  internal const int INVALID_NUMBER = -1;

  public ESerieType SerieType { get; set; } = ESerieType.Unknown;
  public int Number { get; set; } = INVALID_NUMBER;

  private readonly List<IMediaSerieEpisode> Episodes = new();
  private readonly ReaderWriterLockSlim _Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSerieSeason() : base() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaSerieSeason>();
  }

  public TMediaSerieSeason(ILogger logger) : base() {
    Logger = logger;
  }

  public TMediaSerieSeason(ESerieType type, int seasonNumber) : this() {
    SerieType = type;
    Number = seasonNumber;
  }
  public TMediaSerieSeason(IMediaSerieSeason season) : this() {
    SerieType = season.SerieType;
    Number = season.Number;
    Episodes.AddRange(season.GetEpisodes());
    MediaInfos.AddRange(season.MediaInfos.GetAll());
    MediaPictures.AddRange(season.MediaPictures.GetAll());
    MediaSources.AddRange(season.MediaSources.GetAll());
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendIndent($"- {nameof(SerieType)} = {SerieType}", indent);
    RetVal.AppendIndent($"- {nameof(Number)} = {Number}", indent);

    if (MediaInfos.Any()) {
      RetVal.AppendIndent($"- {nameof(MediaInfos)}", indent);
      foreach (var MediaInfoItem in MediaInfos.GetAll()) {
        RetVal.AppendIndent($"- {MediaInfoItem}", indent + 2);
      }
    } else {
      RetVal.AppendIndent($"- {nameof(MediaInfos)} is empty", indent);
    }

    if (Episodes.Any()) {
      RetVal.AppendIndent($"- {nameof(Episodes)} ({Episodes.Count})", indent);
      foreach (TMediaSerieEpisode EpisodeItem in Episodes) {
        RetVal.AppendIndent($"### {EpisodeItem.Season}x{EpisodeItem.Number:00} {EpisodeItem.Name} ###", indent + 2);
        RetVal.AppendIndent($"{EpisodeItem.ToString(indent)}", indent + 2);
      }
    } else {
      RetVal.AppendIndent($"- No episodes available", indent);
    }
    return RetVal.ToString();
  }


  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public bool AddEpisode(IMediaSerieEpisode episode) {
    if (episode.Number < 0) {
      Logger.LogError($"Unable to add episode #{episode.Number} : Episode number {episode.Number} is invalid");
      return false;
    }

    if (episode.SerieType != SerieType) {
      Logger.LogError($"Unable to add season #{episode.Number} : SerieType does not match");
      return false;
    }

    try {
      _Lock.EnterUpgradeableReadLock();
      if (Episodes.Any(s => s.Number == episode.Number)) {
        Logger.LogError($"Unable to add episode #{episode.Number} : episode already exists");
        return false;
      }
      try {
        _Lock.EnterWriteLock();
        Episodes.Add(episode);
        return true;

      } catch (Exception ex) {
        Logger.LogErrorBox("Unable to add episode", ex);
        return false;
      } finally {
        _Lock.ExitWriteLock();
      }
    } finally {
      _Lock.ExitUpgradeableReadLock();
    }
  }

  public bool RemoveEpisode(IMediaSerieEpisode episode) {
    if (episode.Number < 0) {
      Logger.LogError($"Unable to remove episode #{episode.Number} : Episode number {episode.Number} is invalid");
      return false;
    }

    try {
      _Lock.EnterWriteLock();
      int Index = Episodes.FindIndex(s => s.Number == episode.Number);
      if (Index >= 0) {
        Episodes.RemoveAt(Index);
        return true;
      }
      Logger.LogError($"Unable to remove episode @{episode.Number} : Episode is missing");
      return false;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to remove season #{episode.Number}", ex);
      return false;
    } finally {
      _Lock.ExitWriteLock();
    }
  }

  public bool RemoveEpisode(int episodeNumber) {
    if (episodeNumber < 0) {
      Logger.LogError($"Unable to remove episode #{episodeNumber} : Episode number {episodeNumber} is invalid");
      return false;
    }

    try {
      _Lock.EnterWriteLock();
      int Index = Episodes.FindIndex(s => s.Number == episodeNumber);
      if (Index >= 0) {
        Episodes.RemoveAt(Index);
        return true;
      }
      Logger.LogError($"Unable to remove episode #{episodeNumber} : Season is missing");
      return false;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to remove episode #{episodeNumber}", ex);
      return false;
    } finally {
      _Lock.ExitWriteLock();
    }
  }

  public IEnumerable<IMediaSerieEpisode> GetEpisodes() {
    try {
      _Lock.EnterReadLock();
      foreach (IMediaSerieEpisode EpisodeItem in Episodes) {
        yield return EpisodeItem;
      }
    } finally {
      _Lock.ExitReadLock();
    }
  }

  public IMediaSerieEpisode? GetEpisode(int index) {
    if (index < 0) {
      Logger.LogError($"Unable to retrieve episode #{index} : Episode number {index} is invalid");
      return null;
    }

    try {
      _Lock.EnterReadLock();
      IMediaSerieEpisode? RetVal = Episodes.FirstOrDefault(s => s.Number == index);
      if (RetVal is null) {
        Logger.LogError($"Unable to retrieve episode #{index} : Episode is missing");
        return null;
      }
      return RetVal;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to retrieve episode #{index}", ex);
      return null;
    } finally {
      _Lock.ExitReadLock();
    }

  }
  public void Clear() {
    try {
      _Lock.EnterWriteLock();
      Episodes.Clear();
    } finally {
      _Lock.ExitWriteLock();
    }
  }
}
