namespace MediaSearch.Models2;

public interface IFilterOutputDate {

  /// <summary>
  /// The minimum output date for the media (limit is included)
  /// </summary>
  int OutputDateMin { get; }

  /// <summary>
  /// The maximum output date for the media (limit is included)
  /// </summary>
  int OutputDateMax { get; }


}
