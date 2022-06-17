namespace MediaSearch.Models;

public interface IMediaSource {
  public string RootStorage { get; set; }
  /// <summary>
  /// Extended version of ToString with an optional indent size
  /// </summary>
  /// <param name="indent">The number of spaces to indent content</param>
  /// <returns>A string representation of the object</returns>
  public string ToString(int indent);

  /// <summary>
  /// The type of media referenced by this source
  /// </summary>
  public Type? MediaType { get; }
}

public interface IMediaSource<RECORD> : IMediaSource where RECORD : class, IMSRecord {

}

public interface IMediaSourceGeneric : IMediaSource<IMSRecord> { }
public interface IMediaSourceMovie : IMediaSource<IMovie> { }
