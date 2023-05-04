namespace MediaSearch.Models;

/// <summary>
/// This defines a media source that is physical (book, dvd, cd, ...)
/// </summary>
public interface IMediaSourcePhysical : IMediaSource {

  /// <summary>
  /// The place where the source is
  /// </summary>
  public string StoragePlace { get; set; }

  /// <summary>
  /// The kind of storage
  /// </summary>
  public EStorageType StorageType { get; set; }

}
