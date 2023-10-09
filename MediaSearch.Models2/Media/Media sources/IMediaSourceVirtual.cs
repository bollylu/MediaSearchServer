namespace MediaSearch.Models;

/// <summary>
/// This defines a media source where the content is virtualized to a file
/// </summary>
public interface IMediaSourceVirtual : IMediaSource {

  /// <summary>
  /// The common part of the storage path
  /// e.g. "\\server\share"
  /// </summary>
  string StorageRoot { get; set; }

  /// <summary>
  /// The path of the movie in the storage
  /// e.g. "movies"
  /// </summary>
  string StoragePath { get; set; }

  /// <summary>
  /// The filename used to store the media
  /// e.g. "nice movie.mkv"
  /// </summary>
  string FileName { get; set; }

  /// <summary>
  /// The extension of the filename (without the dot)
  /// e.g.: "mkv", "pdf", ...
  /// </summary>
  string FileExtension { get; set; }

  /// <summary>
  /// The complete full file name, including path and extension
  /// </summary>
  string FullFileName { get; }

  /// <summary>
  /// The size of the file containing the data (in bytes)
  /// </summary>
  long Size { get; set; }

  IMediaStreams MediaStreams { get; }

}
