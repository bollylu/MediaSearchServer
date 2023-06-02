using MediaSearch.Models2;

namespace MediaSearch.Models;

public interface IMedia : IRecord, IDisposable, IAsyncDisposable, IToStringIndent {

  /// <summary>
  /// A shortcut taken from MediaInfos title
  /// </summary>
  string Name { get; }

  /// <summary>
  /// The type of media
  /// </summary>
  EMediaType MediaType { get; set; }

  /// <summary>
  /// The date when the media was added to the library
  /// </summary>
  DateOnly DateAdded { get; set; }

  /// <summary>
  /// The default language for getting data from language based values
  /// </summary>
  ELanguage DefaultLanguage { get; set; }

  /// <summary>
  /// The info(s) about the media in various languages (must be >= 1)
  /// </summary>
  IMediaInfos MediaInfos { get; set; }

  /// <summary>
  /// Physical location(s) where the media can be found (must be >= 0, should be >= 1)
  /// </summary>
  IMediaSources MediaSources { get; set; }

  /// <summary>
  /// The pictures associated with the media, in various languages (must be >= 0; should be >= 1)
  /// </summary>
  IMediaPictures MediaPictures { get; set; }

  /// <summary>
  /// Indicate tha the object is invalid, i.e. contains invalid entry(ies) and/or has missing value(s)
  /// </summary>
  bool IsInvalid { get; }

}

