using System.Linq.Expressions;

namespace MediaSearch.Models;

/// <summary>
/// A media used to store a serie (TV show, anime, ...)
/// </summary>
public class TMediaSerie : AMedia {

  /// <summary>
  /// The type of serie
  /// </summary>
  public ESerieType SerieType { get; set; }

  private readonly List<IMediaSerieSeason> Seasons = new List<IMediaSerieSeason>();
  private readonly ReaderWriterLockSlim _Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSerie() : base() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaSerie>();
    MediaType = EMediaType.Serie;
  }
  public TMediaSerie(ILogger logger) : base(logger) {
    MediaType = EMediaType.Serie;
  }
  public TMediaSerie(TMediaSerie movie) : base(movie) {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaSerie>();
    MediaType = EMediaType.Serie;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder(base.ToString(indent));
    RetVal.AppendIndent($"- {nameof(SerieType)} = {SerieType}", indent);
    RetVal.AppendIndent($"- {nameof(Seasons)}", indent);
    foreach (IMediaSerieSeason SeasonItem in Seasons) {
      RetVal.AppendIndent(SeasonItem.ToString(indent), indent + 2);
    }
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  /// <summary>
  /// Add a non-existing season to the serie
  /// </summary>
  /// <param name="season">The season to add</param>
  /// <returns><see langword="true"/> if successful, <see langword="false"/> otherwise</returns>
  public bool AddSeason(IMediaSerieSeason season) {
    if (season.Number < 0) {
      LogError($"Unable to add season #{season.Number} : Season number {season.Number} is invalid");
      return false;
    }

    if (season.SerieType != SerieType) {
      LogError($"Unable to add season #{season.Number} : SerieType does not match");
      return false;
    }

    try {
      _Lock.EnterUpgradeableReadLock();
      if (Seasons.Any(s => s.Number == season.Number)) {
        Logger.LogError($"Unable to add season #{season.Number} : season already exists");
        return false;
      }
      try {
        _Lock.EnterWriteLock();
        Seasons.Add(season);
        return true;

      } catch (Exception ex) {
        Logger.LogErrorBox("Unable to add season", ex);
        return false;
      } finally {
        _Lock.ExitWriteLock();
      }
    } finally {
      _Lock.ExitUpgradeableReadLock();
    }
  }

  /// <summary>
  /// Remove a season from the serie
  /// </summary>
  /// <param name="season">The season to remove</param>
  /// <returns><see langword="true"/> if successful, <see langword="false"/> otherwise</returns>
  public bool RemoveSeason(IMediaSerieSeason season) {
    if (season.Number < 0) {
      LogError($"Unable to remove season #{season.Number} : Season number {season.Number} is invalid");
      return false;
    }

    try {
      _Lock.EnterWriteLock();
      int Index = Seasons.FindIndex(s => s.Number == season.Number);
      if (Index >= 0) {
        Seasons.RemoveAt(Index);
        return true;
      }
      Logger.LogError($"Unable to remove season @{season.Number} : Season is missing");
      return false;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to remove season #{season.Number}", ex);
      return false;
    } finally {
      _Lock.ExitWriteLock();
    }
  }

  /// <summary>
  /// Remove a season from the serie
  /// </summary>
  /// <param name="seasonNumber">The number of the season to remove</param>
  /// <returns><see langword="true"/> if successful, <see langword="false"/> otherwise</returns>
  public bool RemoveSeason(int seasonNumber) {
    if (seasonNumber < 0) {
      LogError($"Unable to remove season #{seasonNumber} : Season number {seasonNumber} is invalid");
      return false;
    }

    try {
      _Lock.EnterWriteLock();
      int Index = Seasons.FindIndex(s => s.Number == seasonNumber);
      if (Index >= 0) {
        Seasons.RemoveAt(Index);
        return true;
      }
      Logger.LogError($"Unable to remove season #{seasonNumber} : Season is missing");
      return false;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to remove season #{seasonNumber}", ex);
      return false;
    } finally {
      _Lock.ExitWriteLock();
    }
  }

  /// <summary>
  /// Get all the seasons from the serie
  /// </summary>
  /// <returns>An enumeration of the seasons</returns>
  public IEnumerable<IMediaSerieSeason> GetSeasons() {
    try {
      _Lock.EnterReadLock();
      foreach (TMediaSerieSeason SeasonItem in Seasons) {
        yield return SeasonItem;
      }
    } finally {
      _Lock.ExitReadLock();
    }
  }

  /// <summary>
  /// Get on season from the serie
  /// </summary>
  /// <param name="index">The number of the season to retrieve</param>
  /// <returns>The season requested or <see langword="null"/> if any error</returns>
  public IMediaSerieSeason? GetSeason(int index) {
    if (index < 0) {
      LogError($"Unable to retrieve season #{index} : Season number {index} is invalid");
      return null;
    }

    try {
      _Lock.EnterReadLock();
      IMediaSerieSeason? RetVal = Seasons.FirstOrDefault(s => s.Number == index);
      if (RetVal is null) {
        LogError($"Unable to retrieve season #{index} : Season is missing");
        return null;
      }
      return RetVal;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to retrieve season #{index}", ex);
      return null;
    } finally {
      _Lock.ExitReadLock();
    }

  }

  /// <summary>
  /// Clear all the seasons from the serie (and the episodes from each seasons)
  /// </summary>
  public void Clear() {
    try {
      _Lock.EnterWriteLock();
      Seasons.ForEach(s => s.Clear());
      Seasons.Clear();
    } finally {
      _Lock.ExitWriteLock();
    }
  }
}
