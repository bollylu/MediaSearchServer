namespace MediaSearch.Models;

public interface IMedia : IRecord,
                          IMediaInfoGroupMembership,
                          IMediaInfoCreation,
                          IDisposable,
                          IAsyncDisposable,
                          IPictureContainer {

  string Name { get; }

  EMediaType MediaType { get; set; }

  /// <summary>
  /// Physical location(s) where the media can be found (should be >= 1, but 0 is allowed)
  /// </summary>
  TMediaSources MediaSources { get; }

  /// <summary>
  /// The info about the media in various languages
  /// </summary>
  TMediaInfos MediaInfos { get; }

  /// <summary>
  /// The date when the media was added to the library
  /// </summary>
  DateOnly DateAdded { get; set; }

}

