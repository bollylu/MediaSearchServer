﻿namespace MediaSearch.Models;

public class TMediaSerieEpisode : AMedia, IMediaSerieEpisode {

  /// <summary>
  /// The numer of the episode, counting from first episode
  /// </summary>
  public int AbsoluteNumber { get; set; }

  /// <summary>
  /// The season where the episode belongs
  /// </summary>
  public int Season { get; set; }

  /// <summary>
  /// The number inside the season
  /// </summary>
  public int Number { get; set; }

  /// <summary>
  /// The type of serie
  /// </summary>
  public ESerieType SerieType { get; set; }

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSerieEpisode() : base() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaSerieEpisode>();
    MediaType = EMediaType.SerieEpisode;
  }
  public TMediaSerieEpisode(ILogger logger) : base(logger) {
    MediaType = EMediaType.SerieEpisode;
  }
  public TMediaSerieEpisode(TMediaSerieEpisode episode) : base(episode) {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaSerieEpisode>();
    AbsoluteNumber = episode.AbsoluteNumber;
    Season = episode.Season;
    Number = episode.Number;
    SerieType = episode.SerieType;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString() {
    return ToString(0);
  }

  public override string ToString(int indent) {
    StringBuilder RetVal = new(base.ToString(indent));
    RetVal.AppendIndent($"- {nameof(SerieType)} = {SerieType}", indent);
    RetVal.AppendIndent($"- {nameof(AbsoluteNumber)} = {AbsoluteNumber}", indent);
    RetVal.AppendIndent($"- {nameof(Season)} = {Season}", indent);
    RetVal.AppendIndent($"- {nameof(Number)} = {Number}", indent);
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------
}