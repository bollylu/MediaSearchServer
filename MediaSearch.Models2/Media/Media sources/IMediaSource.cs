namespace MediaSearch.Models;

public interface IMediaSource : IToStringIndent {

  /// <summary>
  /// The common part of the storage path
  /// </summary>
  public string StorageRoot { get; set; }

  /// <summary>
  /// The path of the movie in the storage
  /// </summary>
  string StoragePath { get; set; }

  /// <summary>
  /// The filename used to store the media
  /// </summary>
  string FileName { get; set; }

  /// <summary>
  /// The extension of the filename (e.g.: mkv, pdf, ...)
  /// </summary>
  string FileExtension { get; set; }

  /// <summary>
  /// The size of the file containing the data
  /// </summary>
  long Size { get; set; }

  /// <summary>
  /// The date the data was acquired
  /// </summary>
  DateOnly DateAdded { get; set; }

  /// <summary>
  /// The date the data was created
  /// </summary>
  DateOnly CreationDate { get; set; }

  /// <summary>
  /// The year the data was created
  /// </summary>
  int CreationYear { get; }

  /// <summary>
  /// The type of media source
  /// </summary>
  EMediaType MediaSourceType { get; init; }

  /// <summary>
  /// A description of the source
  /// </summary>
  string Description { get; set; }

  /// <summary>
  /// The language of the source
  /// </summary>
  ELanguage Language { get; set; }
}


