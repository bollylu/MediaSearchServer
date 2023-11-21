namespace MediaSearch.Models;

public interface IMedia :
  IRecord,
  IDisposable,
  IAsyncDisposable,
  IToStringIndent {

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
  /// Indicate tha the object is invalid, i.e. contains invalid entry(ies) and/or has missing value(s)
  /// </summary>
  bool IsInvalid { get; }

  IMediaInfos MediaInfos { get; }
  IMediaSources MediaSources { get; }
  IMediaPictures MediaPictures { get; }

  List<string> Groups { get; }
  bool IsGroupMember { get; }

}

